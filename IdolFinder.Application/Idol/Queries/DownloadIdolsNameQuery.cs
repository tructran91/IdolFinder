using MediatR;

namespace IdolFinder.Application.Idol.Queries
{
    public class DownloadIdolsNameQuery : IRequest<int>
    {
        public string Category { get; set; } = "1pondo";
    }
}
