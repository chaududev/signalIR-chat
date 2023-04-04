namespace ChatRoom.Models
{
    public class Room
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public User Admin { get; private set; }
        public ICollection<Message> Messages { get; private set; } = new List<Message>();

        public Room()
        {
        }

        public Room(string name, User admin)
        {
            Name = name;
            Admin = admin;
        }
        public void setName(string name)
        {
            Name = name;
        }
    }
}
