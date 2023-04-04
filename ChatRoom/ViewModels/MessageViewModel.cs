using System.ComponentModel.DataAnnotations;

namespace ChatRoom.ViewModels
{
    public class MessageViewModel : MessageAddViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string FromUserName { get; set; }
        public string FromFullName { get; set; }
        
    }
}
