using AutoMapper;
using ChatRoom.Hubs;
using ChatRoom.IService;
using ChatRoom.Models;
using ChatRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatRoom.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _service;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(IMessageService service, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _service = service;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get(int id)
        {
            try
            {
                var rs = _service.GetById(id);
                if (rs == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<Message, MessageViewModel>(rs));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Room/{roomName}")]
        public IActionResult GetMessages(string roomName)
        {
            try
            {
                var rs = _service.GetAll(roomName);
                if (rs == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(rs));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<Message>> Create(MessageAddViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                var msg = _service.Add(viewModel.Room, viewModel.Content, User.Identity.Name);
                var createdMessage = _mapper.Map<Message, MessageViewModel>(msg);
                await _hubContext.Clients.Group(viewModel.Room).SendAsync("newMessage", createdMessage);

                return CreatedAtAction(nameof(Get), new { id = msg.Id }, createdMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try 
            {
                _service.Delete(id,User.Identity.Name);
                await _hubContext.Clients.All.SendAsync("removeChatMessage", id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

