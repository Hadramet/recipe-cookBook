using System.Collections.Generic;

namespace CookBook.Model;

public class Recipe
{
    public List<Ingredient> Ingredients = new();
    public string FileName { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Text { get; set; }
}