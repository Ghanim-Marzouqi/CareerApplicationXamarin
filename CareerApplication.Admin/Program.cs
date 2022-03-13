using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CareerApplication.Admin.Services;
using CareerApplication.Core;
using CareerApplication.Core.Providers;
using Firebase.Auth;
using Firebase.Database;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace CareerApplication.Admin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            var firebaseSettings = new FirebaseSettings();
            var firebaseApiKey = new FirebaseConfig(firebaseSettings.ApiKey);
            var firebaseJson = JsonSerializer.Serialize(firebaseSettings);

            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(_ => new AuthProvider(new FirebaseAuthProvider(firebaseApiKey)));
            builder.Services.AddSingleton(_ => new DatabaseProvider(new FirebaseClient(firebaseSettings.RealtimeDatabaseUrl)));
            builder.Services.AddScoped<IBrowserStorageService, BrowserStorageService>();
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 3000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            await builder.Build().RunAsync();
        }
    }
}