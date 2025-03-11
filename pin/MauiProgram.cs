using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.LifecycleEvents;
using pin.Infrastructure;
using pin.Services;
using System.Reflection;

namespace pin
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>  
        {  
            events.AddWindows(wndLifeCycleBuilder =>  
            {  
                wndLifeCycleBuilder.OnWindowCreated(window =>  
                {  
                //    window.ExtendsContentIntoTitleBar = false;  
                    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    Microsoft.UI.WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);  
                    var _appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);  
                   // _appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);   
                 //if you want to full screen, you can use this line
                (_appWindow.Presenter as Microsoft.UI.Windowing.OverlappedPresenter).Maximize();   
                //if you want to Maximize the window, you can use this line                    
                });  
            });  
        });
#endif
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var appPath = Path.Combine(appDataPath, "pin");
            var wwwrootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot","appsettings.json");
            if(!Directory.Exists(appPath))
                Directory.CreateDirectory(appPath);
            if (!File.Exists(Path.Combine(appPath, "appsettings.json")))
                File.Copy(wwwrootPath, Path.Combine(appPath, "appsettings.json"));

            var config = new ConfigurationBuilder()
                .SetBasePath(appPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();
            builder.Configuration.AddConfiguration(config);
            builder.Services.ConfigureWritable<UserOptions>();

            builder.Services.AddMauiBlazorWebView();

            ConfigureServices(builder.Services);

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IProviderService, ProviderService>();
        }
    }
}
