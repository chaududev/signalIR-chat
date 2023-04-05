using ChatRoom.IRepository;
using ChatRoom.IService;
using ChatRoom.Models;
using System.Linq.Expressions;

namespace ChatRoom.Service
{
    public class RoomService : IRoomService
    {
        readonly IBaseRepository<Room> _repositoryRoom;
        readonly IBaseRepository<User> _repositoryUser;

        public RoomService(IBaseRepository<Room> repositoryRoom, IBaseRepository<User> repositoryUser)
        {
            _repositoryRoom = repositoryRoom;
            _repositoryUser = repositoryUser;
        }

        public Room Add(string name, string username)
        {
            Room room = _repositoryRoom.FirstOrDefault(r => r.Name == name);
            if (room != null)
            {
                throw new Exception($"The entity with name {name} was exist.");
            }
            User user = _repositoryUser.FirstOrDefault(u => u.UserName == username);
            var entity = new Room(name,user);
            _repositoryRoom.Add(entity);
            return entity;

        }

        public void Delete(int id, string username)
        {
            Room entity = _repositoryRoom.FirstOrDefault(m => m.Id == id && m.Admin.UserName == username);
            if (entity == null)
            {
                throw new Exception($"The entity with ID {id} was not found.");
            }
            _repositoryRoom.Delete(entity);
        }

        public IEnumerable<Room> GetAll()
        {
            return _repositoryRoom.GetAll(new Expression<Func<Room, object>>[] { p => p.Admin }, null, null);
        }

        public Room GetById(int id)
        {
            Room entity = _repositoryRoom.GetById(id, null);
            if (entity == null)
            {
                throw new Exception($"The entity with ID {id} was not found.");
            }
            return entity;
        }

        public Room Update(int id, string name, string username)
        {
            Room room = _repositoryRoom.FirstOrDefault(r => r.Name == name);
            if (room != null)
            {
                throw new Exception($"The entity with name {name} was exist.");
            }
            Room entity = _repositoryRoom.GetAll(new Expression<Func<Room, object>>[] { p => p.Admin }, r => r.Id == id && r.Admin.UserName == username, null).FirstOrDefault();
            entity.setName(name);
            _repositoryRoom.Update(entity);
            return entity;
        }
    }
}
