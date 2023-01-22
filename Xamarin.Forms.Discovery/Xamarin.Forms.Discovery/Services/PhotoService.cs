using PexelsDotNetSDK.Api;
using PexelsDotNetSDK.Models;
using System.Threading.Tasks;

namespace Discovery.Services;

public sealed class PhotoService
{
    private const string ApiKey = "563492ad6f91700001000001dfe6486d132a444e9e1aa767ea556fa5";
    private const string DefaultCategory = "Nature";

    public async Task<PhotoPage> GetCategorizedPhotos(string category, int pageNumber = 1, int pageSize = 50)
    {
        if (string.IsNullOrEmpty(category)) category = DefaultCategory;

        var httpClient = new PexelsClient(ApiKey);

        return await httpClient.SearchPhotosAsync(query: category, page: pageNumber, pageSize: pageSize);
    }

    public async Task<PhotoPage> GetCuratedPhotos(int pageNumber = 1, int pageSize = 50)
    {
        var httpClient = new PexelsClient(ApiKey);

        return await httpClient.CuratedPhotosAsync(pageSize: pageSize, page: pageNumber);
    }
}
