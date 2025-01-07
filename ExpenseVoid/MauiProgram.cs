using ExpenseVoid.Interface;
using Microsoft.Extensions.Logging;
using ExpenseVoid.Services;
using Microsoft.Maui.Storage;
using ExpenseVoid.Provider;
using Microcharts.Maui;
using ExpenseVoid.Persistence;
using ExpenseVoid.Helper;

namespace ExpenseVoid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    // Custom Font
                    fonts.AddFont("Retro-Gaming.ttf", "Retro Gaming");
                });

            builder.Services.AddMauiBlazorWebView();

            // Core Services
            builder.Services.AddTransient<IUser,UserService>();
            builder.Services.AddTransient<IDebt, DebtService>();
            builder.Services.AddTransient<ISource, SourceService>();
            builder.Services.AddTransient<ITag, TagService>();
            builder.Services.AddTransient<ITransaction, TransactionService>();
            builder.Services.AddTransient<ITransactionType, TransactionTypeService>();
            builder.Services.AddTransient<IGroup, GroupService>();
            builder.Services.AddTransient<IProfile, ProfileService>();

            // Provider Services
            builder.Services.AddSingleton<ICurrency, TcbExchangeProvider>();
            builder.Services.AddSingleton<ICurrency, EcbExchangeProvider>();

            //Local Session Services
            builder.Services.AddSingleton<PreferencesStoreClone>();

            //Helper Services
            builder.Services.AddTransient<EmailToUserMap>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
