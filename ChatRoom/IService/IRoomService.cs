using ChatRoom.Models;

namespace ChatRoom.IService
{
    public interface IRoomService
    {
        Room GetById(int id);
        IEnumerable<Room> GetAll();
        Room Add(string name, string username);
        Room Update(int id, string name, string username);
        void Delete(int id, string username);
    }
}
