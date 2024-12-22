using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.LifecycleEvents;
using pin.Infrastructure;
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
            //var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //var appPath = Path.Combine(appDataPath, "pin");
            //var wwwrootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
            //var fi = new FileInfo(wwwrootPath);
            //fi.CopyTo(appPath);

            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();
            builder.Configuration.AddConfiguration(config);
            builder.Services.ConfigureWritable<UserOptions>();

            builder.Services.AddMauiBlazorWebView();


#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
