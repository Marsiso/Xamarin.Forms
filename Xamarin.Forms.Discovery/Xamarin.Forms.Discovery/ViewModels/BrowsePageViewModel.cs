using Discovery.Models;
using PexelsDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Discovery;

namespace Discovery.ViewModels;

public sealed class BrowsePageViewModel : INotifyPropertyChanged
{
    private IList<PhotoEntity> photos = new List<PhotoEntity>();
    private string? searchTerm = "Nature";

    public string? SearchTerm
    {
        get => searchTerm;
        set
        {
            if (searchTerm != value)
            {
                searchTerm = value;
                _ = GetCategorizedPhotos(value);

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

    public Command<object> AddToFavouritesCommand { get; set; }
    public Command<object> AddToBlacklistCommand { get; set; }
    public Command<object> DownloadCommand { get; set; }
    public Command<object> LoadMoreItemsCommand { get; set; }

    public BrowsePageViewModel()
    {
        AddToFavouritesCommand = new Command<object>(AddToFavouritesCommandMethod);
        AddToBlacklistCommand = new Command<object>(AddToBlacklistCommandMethod);
        DownloadCommand = new Command<object>(DownloadCommandMethod);
        LoadMoreItemsCommand = new Command<object>(LoadMoreItemsCommandMethod);
    }

    public async Task GetCategorizedPhotos(string? searchTerm = "Nature", int pageNumber = 1)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = "Nature";
        }

        if (pageNumber < 1)
        {
            return;
        }

        var photoEntities = await App.DatabaseService.GetAllPhotos();
        var temp = new List<PhotoEntity>(photoEntities.Where(photo =>
            photo.IsBlackListed == false &&
            photo.IsFavourite == false &&
            photo.Category == searchTerm));

        try
        {
            while (temp.Count < 50)
            {
                var photoPageTask = App.PhotoService.GetCategorizedPhotos(category: searchTerm!, pageNumber: pageNumber);
                var photoPage = photoPageTask.Result;

                if (photoPage?.photos is null || photoPage.photos.Count == 0) break;

                foreach (var photo in photoPage.photos)
                {
                    if (photoEntities?.Count > 0 && photoEntities.Any(x => x.Id == photo.id))
                    {
                        continue;
                    }

                    var photoEntityToCreate = new PhotoEntity
                    {
                        Id = photo.id,
                        Alt = photo.alt,
                        Url = photo.source.portrait,
                        Photographer = photo.photographer,
                        Category = searchTerm
                    };

                    await App.DatabaseService.CreatePhoto(photoEntityToCreate);
                    temp.Add(photoEntityToCreate);
                }

                pageNumber++;
                PhotoPage = photoPage;
            }
        }
        catch (Exception)
        {
        }

        Photos = temp;
    }

    private void AddToFavouritesCommandMethod(object obj)
    {
        var photo = obj as PhotoEntity;
        if (photo is not null)
        {
            photo.IsVisible = false;
            photo.IsFavourite = true;
            photo.IsBlackListed = false;
            _ = App.DatabaseService.UpdatePhoto(photo);
            Photos = Photos.Where(x => x.Id != photo.Id).ToList();
        }
    }

    private void AddToBlacklistCommandMethod(object obj)
    {
        var photo = obj as PhotoEntity;
        if (photo is not null)
        {
            photo.IsVisible = false;
            photo.IsFavourite = false;
            photo.IsBlackListed = true;
            _ = App.DatabaseService.UpdatePhoto(photo);
            Photos = Photos.Where(x => x.Id != photo.Id).ToList();
        }
    }

    private void DownloadCommandMethod(object obj)
    {
        var photo = obj as PhotoEntity;
        if (photo is not null) GetPhotoFromUrl(photo.Id.ToString(), photo.Url);
    }

    private async void LoadMoreItemsCommandMethod(object obj) => await GetCategorizedPhotos(this.SearchTerm, PhotoPage is not null ? PhotoPage.page + 1 : 1);

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void GetPhotoFromUrl(string photoName, string url)
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