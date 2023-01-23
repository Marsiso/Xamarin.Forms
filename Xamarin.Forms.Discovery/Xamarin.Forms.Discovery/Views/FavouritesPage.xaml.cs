using Discovery.Models;
using Syncfusion.XForms.Backdrop;
using Syncfusion.XForms.Cards;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Discovery;
using Xamarin.Forms.Xaml;

namespace Discovery.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
[DesignTimeVisible(false)]
public partial class FavouritesPage : SfBackdropPage
{
    private PhotoEntity? SelectedPhotoEntity { get; set; } = default!;

    public FavouritesPage()
    {
        InitializeComponent();
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var app = Application.Current;

        if (app is null) return;
        switch (e.SelectedItem.ToString())
        {
            case "Home":
                app.MainPage = new NavigationPage(new HomePage());
                break;

            case "Browse":
                app.MainPage = new NavigationPage(new BrowsePage());
                break;

            case "Favourites":
                app.MainPage = new NavigationPage(new FavouritesPage());
                break;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await favouritesPageViewModel.GetFavouritePhotos();
    }

    private void SfCardLayout_CardTapped(object sender, TappedEventArgs e)
    {
        if (popupLayout is not null && e.Parameter is SfCardView cardView
            && cardView.BindingContext is PhotoEntity photoEntity)
        {
            SelectedPhotoEntity = photoEntity;
            popupLayout.IsOpen = true;
        }
    }

    private async void SfButton_RemoveFavouriteClicked(object sender, System.EventArgs e)
    {
        if (popupLayout is not null && SelectedPhotoEntity is not null)
        {
            SelectedPhotoEntity.IsVisible = true;
            SelectedPhotoEntity.IsFavourite = false;
            SelectedPhotoEntity.IsBlackListed = false;

            await App.DatabaseService.UpdatePhoto(SelectedPhotoEntity);
            favouritesPageViewModel.Photos = favouritesPageViewModel.Photos
                .Where(x => x.Id != SelectedPhotoEntity.Id)
                .ToList();

            SelectedPhotoEntity = null;
            popupLayout.IsOpen = false;
        }
    }

    private async void SfButton_BlacklistPhotoClicked(object sender, System.EventArgs e)
    {
        if (popupLayout is not null && SelectedPhotoEntity is not null)
        {
            SelectedPhotoEntity.IsVisible = false;
            SelectedPhotoEntity.IsFavourite = false;
            SelectedPhotoEntity.IsBlackListed = true;

            await App.DatabaseService.UpdatePhoto(SelectedPhotoEntity);
            favouritesPageViewModel.Photos = favouritesPageViewModel.Photos
                .Where(x => x.Id != SelectedPhotoEntity.Id)
                .ToList();

            SelectedPhotoEntity = null;
            popupLayout.IsOpen = false;
        }
    }

    private void SfButton_DownloadPhotoClicked(object sender, System.EventArgs e)
    {
        if (popupLayout is not null && SelectedPhotoEntity is not null)
        {
            favouritesPageViewModel.GetPhotoFromUrl(SelectedPhotoEntity.Id.ToString(), SelectedPhotoEntity.Url);

            SelectedPhotoEntity = null;
            popupLayout.IsOpen = false;
        }
    }
}