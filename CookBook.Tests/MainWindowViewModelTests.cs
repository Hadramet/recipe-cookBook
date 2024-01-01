using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CookBook.Helpers;
using CookBook.Services;
using CookBook.ViewModel;
using Moq;

namespace CookBook.Tests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IMessageHelper> _messageHelperMock = new();
        


        [Fact]
        public void RemoveRecipeByFileName_WhenCalled_RemovesRecipe()
        {
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId, id: Guid.NewGuid());
            string storagePath = Path.GetDirectoryName(filePath);

            var viewModel = new MainWindowViewModel(
                _messageHelperMock.Object, 
                new RecipeService(new RecipeRepository(storagePath)));

            viewModel.RemoveRecipeByFileName(filePath);
            Assert.Empty(viewModel.GetRecipes());
            TestFaker.DeleteTestDirectory(storagePath);
        }

        [Theory]
        [InlineData(1, false, false)]
        [InlineData(1, true, true)]
        [InlineData(2, true, false)]
        public void RemoveRecipe_WhenCalled_RemovesRecipe(int index, bool userConfirm, bool isDeleted)
        {
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId, id: Guid.NewGuid());
            string filePath2 = TestFaker.CreateRecipeFile(testId, id: Guid.NewGuid());
            string storagePath = Path.GetDirectoryName(filePath);

            _messageHelperMock.Setup(x => 
                x.ShowMessage(It.IsAny<string>(), It.IsAny<string>())).Returns(userConfirm);

            var viewModel = new MainWindowViewModel( 
                messageHelper: _messageHelperMock.Object, 
                recipeService: new RecipeService(new RecipeRepository(storagePath)));

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

            var viewModel = new MainWindowViewModel( 
                messageHelper: _messageHelperMock.Object, 
                recipeService: new RecipeService(new RecipeRepository(storagePath)));


            Assert.Equal(2, viewModel.GetRecipes().Count());
            TestFaker.DeleteRecipeFile(filePath);
            TestFaker.DeleteRecipeFile(filePath2);
        }


    }
}
