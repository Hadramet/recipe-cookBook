using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBook.Model;
using CookBook.Services;
using Moq;

namespace CookBook.Tests
{
    public class RecipeServiceTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock = new();

        [Fact]
        public void GetRecipes_WhenCalled_ReturnsRecipes()
        {
            var recipes = new List<Recipe>
            {
                new() {Id = Guid.NewGuid(), Name = "Recipe 1"},
                new() {Id = Guid.NewGuid(), Name = "Recipe 2"}
            };

            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes);
            
            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            var result = recipeService.GetRecipeList();

            _recipeRepositoryMock.Verify(x => x.GetRecipes(), Times.Once);
            Assert.Equal(recipes, result);
        }

        [Fact]
        public void GetRecipes_WhenCalled_ReturnsEmptyList()
        {
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(Enumerable.Empty<Recipe>);

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            var result = recipeService.GetRecipeList();

            _recipeRepositoryMock.Verify(x => x.GetRecipes(), Times.Once);
            Assert.Empty(result);
        }

        [Fact]
        public void GetRecipes_WhenCalled_ReturnsNull()
        {
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(() => null);

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            var result = recipeService.GetRecipeList();

            _recipeRepositoryMock.Verify(x => x.GetRecipes(), Times.Once);
            Assert.Null(result);
        }

        [Fact]
        public void GetRecipeById_WhenCalled_ReturnsRecipe()
        {
            var recipe = new Recipe { Id = Guid.NewGuid(), Name = "Recipe 1"};

            _recipeRepositoryMock.Setup(x => x.GetRecipeById(recipe.Id)).ReturnsAsync(recipe);

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            var result = recipeService.GetRecipeById(recipe.Id);

            _recipeRepositoryMock.Verify(x => x.GetRecipeById(recipe.Id), Times.Once);
            Assert.Equal(recipe, result);
        }

        [Fact]
        public void GetRecipeById_WhenCalled_ReturnsNull()
        {
            _recipeRepositoryMock.Setup(x => x.GetRecipeById(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            var result = recipeService.GetRecipeById(Guid.NewGuid());

            _recipeRepositoryMock.Verify(x => x.GetRecipeById(It.IsAny<Guid>()), Times.Once);
            Assert.Null(result);
        }

        [Fact]
        public void SaveRecipe_WhenCalled_SavesRecipe()
        {
            var recipe = new Recipe { Id = Guid.NewGuid(), Name = "Recipe 1" };

            _recipeRepositoryMock.Setup(x => x.AddRecipe(recipe)).Returns(Task.CompletedTask);

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            recipeService.SaveRecipe(recipe);

            _recipeRepositoryMock.Verify(x => x.AddRecipe(recipe), Times.Once);
        }

        [Fact]
        public void SaveRecipe_WhenCalled_ThrowsException()
        {
            var recipe = new Recipe { Id = Guid.NewGuid(), Name = "Recipe 1" };

            _recipeRepositoryMock.Setup(x => x.AddRecipe(recipe)).Returns(Task.FromException(new Exception()));

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            Assert.Throws<Exception>(() => recipeService.SaveRecipe(recipe));

            _recipeRepositoryMock.Verify(x => x.AddRecipe(recipe), Times.Once);
        }

        [Fact]
        public void RemoveRecipe_WhenCalled_RemovesRecipe()
        {
            var recipe = new Recipe { Id = Guid.NewGuid(), Name = "Recipe 1" };

            _recipeRepositoryMock.Setup(x => x.DeleteRecipe(recipe.Id)).Returns(Task.CompletedTask);

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            recipeService.RemoveRecipe(recipe.Id);

            _recipeRepositoryMock.Verify(x => x.DeleteRecipe(recipe.Id), Times.Once);
        }

        [Fact]
        public void RemoveRecipe_WhenCalled_ThrowsException()
        {
            var recipe = new Recipe { Id = Guid.NewGuid(), Name = "Recipe 1" };

            _recipeRepositoryMock.Setup(x => x.DeleteRecipe(recipe.Id)).Returns(Task.FromException(new Exception()));

            var recipeService = new RecipeService(_recipeRepositoryMock.Object);

            Assert.Throws<Exception>(() => recipeService.RemoveRecipe(recipe.Id));

            _recipeRepositoryMock.Verify(x => x.DeleteRecipe(recipe.Id), Times.Once);
        }

        
    }
}
