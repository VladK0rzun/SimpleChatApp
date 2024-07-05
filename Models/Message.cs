namespace SimpleChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
