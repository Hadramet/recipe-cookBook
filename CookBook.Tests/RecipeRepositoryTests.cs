using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CookBook.Model;
using CookBook.Services;

namespace CookBook.Tests
{
    public class RecipeRepositoryTests
    {
        [Fact]
        public void RecipeRepository_WhenInitialized_ThrowsException_WhenRepositoryPathIsNotSet()
        {
            Assert.Throws<ArgumentNullException>(() => new RecipeRepository(null));
        }

        [Fact]
        public void RecipeRepository_WhenInitialized_ThrowsException_WhenRepositoryPathIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new RecipeRepository(string.Empty));
        }

        [Fact]
        public void RecipeRepository_WhenInitialized_CreatesRepositoryIfItDoesNotExist()
        {
            string repositoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _ = new RecipeRepository(repositoryPath);
            Assert.True(Directory.Exists(repositoryPath));
            TestFaker.DeleteTestDirectory(repositoryPath);
        }

        [Fact]
        public void RecipeRepository_WhenInitialized_LoadsRecipesFromRepository()
        {
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId);
            string filePath2 = TestFaker.CreateRecipeFile(testId);
            string storagePath = Path.GetDirectoryName(filePath);

            var repository = new RecipeRepository(storagePath);
            var recipes = repository.GetRecipes();

            Assert.Equal(2, recipes.Count());

            TestFaker.DeleteTestDirectory(storagePath);
        }

        [Fact]
        public void GetRecipeById_WhenCalled_ReturnsRecipe()
        {
            var testGuid = Guid.NewGuid();
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId, id:testGuid);
            string storagePath = Path.GetDirectoryName(filePath);

            var repository = new RecipeRepository(storagePath);
            var recipe = repository.GetRecipeById(testGuid);

            Assert.Equal(testGuid.ToString(), recipe.Name);

            TestFaker.DeleteTestDirectory(storagePath);
        }

        [Fact]
        public void GetRecipeById_WhenCalled_ThrowsException_WhenIdIsEmpty()
        {
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath = TestFaker.CreateRecipeFile(testId);
            string storagePath = Path.GetDirectoryName(filePath);

            var repository = new RecipeRepository(storagePath);
            Assert.Throws<ArgumentNullException>(() => repository.GetRecipeById(Guid.Empty));

            TestFaker.DeleteTestDirectory(storagePath);
        }

        [Fact]
        public void RemoveRecipe_WhenCalled_RemovesRecipe()
        {
            var testGuid = Guid.NewGuid();
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath2 = TestFaker.CreateRecipeFile(testId, id:testGuid);
            string storagePath = Path.GetDirectoryName(filePath2);

            var repository = new RecipeRepository(storagePath);
            repository.DeleteRecipe(testGuid);

            Assert.Empty(repository.GetRecipes());
            Assert.False(File.Exists(filePath2));

            TestFaker.DeleteTestDirectory(storagePath);
        }
        // TODO: AddRecipe
        [Fact]
        public void AddRecipe_WhenCalled_AddsRecipe()
        {
            var testGuid = Guid.NewGuid();
            string testId = MethodBase.GetCurrentMethod()?.Name;
            string filePath2 = TestFaker.CreateRecipeFile(testId, id:testGuid);
            string storagePath = Path.GetDirectoryName(filePath2);

            var repository = new RecipeRepository(storagePath);
            repository.AddRecipe(new Recipe()
            {
                FileName = string.Empty,
                Name = "Test",
                Text = "Test",
                Type = "Test",
                Ingredients =
                {
                    new Ingredient()
                    {
                        Name = "Test",
                        Col = "1",
                        Ed = "cup"
                    }
                }
            });
            
            Assert.Equal(2,repository.GetRecipes().Count());
            Assert.Equal(2, Directory.GetFiles(storagePath).Length);

            TestFaker.DeleteTestDirectory(storagePath);

        }
    }
}
