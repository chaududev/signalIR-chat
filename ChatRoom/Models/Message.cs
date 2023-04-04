namespace ChatRoom.Models
{
    public class Message
    {
        public int Id { get; private set; }
        public string Content { get; private set; }
        public DateTime Timestamp { get; private set; }
        public User FromUser { get; private set; }
        public int ToRoomId { get; private set; }
        public Room ToRoom { get; private set; }

        public Message()
        {
        }

        public Message(string content, User fromUser, Room toRoom, DateTime timestamp)
        {
            Content = content;
            Timestamp = timestamp;
            FromUser = fromUser;
            ToRoom = toRoom;
            ToRoomId = toRoom.Id;
        }
    }
}
