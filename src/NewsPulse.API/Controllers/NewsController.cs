using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsPulse.Application.Features.News.Commands;

namespace NewsPulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController(ISender sender) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Create(CreateNewsCommand command)
    {
        var createdNewsId = await sender.Send(command);

        return CreatedAtAction(nameof(Create), new { id = createdNewsId }, createdNewsId);
    }
}
