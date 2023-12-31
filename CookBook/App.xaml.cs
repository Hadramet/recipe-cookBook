using System;
using System.Windows;

namespace CookBook
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeAppEnvironment();
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
