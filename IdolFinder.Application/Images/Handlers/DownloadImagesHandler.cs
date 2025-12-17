using IdolFinder.Application.Configurations.Options;
using IdolFinder.Application.Images.Endpoints;
using IdolFinder.Application.Images.Queries;
using MediatR;
using Microsoft.Extensions.Options;

namespace IdolFinder.Application.Images.Handlers
{
    public class DownloadImagesHandler : IRequestHandler<DownloadImagesQuery, int>
    {
        private readonly ImageSourceOptions _options;
        private readonly LockettsEndpointHandler _lockettsHandler;
        private readonly AbcEndpointHandler _abcHandler;

        public DownloadImagesHandler(
            IOptions<ImageSourceOptions> options,
            LockettsEndpointHandler lockettsHandler,
            AbcEndpointHandler abcHandler)
        {
            _options = options.Value;
            _lockettsHandler = lockettsHandler;
            _abcHandler = abcHandler;
        }

        public async Task<int> Handle(DownloadImagesQuery request, CancellationToken cancellationToken)
        {
            int total = 0;

            total += await _lockettsHandler.HandleAsync(_options.LockettsEndpoint, cancellationToken);
            total += await _abcHandler.HandleAsync(_options.AbcEndpoint, cancellationToken);

            return total;
        }
    }
}
