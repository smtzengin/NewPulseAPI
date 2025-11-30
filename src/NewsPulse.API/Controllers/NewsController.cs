using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsPulse.Application.Features.News.Commands;
using NewsPulse.Application.Features.News.Commands.SyncNews;
using NewsPulse.Application.Features.News.Queries;

namespace NewsPulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController(ISender sender) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await sender.Send(new GetNewsByIdQuery(id));
        return result != null ? Ok(result) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateNewsCommand command)
    {
        var createdNewsId = await sender.Send(command);

        return CreatedAtAction(nameof(Create), new { id = createdNewsId }, createdNewsId);
    }
    [HttpPost("Sync")]
    public async Task<IActionResult> SyncElastic()
    {
        await sender.Send(new SyncNewsCommand());
        return Ok("Senkronizasyon işlemi başlatıldı. Arka planda işleniyor...");
    }
}
