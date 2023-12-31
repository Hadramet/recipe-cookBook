using System;
using System.Collections.Generic;
using System.IO;
using CookBook.Helpers;
using CookBook.Model;

namespace CookBook.Services
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly string _repositoryPath;
        private readonly List<Recipe> _recipes = new();

        public RecipeRepository(string repositoryPath)
        {
            if (string.IsNullOrEmpty(repositoryPath))
                throw new ArgumentNullException(nameof(repositoryPath));

            _repositoryPath = repositoryPath;
            
            InitializeRepository();
            InitializeRecipes();
        }


        public IEnumerable<Recipe> GetRecipes()
        {
            return _recipes;
        }

        public Recipe GetRecipeById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            var recipe = _recipes.Find(r => r.Id == id);
            return recipe;
        }

        public void AddRecipe(Recipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            try
            {
                recipe.Id = Guid.NewGuid();
                string filePath = RecipeHelper.WriteRecipeToFile(recipe, _repositoryPath);
                recipe.FileName = Path.GetFileName(filePath);
                _recipes.Add(recipe);
            }
            catch (Exception e)
            {
                throw new Exception($"Error while creating file for recipe {recipe.Name}", e);
            }

        }

        public void DeleteRecipe(Guid id)
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
