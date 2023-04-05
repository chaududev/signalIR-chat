using AutoMapper;
using ChatRoom.Data;
using ChatRoom.Hubs;
using ChatRoom.IService;
using ChatRoom.Models;
using ChatRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _service;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public RoomsController(IRoomService service, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _service = service;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomViewModel>>> Get()
        {
            try
            {
                var rs = _service.GetAll();
                if (rs == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<IEnumerable<Room>, IEnumerable<RoomViewModel>>(rs));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
                return Ok(_mapper.Map<Room, RoomViewModel>(rs));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Create(RoomAddViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                var msg = _service.Add(viewModel.Name, User.Identity.Name);
                var createdRoom = _mapper.Map<Room, RoomViewModel>(msg);
                await _hubContext.Clients.All.SendAsync("addChatRoom", createdRoom);

                return CreatedAtAction(nameof(Get), new { id = msg.Id }, createdRoom);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, RoomAddViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity(ModelState);
                }
                var msg = _service.Update(id,viewModel.Name, User.Identity.Name);
                var updatedRoom = _mapper.Map<Room, RoomViewModel>(msg);
                await _hubContext.Clients.All.SendAsync("updateChatRoom", updatedRoom);
                return Ok();
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
                Room room = _service.GetById(id);
                _service.Delete(id, User.Identity.Name);
                await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
                await _hubContext.Clients.Group(room.Name).SendAsync("onRoomDeleted");
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
