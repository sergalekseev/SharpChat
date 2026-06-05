using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;
using SharpChat.Server.Services;

namespace SharpChat.Server.Controllers;

[Authorize]
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

    [HttpPost]
    public ActionResult<Chat> CreateMessage(MessageCreateDto newMessage)
    {
        try
        {
            var createdMessage = _messagesManager.CreateMessage(User, newMessage);

            if (createdMessage is null)
            {
                throw new InvalidOperationException("New message was not created");
            }

            return Ok(createdMessage);
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(ex.Message);
        }
    }
}
