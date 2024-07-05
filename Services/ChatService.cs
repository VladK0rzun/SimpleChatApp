using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SimpleChatApp.Data;
using SimpleChatApp.Hubs;
using SimpleChatApp.Models;

namespace SimpleChatApp.Services
{
    public class ChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<Chat> CreateChatAsync(string name, string userId)
        {
            var chat = new Chat { Name = name, CreatedByUserId = userId };
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat> GetChatAsync(int id)
        {
            return await _context.Chats.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteChatAsync(int id, string userId)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null || chat.CreatedByUserId != userId)
            {
                throw new UnauthorizedAccessException("No permissions to delete this chat.");
            }

            // Notify clients that the chat is being deleted
            await _hubContext.Clients.Group(id.ToString()).SendAsync("ChatDeleted", id);

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }
    }
}
