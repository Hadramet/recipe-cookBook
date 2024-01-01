using System;
using System.Collections.Generic;
using CookBook.Model;

namespace CookBook.Services;

public interface IRecipeService
{
    IEnumerable<Recipe> GetRecipeList();
    IEnumerable<string> GetRecipeTypes();
    Recipe GetRecipeById(Guid id);
    void SaveRecipe(Recipe recipe);
    void RemoveRecipe(Guid id);
}