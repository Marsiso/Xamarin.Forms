using Discovery.Models;
using PexelsDotNetSDK.Models;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms.Discovery;

namespace Discovery.ViewModels;

public sealed class HomePageViewModel : INotifyPropertyChanged
{
    private ObservableCollection<PhotoEntity> photos = new();

    public PhotoPage? PhotoPage { get; set; }

    public ObservableCollection<PhotoEntity> Photos
    {
        get => photos;
        set => photos = value;
    }

    public HomePageViewModel()
    {
        photos = new ObservableCollection<PhotoEntity>();

        Task.Run(() => { this.InitializeAsync().Wait(); });
    }

    public async Task InitializeAsync()
    {
        const string category = "Curated";

        // Database Access
        try
        {
            var photoEntities = await App.DatabaseService.GetAllPhotos();
            photoEntities?.ForEach(photo => photos.Add(photo));
        }
        catch (Exception)
        {
        }

        // Api Call
        try
        {
            var photoPage = await App.PhotoService.GetCuratedPhotos();
            PhotoPage = photoPage;

            PhotoPage.photos
                .Where(photo => photos.All(p => p.Id != photo.id))
                .ForEach(async photo =>
                {
                    if (photo is not null)
                    {
                        var photoEntity = new PhotoEntity
                        {
                            Id = photo.id,
                            Alt = photo.alt,
                            Url = photo.source.portrait,
                            Photographer = photo.photographer,
                            Category = category
                        };

                        photos.Add(photoEntity);
                        await App.DatabaseService.CreatePhoto(photoEntity);
                    }
                });
        }
        catch (Exception)
        {
        }

        //photos.Reverse();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
