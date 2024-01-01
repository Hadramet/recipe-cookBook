using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CookBook.Helpers;
using CookBook.Model;
using CookBook.Services;

namespace CookBook.ViewModel
{

    public class MainWindowViewModel
    {
        private readonly IMessageHelper _messageHelper;
        private readonly IRecipeService _recipeService;

        public MainWindowViewModel(IMessageHelper messageHelper, IRecipeService recipeService)
        {
            _messageHelper = messageHelper;
            _recipeService = recipeService;
        }


        public IEnumerable<string> GetRecipeTypes()
        { 
            return _recipeService.GetRecipeTypes();
        }


        public Recipe GetRecipeByFileName(string fileName)
        {
            var recipes = _recipeService.GetRecipeList();
            var recipe = recipes.FirstOrDefault(r => r.FileName == fileName);
            return recipe;
        }

        public IEnumerable<Recipe> GetRecipes()
        {
            return _recipeService.GetRecipeList();
        }

        public void RemoveRecipeByFileName(string name)
        {
            var recipes = _recipeService.GetRecipeList();
            var recipe = recipes.FirstOrDefault(r => r.FileName == name);
            if (recipe == null) return;
            _recipeService.RemoveRecipe(recipe.Id);
        }

        public void RemoveRecipe(int index)
        {
            var recipes = _recipeService.GetRecipeList();
            var enumerable = recipes.ToList();
            if (index < 0 || index >= enumerable.Count) return;
            
            const string message = "The recipe will be permanently deleted. Do you want me to continue?";
            const string title = "Delete a recipe";

            if (!_messageHelper.ShowMessage(message, title)) return;

            var recipe = enumerable[index];
            _recipeService.RemoveRecipe(recipe.Id);
        }

    }
}
