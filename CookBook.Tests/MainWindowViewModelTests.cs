using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CookBook.Helpers;
using CookBook.ViewModel;
using Moq;

namespace CookBook.Tests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IMessageHelper> _messageHelperMock = new();

        [Fact]
        public void AddIem_WhenCalled_AddsItemToList()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddRecipeType("test");
            Assert.Contains("test", viewModel.GetRecipeTypes());
        }

        [Fact]
        public void GetItemsTree_WhenCalled_ReturnsIEnumerableItems()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddRecipeType("test");
            viewModel.AddRecipeType("test2");

            var items = viewModel.GetRecipeTypes();
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public void RemoveItem_WhenCalled_RemovesItemFromList()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddRecipeType("test");
            viewModel.AddRecipeType("test2");
            viewModel.RemoveRecipeType("test");
            viewModel.RemoveRecipeType("test");
            Assert.DoesNotContain("test", viewModel.GetRecipeTypes());
        }

        [Fact]
        public void SortItems_WhenCalled_SortsItems()
        {
            var viewModel = new MainWindowViewModel();
            viewModel.AddRecipeType("test2");
            viewModel.AddRecipeType("test");
            viewModel.AddRecipeType("test3");
            viewModel.SortRecipeTypes();
            Assert.Equal("test", viewModel.GetRecipeTypes().First());
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
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId);

            var viewModel = new MainWindowViewModel();
            viewModel.ReadRecipeFromFile(filePath);
            var recipe = viewModel.GetRecipeByFileName(filePath);
            
            Assert.Single(viewModel.GetRecipes());
            Assert.Equal("Vegan", recipe.Type);
            Assert.Equal(15, recipe.Ingredients.Count);

            TestFaker.DeleteRecipeFile(filePath);
        }

        [Fact]
        public void RemoveRecipeByFileName_WhenCalled_RemovesRecipe()
        {
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId);

            var viewModel = new MainWindowViewModel();
            viewModel.ReadRecipeFromFile(filePath);
            viewModel.RemoveRecipeByFileName(filePath);
            
            Assert.Empty(viewModel.GetRecipes());

            TestFaker.DeleteRecipeFile(filePath);
        }

        [Theory]
        [InlineData(1, false, false)]
        [InlineData(1, true, true)]
        [InlineData(2, true, false)]
        public void RemoveRecipe_WhenCalled_RemovesRecipe(int index, bool userConfirm, bool isDeleted)
        {
            string testId = MethodBase.GetCurrentMethod()?.Name;
            _messageHelperMock.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>())).Returns(userConfirm);
            var viewModel = new MainWindowViewModel( messageHelper: _messageHelperMock.Object);

            string filePath = TestFaker.CreateRecipeFile(testId);
            string filePath2 = TestFaker.CreateRecipeFile(testId);
            
            viewModel.ReadRecipeFromFile(filePath);
            viewModel.ReadRecipeFromFile(filePath2);
            viewModel.RemoveRecipe(index);

            Assert.Equal(isDeleted, viewModel.GetRecipes().Count() == 1);
            TestFaker.DeleteRecipeFile(filePath);
            TestFaker.DeleteRecipeFile(filePath2);

        }


        [Fact]
        public void InitializeStorage_WhenCalled_ReadsRecipesFromStoragePath()
        {
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId);
            string filePath2 = TestFaker.CreateRecipeFile(testId);
            string storagePath = Path.GetDirectoryName(filePath);

            var viewModel = new MainWindowViewModel();
            viewModel.InitializeStorage(storagePath);

            Assert.Equal(2, viewModel.GetRecipes().Count());
            TestFaker.DeleteRecipeFile(filePath);
            TestFaker.DeleteRecipeFile(filePath2);
        }


    }
}
