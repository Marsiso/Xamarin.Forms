using Syncfusion.XForms.Backdrop;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Discovery.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
[DesignTimeVisible(false)]
public partial class BrowsePage : SfBackdropPage
{
    public BrowsePage()
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

        await browsePageViewModel.GetCategorizedPhotos();
    }
}