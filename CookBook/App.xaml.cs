using System;
using System.Windows;
using CookBook.Helpers;
using CookBook.Services;
using CookBook.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook
{
    public partial class App : Application
    {
        public App()
        {
            InitializeAppEnvironment();
            Ioc.Default.ConfigureServices(ConfigureServices());
            InitializeComponent();
        }
        
        private static void InitializeAppEnvironment()
        {
            string environment = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string storagePath = System.IO.Path.Combine(environment, ".cookbook");

            if (!System.IO.Directory.Exists(storagePath))
            {
                System.IO.Directory.CreateDirectory(storagePath);
            }

            Environment.CurrentDirectory = storagePath;

            string recipeStorePath = System.IO.Path.Combine(storagePath, "recipes");
            if (!System.IO.Directory.Exists(recipeStorePath))
            {
                System.IO.Directory.CreateDirectory(recipeStorePath);
            }

            Current.Resources["RecipeStorePath"] = recipeStorePath;
            Current.Resources["StoragePath"] = storagePath;
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IRecipeRepository>(new RecipeRepository(GetRecipeStorePath()));
            services.AddSingleton<IRecipeService, RecipeService>();
            services.AddSingleton<IMessageHelper, MessageHelper>();
            services.AddSingleton<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
        public static string GetRecipeStorePath()
        {
            return Current.Resources["RecipeStorePath"] as string;
        }

        public static string GetStoragePath()
        {
            return Current.Resources["StoragePath"] as string;
        }
    }
}
