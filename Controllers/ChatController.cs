using Microsoft.AspNetCore.Mvc;
using SimpleChatApp.Models;
using SimpleChatApp.Services;

namespace SimpleChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> CreateChat(string name, string userId)
        {
            var chat = await _chatService.CreateChatAsync(name, userId);
            return CreatedAtAction(nameof(GetChat), new { id = chat.Id }, chat);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChat(int id)
        {
            var chat = await _chatService.GetChatAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return chat;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id, string userId)
        {
            try
            {
                await _chatService.DeleteChatAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("There are no permissions to do the operation");
            }
        }
    }
}
