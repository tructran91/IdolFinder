using IdolFinder.Application.Images.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdolFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("download")]
        public async Task<IActionResult> DownloadImages()
        {
            var count = await _mediator.Send(new DownloadImagesQuery());
            return Ok(new { ImagesDownloaded = count });
        }
    }
}
