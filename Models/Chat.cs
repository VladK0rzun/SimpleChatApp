namespace SimpleChatApp.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public List<Message> Messages { get; set; }

        public Chat()
        {
            Messages = new List<Message>();
        }
    }
}
