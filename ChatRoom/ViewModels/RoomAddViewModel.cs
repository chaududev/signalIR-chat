using System.ComponentModel.DataAnnotations;

namespace ChatRoom.ViewModels
{
    public class RoomAddViewModel 
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
    }
}
