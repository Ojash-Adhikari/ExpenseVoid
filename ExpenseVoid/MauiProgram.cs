using ExpenseVoid.Interface;
using Microsoft.Extensions.Logging;
using ExpenseVoid.Services;
using ExpenseVoid.Provider;

namespace ExpenseVoid
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

            // Provider Services
            builder.Services.AddSingleton<ICurrency, TcbExchangeProvider>();
            builder.Services.AddSingleton<ICurrency, EcbExchangeProvider>();


#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
