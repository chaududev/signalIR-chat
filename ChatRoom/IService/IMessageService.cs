using ChatRoom.Models;

namespace ChatRoom.IService
{
    public interface IMessageService
    {
        Message GetById(int id);
        IEnumerable<Message> GetAll(string roomName);
        Message Add(string roomName, string content, string username);
        void Delete(int id, string username);

    }
}
