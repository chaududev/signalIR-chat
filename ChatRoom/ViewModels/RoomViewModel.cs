using System.ComponentModel.DataAnnotations;

namespace ChatRoom.ViewModels
{
    public class RoomViewModel : RoomAddViewModel
    {
        public int Id { get; set; }
        public string Admin { get; set; }
    }
}
