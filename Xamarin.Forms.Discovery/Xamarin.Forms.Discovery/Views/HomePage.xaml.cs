using Syncfusion.XForms.Backdrop;
using System.ComponentModel;
using Xamarin.Forms;

namespace Discovery.Views;

[DesignTimeVisible(false)]
public partial class HomePage : SfBackdropPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
}