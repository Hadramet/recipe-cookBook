using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.IO;
using CookBook.Model;


namespace CookBook
{
    public partial class AddWindow
    {
        private readonly ObservableCollection<Ingredient> _dataList = new ObservableCollection<Ingredient>();
        private readonly List<string> _weigthList = new List<string>(new[]{"gram","kilogram","teaspoon,","tablespoon","cup","milliliter","liter","pinch","piece","clove","jar"});

        public delegate void RecipeEventHandler(string fileName);
        public event RecipeEventHandler RecipeAddedSignal;


        public AddWindow(IEnumerable<string> typeList)
        {
            InitializeComponent();
            FillData();

            dataGrid1.ItemsSource = _dataList;
            _weigthList.Sort();
            dgCombo2.ItemsSource = _weigthList;
            recipeType.ItemsSource = typeList;
        }

        private void FillData()
        {
            _dataList.Add(new Ingredient { Name = "", Col = "", Ed = "" });
            //_dataList.Add(new Ingridient { Ingr = "", Col = "", Ed = "" });
            //_dataList.Add(new Ingridient { Ingr = "onion", Col = "1", Ed = "piece" });
            //_dataList.Add(new Ingridient { Ingr = "sugar", Col = "2", Ed = "teaspoon" });
            //_dataList.Add(new Ingridient { Ingr = "potatoes", Col = "0.5", Ed = "kilogram" });
        }

        private bool CheckInput()
        {
            bool isCorrect = true;
            if (recipeName.Text.Length == 0)
            {
                isCorrect = false;
                recipeName.Background = Brushes.OrangeRed;
            }
            if (recipeType.Text.Length == 0)
            {
                isCorrect = false;
                recipeType.Background = Brushes.OrangeRed;
            }
            return isCorrect;
        }

        private void AddRecipeClick(object sender, RoutedEventArgs e)
        {
            if (!CheckInput())
            {
                MessageBox.Show("Some fields have not been filled in", "Error adding a recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Directory.Exists("Recipes"))
                Directory.CreateDirectory("Recipes");
            int fileNum = 1;
            string fileName;
            while (File.Exists(fileName = "Recipes/recipe" + fileNum + ".xml"))
                fileNum++;

            var writer = new XmlTextWriter(fileName, null);

            writer.WriteStartDocument();
            writer.WriteStartElement("ListOfRecipes");

            writer.WriteStartElement("RecipeData");
            writer.WriteAttributeString("Name", recipeName.Text);
            writer.WriteAttributeString("Type", recipeType.Text);
            writer.WriteAttributeString("Text", recipeText.Text);

            if (_dataList != null)
            {
                writer.WriteStartElement("Ingredients");
                foreach (Ingredient ingridient in _dataList)
                {
                    if (ingridient.Name.Length != 0)
                    {
                        writer.WriteStartElement("Ingredient");
                        writer.WriteAttributeString("Name", ingridient.Name);
                        writer.WriteAttributeString("Col", ingridient.Col);
                        writer.WriteAttributeString("Ed", ingridient.Ed);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            Close();

            RecipeAddedSignal(fileName);
        }

        private void AddRowClick(object sender, RoutedEventArgs e)
        {
            _dataList.Add(new Ingredient());
        }

        private void RecipeNameTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            recipeName.Background = Brushes.White;
        }

        private void RecipeTypeGotFocus(object sender, RoutedEventArgs e)
        {
            recipeType.Background = Brushes.White;
        }
    }
}