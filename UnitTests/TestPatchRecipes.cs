using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        [Fact]
        public async Task TestReturnsNotFoundWhenRecipeIdIsNotFound()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);
            var ingredientList = Array.Empty<IngredientDto>();

            var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList);
            newRecipe.Id = 9999;
            ActionResult<Recipe> result = await controller.PatchRecipe(9999, newRecipe);
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task TestUpdatesRecipeWithoutIngredients()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);
            var existingIngredient = new Recipe {Description = "existing description", Name = "existing title"};
            await recipesContext.Recipes.AddAsync(existingIngredient);
            var controller = new RecipesController(recipesContext);
            var ingredientList = Array.Empty<IngredientDto>();

            var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList);
            newRecipe.Id = 1;
            await controller.PatchRecipe(1, newRecipe);
            var updatedRecipe = recipesContext.Recipes.Last();
            Assert.Equal("Updated recipe", updatedRecipe.Name);
            Assert.Equal("Updated description", updatedRecipe.Description);
            Assert.Empty(updatedRecipe.Ingredients);
        }
    }
}