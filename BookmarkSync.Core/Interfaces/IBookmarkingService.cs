using System.Net.Http;
using System.Threading.Tasks;
using BookmarkSync.Core.Entities;

namespace BookmarkSync.Core.Interfaces;

public interface IBookmarkingService
{
    /// <summary>
    /// Saves a bookmark to the bookmarking service.
    /// </summary>
    /// <param name="bookmark">A service's bookmark implementation.</param>
    public Task<HttpResponseMessage> Save(Bookmark bookmark);
}
