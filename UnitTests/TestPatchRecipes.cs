using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipes.Controllers;
using Recipes.Models;
using Xunit;

namespace Recipes.UnitTests
{
    [Collection("Database collection")]
    public class TestPatchRecipes
    {
        private readonly DatabaseFixture _fixture;

        public TestPatchRecipes(DatabaseFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async Task TestReturnsBadRequestWhenRecipeIdIsUnknown()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);
            var ingredientList = Array.Empty<IngredientDto>();

            var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList);
            ActionResult<Recipe> result = await controller.PatchRecipe(9999, newRecipe);
            Assert.IsType<BadRequestResult>(result.Result);
        }
    }
}