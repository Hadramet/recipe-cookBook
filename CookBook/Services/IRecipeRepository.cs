using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CookBook.Model;

namespace CookBook.Services;

public interface IRecipeRepository
{
    Task<IEnumerable<Recipe>> GetRecipes();
    Task<Recipe> GetRecipeById(Guid id);
    Task AddRecipe(Recipe recipe);
    Task DeleteRecipe(Guid id);
}