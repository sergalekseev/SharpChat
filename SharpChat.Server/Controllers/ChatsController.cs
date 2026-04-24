using Microsoft.AspNetCore.Mvc;
using SharpChat.Core.Models;

namespace SharpChat.Server.Controllers;

[ApiController]
[Route("api/[controller]")] // "api/chats"
public class ChatsController : ControllerBase
{
    private readonly ILogger<ChatsController> _logger;
    private Chat[] _chats =
        [
            new Chat(){ Id = 0, Title = "Anna", LastMessage = new Message() { Text = "Hello" }  },
            new Chat(){ Id = 1, Title = "Alex", LastMessage = new Message() { Text = "Hi" }  },
            new Chat(){ Id = 2, Title = "Sergei", LastMessage = new Message() { Text = "How are you?" }  },
        ];

    public ChatsController(ILogger<ChatsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("list")]
    public ActionResult<IEnumerable<Chat>> GetAll()
    {
        return _chats;
    }

    [HttpGet("{chatId}")]
    public ActionResult<Chat> GetById(int chatId)
    {
        var chat = _chats.FirstOrDefault(c => c.Id == chatId);

        if (chat is null)
        {
            return NotFound($"Chat with id '{chatId}' not found");
        }

        return chat;
    }

}

