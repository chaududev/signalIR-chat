using ChatRoom.IRepository;
using ChatRoom.IService;
using ChatRoom.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ChatRoom.Service
{
    public class MessageService : IMessageService
    {
        readonly IBaseRepository<Message> _repository;
        readonly IBaseRepository<Room> _repositoryRoom;
        readonly IBaseRepository<User> _repositoryUser;

        public MessageService(IBaseRepository<Message> repository, IBaseRepository<Room> repositoryRoom, IBaseRepository<User> repositoryUser)
        {
            _repository = repository;
            _repositoryRoom = repositoryRoom;
            _repositoryUser = repositoryUser;
        }

        public Message Add(string roomName, string content,string username)
        {
            User user = _repositoryUser.FirstOrDefault(u => u.UserName == username);
            Room room = _repositoryRoom.FirstOrDefault(r => r.Name == roomName);
            if (room == null)
            {
                throw new Exception($"The room with name {roomName} was not found.");
            }
            var msg = new Message(Regex.Replace(content, @"<.*?>", string.Empty), user, room, DateTime.Now);
            _repository.Add(msg);
            return msg;

        }

        public void Delete(int id, string username)
        {
            Message entity = _repository.FirstOrDefault(m => m.Id == id && m.FromUser.UserName == username);
            if (entity == null)
            {
                throw new Exception($"The entity with ID {id} was not found.");
            }
            _repository.Delete(entity);
        }

        public IEnumerable<Message> GetAll(string roomName)
        {
            var room = _repositoryRoom.FirstOrDefault(r => r.Name == roomName);
            if (room == null)
                throw new Exception($"The room with name {roomName} was not found.");

            Expression<Func<Message, bool>> filter = m => m.ToRoomId == room.Id;
            Expression<Func<Message, object>>[] includeProperties =
                {
                    p => p.FromUser,
                    p => p.ToRoom
                };
            return _repository.GetAll(includeProperties, filter, null);
        }

        public Message GetById(int id)
        {
            Message entity = _repository.GetById(id, null);
            if (entity == null)
            {
                throw new Exception($"The entity with ID {id} was not found.");
            }
            return entity;

        }
    }
}
