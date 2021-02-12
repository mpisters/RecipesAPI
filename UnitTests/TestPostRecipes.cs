using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
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
      var mockRecipeSet = new Mock<DbSet<Recipe>>();
      // var mockIngredientSet = new Mock<DbSet<Ingredient>>();
      var mockContext = new Mock<RecipesContext>();

      mockContext.Setup(m => m.Recipes).Returns(mockRecipeSet.Object);
      // mockContext.Setup(m => m.Ingredients).Returns(mockIngredientSet.Object);
      var controller = new RecipesController(mockContext.Object);
      // var ingredient = new IngredientDto("aardbei", 123, "gr");
      var ingredientList = new IngredientDto[] { };

      var newRecipe = new RecipeDto("Test recipe", "description test", ingredientList);
      await controller.PostRecipe(newRecipe);
      mockRecipeSet.Verify(recipe => recipe.Add(It.IsAny<Recipe>()), Times.Once());
      mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }
  }
}