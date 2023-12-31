using System;
using System.Collections.Generic;
using CookBook.Model;

namespace CookBook.Services;

public interface IRecipeRepository
{
    IEnumerable<Recipe> GetRecipes();
    Recipe GetRecipeById(Guid id);
    void AddRecipe(Recipe recipe);
    void DeleteRecipe(Guid id);
}