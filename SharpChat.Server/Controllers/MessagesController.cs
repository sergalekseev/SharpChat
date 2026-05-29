using Microsoft.AspNetCore.Mvc;
using SharpChat.Core.Models;
using SharpChat.Server.Services;

namespace SharpChat.Server.Controllers;

[ApiController]
[Route("api/[controller]")] // "api/messages"
public class MessagesController : ControllerBase
{
    private readonly ILogger<ChatsController> _logger;
    private readonly IMessagesManager _messagesManager;

    public MessagesController(ILogger<ChatsController> logger, IMessagesManager messagesManager)
    {
        _logger = logger;
        _messagesManager = messagesManager;
    }

    [HttpGet("list/{chatId}")] //messages/list/{chatId}
    public ActionResult<IEnumerable<Chat>?> GetAll(int chatId)
    {
        return Ok(_messagesManager.GetAll(chatId) ?? []);
    }
}
