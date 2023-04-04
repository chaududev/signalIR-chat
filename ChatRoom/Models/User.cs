using Microsoft.AspNetCore.Identity;

namespace ChatRoom.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; private set; }
        public ICollection<Room> Rooms { get; private set; }
        public ICollection<Message> Messages { get; private set; }

        public void setName(string fullName)
        {
            FullName = fullName;
        }
    }
}
