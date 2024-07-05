namespace SimpleChatApp.Services
{
    using Moq;
    using SimpleChatApp.Data;
    using SimpleChatApp.Hubs;
    using SimpleChatApp.Models;
    using SimpleChatApp.Services;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using System.Threading.Tasks;
    using System;

    public class ChatServiceTests
    {
        private readonly ChatService _chatService;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<IHubContext<ChatHub>> _mockHubContext;

        public ChatServiceTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockHubContext = new Mock<IHubContext<ChatHub>>();

            _chatService = new ChatService(_mockContext.Object, _mockHubContext.Object);
        }

        [Fact]
        public async Task DeleteChatAsync_WithCorrectUserId_DeletesChat()
        {
            // Arrange
            var chat = new Chat { Id = 1, CreatedByUserId = "user1" };
            _mockContext.Setup(c => c.Chats.FindAsync(1)).ReturnsAsync(chat);

            // Act
            await _chatService.DeleteChatAsync(1, "user1");

            // Assert
            _mockContext.Verify(c => c.Chats.Remove(chat), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
            _mockHubContext.Verify(h => h.Clients.Group("1").SendAsync("ChatDeleted", It.IsAny<object[]>(), default), Times.Once());
        }

    }
}
