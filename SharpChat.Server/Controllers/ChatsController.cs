using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpChat.Core.DataContracts;
using SharpChat.Core.Models;
using SharpChat.Server.Services;

namespace SharpChat.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")] // "api/chats"
public class ChatsController : ControllerBase
{
    private readonly ILogger<ChatsController> _logger;
    private readonly IChatsManager _chatsManager;

    public ChatsController(ILogger<ChatsController> logger, IChatsManager chatsManager)
    {
        _logger = logger;
        _chatsManager = chatsManager;
    }

    [HttpGet("list")]
    public ActionResult<IEnumerable<Chat>> GetAll()
    {
        var list = _chatsManager.GetAll();

        if (list is null)
        {
            throw new InvalidDataException("Data repository is not exists");
        }

        return Ok(list);
    }

    [HttpGet("{chatId}")]
    public ActionResult<Chat> GetById(int chatId)
    {
        var chat = _chatsManager.GetById(chatId);

        if (chat is null)
        {
            return NotFound($"Chat with id '{chatId}' not found");
        }

        return chat;
    }

    [HttpPost]
    public ActionResult<Chat> CreateChat(ChatCreateDto newChat)
    {
        try
        {
            var createdChat = _chatsManager.CreateChat(newChat);

            if (createdChat is null)
            {
                throw new InvalidOperationException("New chat was not created");
            }

            return Ok(createdChat);
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

    [HttpPut]
    public ActionResult<Chat> UpdateChat(ChatUpdateDto chatToUpdate)
    {
        try
        {
            var updatedChat = _chatsManager.UpdateChat(chatToUpdate);

            if (updatedChat is null)
            {
                throw new InvalidOperationException("Chat after update is not exist");
            }

            return Ok(updatedChat);
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

