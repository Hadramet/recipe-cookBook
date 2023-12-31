using System;
using System.Collections.Generic;
using System.Xml;
using CookBook.Model;

namespace CookBook.Helpers;

public static class RecipeHelper
{
    public static string WriteRecipeToFile(Recipe recipe, string repositoryPath)
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));
        if (string.IsNullOrEmpty(repositoryPath))
            throw new ArgumentNullException(nameof(repositoryPath));

        string fileName = $"{recipe.Id}.xml";
        string filePath = System.IO.Path.Combine(repositoryPath, fileName);

        var settings = new XmlWriterSettings { Indent = true };
        using var writer = XmlWriter.Create(filePath, settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("ListOfRecipes"); // TODO: change to "Recipe" later

        writer.WriteStartElement("RecipeData");
        writer.WriteAttributeString("Name", recipe.Name);
        writer.WriteAttributeString("Type", recipe.Type);
        writer.WriteAttributeString("Text", recipe.Text);
        writer.WriteAttributeString("Id", recipe.Id.ToString());

        if (recipe.Ingredients != null)
        {
            writer.WriteStartElement("Ingredients");
            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                if (ingredient.Name.Length != 0)
                {
                    writer.WriteStartElement("Ingredient");
                    writer.WriteAttributeString("Name", ingredient.Name);
                    writer.WriteAttributeString("Col", ingredient.Col);
                    writer.WriteAttributeString("Ed", ingredient.Ed);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();


        return filePath;
    }


    public static IEnumerable<Recipe> ParseRecipes(string[] files)
    {
        var list = new List<Recipe>();
        foreach (string file in files)
        {
            var recipe = new Recipe { FileName = file };
            var ingredient = new Ingredient();
            var reader = new XmlTextReader(file);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        string value = reader.Value;
                        switch (reader.Name)
                        {
                            case "Name":
                                recipe.Name = value;
                                break;
                            case "Type":
                                recipe.Type = value;
                                break;
                            case "Text":
                                recipe.Text = value;
                                break;
                            case "Ingr":
                                ingredient = new Ingredient { Name = value };
                                break;
                            case "Col":
                                ingredient.Col = value;
                                break;
                            case "Ed":
                                ingredient.Ed = value;
                                recipe.Ingredients.Add(ingredient);
                                break;
                            case "Id":
                                recipe.Id = Guid.TryParse(value, out Guid id) ? id : Guid.Empty;
                                break;
                        }
                    }
                }
            }
            reader.Close();
            list.Add(recipe);
        }
        return list;
    }
}