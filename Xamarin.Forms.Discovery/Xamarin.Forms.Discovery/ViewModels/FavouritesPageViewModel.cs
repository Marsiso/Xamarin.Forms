using Discovery.Models;
using PexelsDotNetSDK.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms.Discovery;

namespace Discovery.ViewModels;

public sealed class FavouritesPageViewModel : INotifyPropertyChanged
{
    private IList<PhotoEntity> photos = new List<PhotoEntity>();
    private string? searchTerm;

    public string? SearchTerm
    {
        get => searchTerm;
        set
        {
            if (searchTerm != value)
            {
                searchTerm = value;
                _ = GetFavouritePhotos(value);

                OnPropertyChanged(nameof(SearchTerm));
            }
        }
    }


    public PhotoPage? PhotoPage { get; set; }

    public IList<PhotoEntity> Photos
    {
        get => photos;
        set
        {
            if (value is not null)
            {
                photos = value;

                OnPropertyChanged(nameof(Photos));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public FavouritesPageViewModel()
    {

    }

    public async Task GetFavouritePhotos(string? searchTerm = default!)
    {
        var photoEntities = await App.DatabaseService.GetAllPhotos();
        List<PhotoEntity>? temp;
        if (string.IsNullOrEmpty(searchTerm))
        {
            temp = new List<PhotoEntity>(photoEntities.Where(photo => photo.IsFavourite is true));
        }
        else
        {
            searchTerm = searchTerm!.ToLower();
            //var searchTerms = searchTerm.Split(' ');
            temp = new List<PhotoEntity>(photoEntities.Where(photo => photo.IsFavourite is true
                && (photo.Photographer.ToLower().Contains(searchTerm) || photo.Alt.ToLower().Contains(searchTerm))));
        }

        Photos = temp;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void GetPhotoFromUrl(string photoName, string url)
    {
        using var webClient = new WebClient();
        var imageBytes = webClient.DownloadData(url);
        if (imageBytes is not null && imageBytes.Length > 0)
        {
            SavePhoto(photoName, imageBytes, "Photos");
        }
    }

    private void SavePhoto(string photoName, byte[] data, string location = "temp")
    {
        /*var downloadsPath = Android.App.Application.Context.GetExternalFilesDir(string.Empty).AbsolutePath;
        if (Path.HasExtension(photoName) is false)
        {
            photoName += ".jpeg";
        }

        var filePath = Path.Combine(downloadsPath, photoName);

        File.WriteAllBytes(filePath, data);*/
    }
}
