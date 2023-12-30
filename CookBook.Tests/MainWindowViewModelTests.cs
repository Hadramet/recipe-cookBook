using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CookBook.ViewModel;

namespace CookBook.Tests
{
    public class MainWindowViewModelTests
    {
        
        [Fact]
        public void AddIem_WhenCalled_AddsItemToList()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddItem("test");
            Assert.Contains("test", viewModel.GetItemsList());
        }

        [Fact]
        public void GetItemsTree_WhenCalled_ReturnsIEnumerableItems()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddItem("test");
            viewModel.AddItem("test2");

            var items = viewModel.GetItemsTree();
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public void RemoveItem_WhenCalled_RemovesItemFromList()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddItem("test");
            viewModel.AddItem("test2");
            viewModel.RemoveItem("test");
            viewModel.RemoveItem("test");
            Assert.DoesNotContain("test", viewModel.GetItemsList());
        }

        [Fact]
        public void SortItems_WhenCalled_SortsItems()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddItem("test2");
            viewModel.AddItem("test");
            viewModel.AddItem("test3");
            viewModel.SortItems();
            Assert.Equal("test", viewModel.GetItemsList().First());
        }

        [Fact]
        public void ReadRecipeFromFile_ThrowsException_WhenFileDoesNotExist()
        {
            var viewModel = new MainWindowViewModel();
            Assert.Throws<FileNotFoundException>(() => viewModel.ReadRecipeFromFile("test"));
        }

        [Fact]
        public void ReadRecipeFromFile_WhenCalled_ReadsRecipeFromFile()
        {
            string filePath = TestFaker.CreateRecipeFile();

            var viewModel = new MainWindowViewModel();
            viewModel.ReadRecipeFromFile(filePath);
            var recipe = viewModel.GetRecipeByFileName(filePath);
            
            Assert.Single(viewModel.GetRecipes());
            Assert.Equal("TestRecipe", recipe.Name);
            Assert.Equal("Vegan", recipe.Type);
            Assert.Equal(15, recipe.Ingridients.Count);

            TestFaker.DeleteRecipeFile(filePath);
        }

        [Fact]
        public void RemoveRecipeByFileName_WhenCalled_RemovesRecipe()
        {
             
            string filePath = TestFaker.CreateRecipeFile();

            var viewModel = new MainWindowViewModel();
            viewModel.ReadRecipeFromFile(filePath);
            viewModel.RemoveRecipeByFileName(filePath);
            
            Assert.Empty(viewModel.GetRecipes());

            TestFaker.DeleteRecipeFile(filePath);
        }

    }
}
