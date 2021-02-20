using System;
using System.Collections.Generic;
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
            var result = await controller.PatchRecipe(9999, newRecipe);
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
            var result = await controller.PatchRecipe(9999, newRecipe);
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task TestUpdatesRecipeWithoutIngredients()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);
            var emptyIngredientList = new List<Ingredient>();
            var existingIngredient = new Recipe {Description = "existing description", Name = "existing title", Ingredients = emptyIngredientList};
            await recipesContext.Recipes.AddAsync(existingIngredient);
            await recipesContext.SaveChangesAsync();
            var controller = new RecipesController(recipesContext);
            var ingredientList = Array.Empty<IngredientDto>();

            var newRecipe = new RecipeDto( "Updated recipe", "Updated description", ingredientList) {Id = 1};
            newRecipe.Id = 1;
            await controller.PatchRecipe(1, newRecipe);
            var updatedRecipe = recipesContext.Recipes.Last();
            Assert.Single(recipesContext.Recipes.ToList());
            Assert.Equal("Updated recipe", updatedRecipe.Name);
            Assert.Equal("Updated description", updatedRecipe.Description);
            Assert.Empty(updatedRecipe.Ingredients);
        }
        [Fact]
        public async Task TestUpdatesRecipeWithNewIngredients()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);
            var ingredientList = new IngredientDto[2];
            var ingredient1 = new IngredientDto("Ingredient1", 100, "gr");
            var ingredient2 = new IngredientDto("Ingredient2", 200, "kg");
            ingredientList[0] = ingredient1;
            ingredientList[1] = ingredient2;
            var emptyIngredientList = new List<Ingredient>();
            var existingIngredient = new Recipe {Description = "existing description", Name = "existing title", Ingredients = emptyIngredientList};
            await recipesContext.Recipes.AddAsync(existingIngredient);
            await recipesContext.SaveChangesAsync();
            var controller = new RecipesController(recipesContext);

            var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList) {Id = 1};
            await controller.PatchRecipe(1, newRecipe);
            var updatedRecipe = recipesContext.Recipes.Last();
            Assert.Single(recipesContext.Recipes.ToList());
            Assert.Equal("Updated recipe", updatedRecipe.Name);
            Assert.Equal("Updated description", updatedRecipe.Description);
            Assert.Equal(2,updatedRecipe.Ingredients.Count);
            Assert.Equal("Ingredient1", updatedRecipe.Ingredients[0].Name);
            Assert.Equal(100, updatedRecipe.Ingredients[0].Amount);
            Assert.Equal("gr", updatedRecipe.Ingredients[0].Unit);
            Assert.Equal("Ingredient2", updatedRecipe.Ingredients[1].Name);
            Assert.Equal(200, updatedRecipe.Ingredients[1].Amount);
            Assert.Equal("kg", updatedRecipe.Ingredients[1].Unit);
        }
    }
}