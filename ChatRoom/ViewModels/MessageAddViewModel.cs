using System.ComponentModel.DataAnnotations;

namespace ChatRoom.ViewModels
{
    public class MessageAddViewModel
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public string Room { get; set; }
    }
}
