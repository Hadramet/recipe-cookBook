using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CookBook.ViewModel
{

    public class MainWindowViewModel
    {
        private  List<Recipe> _recipeList = new();
        private  List<string> _typeList = new();

        public IEnumerable<string> GetItemsTree()
        { 
            return _typeList;
        }

        public IEnumerable<string> GetItemsList()
        {
            return _typeList;
        }

        public void RemoveItem(string item)
        {
            _typeList.Remove(item);
        }

        public void AddItem(string item)
        {
            _typeList.Add(item);
        }

        public void SortItems()
        {
            _typeList.Sort();
        }

        public void ReadRecipeFromFile(string fileName)
        {
            var recipe = new Recipe {FileName = fileName};
            var ingr = new Ingridient();
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
                                ingr = new Ingridient {Ingr = value};
                                break;
                            case ("Col"):
                                ingr.Col = value;
                                break;
                            case ("Ed"):
                                ingr.Ed = value;
                                recipe.Ingridients.Add(ingr);
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

    }
}
