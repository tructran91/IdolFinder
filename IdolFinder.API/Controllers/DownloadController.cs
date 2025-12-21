using IdolFinder.Application.Idol.Queries;
using IdolFinder.Application.Images.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdolFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DownloadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("DownloadIdolsName")]
        public async Task<IActionResult> DownloadIdolsName()
        {
            var count = await _mediator.Send(new DownloadIdolsNameQuery());
            return Ok(new { ImagesDownloaded = count });
        }

        [HttpGet("DownloadImages")]
        public async Task<IActionResult> DownloadImages()
        {
            var count = await _mediator.Send(new DownloadImagesQuery());
            return Ok(new { ImagesDownloaded = count });
        }
    }
}
