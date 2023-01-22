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
        if (app is not null)
        {
            if (e.SelectedItem.ToString() == "Home")
            {
                app.MainPage = new NavigationPage(new HomePage());
            }
            else if (e.SelectedItem.ToString() == "Browse")
            {
                app.MainPage = new NavigationPage(new BrowsePage());
            }
            else if (e.SelectedItem.ToString() == "Favourites")
            {
                app.MainPage = new NavigationPage(new FavouritesPage());
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await browsePageViewModel.GetCategorizedPhotos();
    }
}