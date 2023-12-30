using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml;
using CookBook.Extensions;
using CookBook.ViewModel;

namespace CookBook
{
    public partial class MainWindow
    {
        private readonly TreeViewItem _root = new TreeViewItem { Header = "All dishes", IsExpanded = true };
        private int _curShowIndex;

        private MainWindowViewModel _viewModel = new();
        public MainWindow()
        {
            InitializeComponent();
            CreateTree();
            AddRoot();
            GetFileNames();
        }

        public void AddRecipeClick(object sender, RoutedEventArgs e)
        {
            _viewModel.SortItems();
            var addWindow = new AddWindow(_viewModel.GetItemsList());
            addWindow.Show();
            addWindow.RecipeAddedSignal += ReadRecipeFromXml;
        }



        private void ReadRecipeFromXml(string fileName)
        {
            _viewModel.ReadRecipeFromFile(fileName);
            AddRecipeToTree(_viewModel.GetRecipeByFileName(fileName));
        }

        private void AddRecipeToTree(Recipe recipe)
        {
            bool isExist = false;
            TreeViewItem typeItem = null;
            foreach (TreeViewItem ti in _root.Items)
            {
                if (Equals(ti.Header, recipe.Type))
                {
                    isExist = true;
                    typeItem = ti;
                    break;
                }
            }

            if (!isExist)
            {
                typeItem = new TreeViewItem { Header = recipe.Type, IsExpanded = true };
                _root.Items.Add(typeItem);
                _viewModel.AddItem(recipe.Type);
            }

            var rec = new TreeViewItem { Header = recipe.Name };
            rec.MouseDoubleClick += RecipeDoubleClicked;
            typeItem.Items.Add(rec);
            SortElementsInNode(_root);
            SortElementsInNode(typeItem);
        }

        private static void SortElementsInNode(TreeViewItem item)
        {
            item.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
        }

        private void RecipeDoubleClicked(object sender, EventArgs e)
        {
            foreach (Recipe recipe in _viewModel.GetRecipes().ToList())
            {
                if (((TreeViewItem)sender).Header.Equals(recipe.Name))
                {
                    ShowInStack(recipe);
                    break;
                }
            }
        }

        private void ShowInStack(Recipe recipe)
        {
            stack.Children.Clear();
            stack.Children.Add(new Label
            {
                FontWeight = FontWeights.Bold,
                FontSize = 20,
                Foreground = Brushes.Green,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = recipe.Name
            });
            stack.Children.Add(new Label
            {
                Foreground = Brushes.White,
                Content = recipe.Type
            });
            foreach (Ingridient ingridient in recipe.Ingridients)
            {
                stack.Children.Add(new Label
                {
                    FontWeight = FontWeights.Thin,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Content = ingridient.Ingr + " " + ingridient.Col + " " + ingridient.Ed
                });
            }
            stack.Children.Add(new TextBlock
            {
                Foreground = Brushes.White,
                Text = recipe.Text,
                TextWrapping = TextWrapping.Wrap,
            });
            stack.UpdateLayout();

            _curShowIndex = _viewModel.GetRecipes().ToList().IndexOf(recipe);
        }

        private void GetFileNames()
        {
            if (!Directory.Exists("Recipes"))
                return;
            string[] files = Directory.GetFiles("Recipes/", "*.xml");
            foreach (string file in files)
                ReadRecipeFromXml(file);
        }

        private void AddRoot()
        {
            treeView1.Items.Add(_root);
        }

        private void CreateTree()
        {
            _root.AddItemsToTreeView(_viewModel.GetItemsTree().ToTreeViewItems());
        }

        private void RefreshTreee()
        {
            _root.Items.Clear();
            CreateTree();
            foreach (var recipe in _viewModel.GetRecipes())
                AddRecipeToTree(recipe);
        }

        private void CheckName()
        {
            if (recipeNameSearch == null) return;
            if (recipeNameSearch.Text.Length == 0)
                return;

            for (int index = 0; index < _root.Items.Count; index++)
            {
                var ti = (TreeViewItem)_root.Items[index];
                for (int i = 0; i < ti.Items.Count; i++)
                {
                    var ri = (TreeViewItem)ti.Items[i];
                    if (!ri.Header.ToString().ToLower().Contains(recipeNameSearch.Text.ToLower()))
                    {
                        ti.Items.Remove(ri);
                        i--;
                    }
                }
                if (ti.Items.Count == 0)
                {
                    _root.Items.Remove(ti);
                    index--;
                }
            }
        }

        private void CheckType()
        {
            if (recipeTypeSearch == null) return;
            if (recipeTypeSearch.Text.Length == 0)
                return;

            for (int index = 0; index < _root.Items.Count; index++)
            {
                var ti = (TreeViewItem)_root.Items[index];
                if (!ti.Header.ToString().ToLower().Contains(recipeTypeSearch.Text.ToLower()))
                {
                    _root.Items.Remove(ti);
                    index--;
                }
            }
        }

        private void Expander1Collapsed(object sender, RoutedEventArgs e)
        {
            RefreshTreee();

            var a = new DoubleAnimation { From = Height, To = Height - 30 };
            BeginAnimation(HeightProperty, a);
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            RefreshTreee();
            CheckType();
            CheckName();
            CheckIngridient();
        }

        private void CheckIngridient()
        {
            if (recipeIngrSearch == null) return;
            if (recipeIngrSearch.Text.Length == 0)
                return;

            for (int index = 0; index < _root.Items.Count; index++)
            {
                var ti = (TreeViewItem)_root.Items[index];
                for (int i = 0; i < ti.Items.Count; i++)
                {
                    var ri = (TreeViewItem)ti.Items[i];
                    bool isIngrCont = false;
                    foreach (Recipe recipe in _viewModel.GetRecipes().ToList())
                    {
                        if (Equals(recipe.Name, ri.Header))
                        {
                            foreach (Ingridient ingridient in recipe.Ingridients)
                            {
                                if (ingridient.Ingr.ToLower().Contains(recipeIngrSearch.Text.ToLower()))
                                    isIngrCont = true;
                            }
                            break;
                        }
                    }
                    if (!isIngrCont)
                    {
                        ti.Items.Remove(ri);
                        i--;
                    }
                }
                if (ti.Items.Count == 0)
                {
                    _root.Items.Remove(ti);
                    index--;
                }
            }
        }

        private void Expander1Expanded(object sender, RoutedEventArgs e)
        {
            RefreshTreee();
            CheckType();
            CheckName();
            CheckIngridient();

            var a = new DoubleAnimation { From = Height, To = Height + 30 };
            BeginAnimation(HeightProperty, a);
        }

        private void DelRecipeButtonClick(object sender, RoutedEventArgs e)
        {
            if (
                MessageBox.Show("The recipe will be permanently deleted. Do you want me to continue?", "Delete a recipe", MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.No)
                return;
            Recipe rec = _viewModel.GetRecipes().ToList()[_curShowIndex];
            File.Delete(rec.FileName);
            _viewModel.RemoveItem(rec.Type);
            _viewModel.RemoveRecipeByFileName(rec.FileName);
            RefreshTreee();
        }
    }

    public class Ingridient
    {
        public string Ingr { get; set; }
        public string Col { get; set; }
        public string Ed { get; set; }
    }

    public class Recipe
    {
        public List<Ingridient> Ingridients = new List<Ingridient>();
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}