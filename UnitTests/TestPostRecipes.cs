using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recipes.Controllers;
using Recipes.Models;
using Xunit;

namespace Recipes.UnitTests
{
    public class TestPostRecipes
    {
        [Fact]
        public async Task PostRecipeWithoutIngredients()
        {
            // Todo place this in a setup class/fixture
            var options = new DbContextOptionsBuilder<RecipesContext>()
                .UseInMemoryDatabase(databaseName: "TestRecipeDatabase")
                .Options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);
            var ingredientList = Array.Empty<IngredientDto>();

            var newRecipe = new RecipeDto("Test recipe", "Test description", ingredientList);
            await controller.PostRecipe(newRecipe);
            Assert.Single(recipesContext.Recipes.ToListAsync().Result);
            var addedRecipe = recipesContext.Recipes.FirstOrDefault();
            Assert.Equal("Test recipe", addedRecipe.Name);
            Assert.Equal("Test description", addedRecipe.Description);
            Assert.Empty(addedRecipe.Ingredients);
        }
    }
}