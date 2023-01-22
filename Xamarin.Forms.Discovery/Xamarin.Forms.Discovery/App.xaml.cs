using Discovery.Services;
using Discovery.Views;
using System;
using System.IO;

namespace Xamarin.Forms.Discovery;

public partial class App : Application
{
    private static DatabaseService databaseService = default!;
    private static PhotoService carouselService = default!;

    public static DatabaseService DatabaseService
    {
        get => databaseService ??= new DatabaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoStore.db3"));
    }

    public static PhotoService PhotoService
    {
        get => carouselService ??= new PhotoService();
    }

    public App()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("OTgyOTI2QDMyMzAyZTM0MmUzMEhFcTBsbGN3N005QzZkRnFZbnNSOVlleFFFOVJwVHAwRVhSWHp2ZEdPK2c9");

        InitializeComponent();

        MainPage = new NavigationPage(new HomePage());
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
}
