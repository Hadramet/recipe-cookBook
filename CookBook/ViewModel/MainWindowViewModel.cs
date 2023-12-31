using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CookBook.Helpers;
using CookBook.Model;

namespace CookBook.ViewModel
{

    public class MainWindowViewModel
    {
        private readonly IMessageHelper _messageHelper;
        private readonly List<Recipe> _recipeList = new();
        private readonly List<string> _typeList = new();

        public MainWindowViewModel() {}
        public MainWindowViewModel(IMessageHelper messageHelper) : this()
        {
            _messageHelper = messageHelper;
        }


        public IEnumerable<string> GetRecipeTypes()
        { 
            return _typeList;
        }


        public void RemoveRecipeType(string item)
        {
            _typeList.Remove(item);
        }

        public void AddRecipeType(string item)
        {
            _typeList.Add(item);
        }

        public void SortRecipeTypes()
        {
            _typeList.Sort();
        }

        public void ReadRecipeFromFile(string fileName)
        {
            var recipe = new Recipe {FileName = fileName};
            var ingredient = new Ingredient();
            var reader = new XmlTextReader(fileName);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        string value = reader.Value;
                        switch (reader.Name)
                        {
                            case ("Name"):
                                recipe.Name = value;
                                break;
                            case ("Type"):
                                recipe.Type = value;
                                break;
                            case ("Text"):
                                recipe.Text = value;
                                break;
                            case ("Ingr"):
                                ingredient = new Ingredient {Name = value};
                                break;
                            case ("Col"):
                                ingredient.Col = value;
                                break;
                            case ("Ed"):
                                ingredient.Ed = value;
                                recipe.Ingredients.Add(ingredient);
                                break;
                        }
                    }
                }
            }
            reader.Close();
            _recipeList.Add(recipe);
        }

        public Recipe GetRecipeByFileName(string fileName)
        {
            return _recipeList.FirstOrDefault(recipe => recipe.FileName == fileName);
        }

        public IEnumerable<Recipe> GetRecipes()
        {
            return _recipeList;
        }

        public void RemoveRecipeByFileName(string name)
        {
            var item = _recipeList.First(recipe => recipe.FileName.Equals(name));
            _recipeList.Remove(item);
        }

        public void RemoveRecipe(int index)
        {
            if (index < 0 || index >= _recipeList.Count) return;
            
            const string message = "The recipe will be permanently deleted. Do you want me to continue?";
            const string title = "Delete a recipe";

            if (!_messageHelper.ShowMessage(message, title)) return;

            var rec = _recipeList[index];
            File.Delete(rec.FileName);
            RemoveRecipeType(rec.Type);
            RemoveRecipeByFileName(rec.FileName);
        }

        public void InitializeStorage(string storagePath)
        {
            if (string.IsNullOrEmpty(storagePath)) return;
            if (!Directory.Exists(storagePath)) return;
            string[] files = Directory.GetFiles(storagePath, "*.xml");
            foreach (string file in files) ReadRecipeFromFile(file);
        }
    }
}
