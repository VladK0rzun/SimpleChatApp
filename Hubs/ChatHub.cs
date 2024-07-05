using Microsoft.AspNetCore.SignalR;
using SimpleChatApp.Data;
using SimpleChatApp.Models;

namespace SimpleChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(int chatId, string userId, string message)
        {
            var msg = new Message { ChatId = chatId, UserId = userId, Text = message };
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message);
        }

        public async Task JoinChat (int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        public async Task NotifyChatDeleted(int chatId)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ChatDeleted", chatId);
        }
    }
}
