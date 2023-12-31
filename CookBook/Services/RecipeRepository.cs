using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CookBook.Helpers;
using CookBook.Model;

namespace CookBook.Services
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly string _repositoryPath;
        private readonly List<Recipe> _recipes = new();
        private readonly IEnumerable<string> _recipeTypes = new List<string> { "Vegan", "Vegetarian", "Meat", "Fish", "Other"};

        public RecipeRepository(string repositoryPath)
        {
            if (string.IsNullOrEmpty(repositoryPath))
                throw new ArgumentNullException(nameof(repositoryPath));

            _repositoryPath = repositoryPath;
            
            InitializeRepository();
            InitializeRecipes();
        }


        public Task<IEnumerable<Recipe>> GetRecipes()
        {
            return Task.FromResult(_recipes.AsEnumerable());
        }

        public Task<Recipe> GetRecipeById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            var recipe = _recipes.Find(r => r.Id == id);
            
            return Task.FromResult(recipe);
        }

        public Task AddRecipe(Recipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            try
            {
                recipe.Id = Guid.NewGuid();

                if (_recipeTypes.Contains(recipe.Type) == false)
                    recipe.Type = _recipeTypes.Last();

                string filePath = RecipeHelper.WriteRecipeToFile(recipe, _repositoryPath);
                recipe.FileName = Path.GetFileName(filePath);
                
                _recipes.Add(recipe);
            }
            catch (Exception e)
            {
                throw new Exception($"Error while creating file for recipe {recipe.Name}", e);
            }

            return Task.CompletedTask;
        }

        public Task DeleteRecipe(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var recipe = _recipes.Find(r => r.Id == id);
            if (recipe == null)
                throw new ArgumentException($"Recipe with id {id} not found");

            string filePath = Path.Combine(_repositoryPath, recipe.FileName);
            if (File.Exists(filePath)) File.Delete(filePath);
            else throw new FileNotFoundException($"File {filePath} not found");
            
            _recipes.Remove(recipe);

            return Task.CompletedTask;
        }

        
        private void InitializeRecipes()
        {
            string[] files = Directory.GetFiles(_repositoryPath, "*.xml");
            var recipeList = RecipeHelper.ParseRecipes(files);
            _recipes.AddRange(recipeList);
        }

        private void InitializeRepository()
        {
            if (!Directory.Exists(_repositoryPath))
            {
                Directory.CreateDirectory(_repositoryPath);
            }
        }
    }
}
