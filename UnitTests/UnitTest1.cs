using System.Threading.Tasks;
using Recipes.Controllers;
using Xunit;

namespace UnitTests
{
    public class POSTRecipeRequest
    {
        [Fact]
        public async Task PostRecipeWithoutIngredients ()
        {
            var controller = new RecipesController();
        }
    }
}