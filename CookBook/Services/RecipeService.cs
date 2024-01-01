using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBook.Helpers;
using CookBook.Model;

namespace CookBook.Services
{
    // TODO : logging 
    public class RecipeService : IRecipeService
    {
        private readonly IEnumerable<string> _recipeTypes = new List<string> { "Vegan", "Vegetarian", "Meat", "Fish", "Other"};
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        public  IEnumerable<Recipe> GetRecipeList()
        {
            var task = _recipeRepository.GetRecipes();

            if (!task.IsCompletedSuccessfully)
            {
                return Enumerable.Empty<Recipe>();
            }

            return task.Result;
        }

        public IEnumerable<string> GetRecipeTypes()
        {
            return _recipeTypes;
        }
        public Recipe GetRecipeById(Guid id)
        {
            var task = _recipeRepository.GetRecipeById(id);

            if (!task.IsCompletedSuccessfully)
            {
                return null;
            }

            return  task.Result;
        }

        public void SaveRecipe(Recipe recipe)
        {
            if (!_recipeTypes.Contains(recipe.Type))
                recipe.Type = _recipeTypes.Last();

            var task = _recipeRepository.AddRecipe(recipe);

            if (!task.IsCompletedSuccessfully)
            {
                throw new Exception("Recipe was not saved");
            }
        }

        public void RemoveRecipe(Guid id)
        {
            var task = _recipeRepository.DeleteRecipe(id);

            if (!task.IsCompletedSuccessfully)
            {
                throw new Exception("Recipe was not deleted");
            }

        }
    }
}
