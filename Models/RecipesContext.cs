using Microsoft.EntityFrameworkCore;

namespace Recipes.Models
{
  public class RecipesContext : DbContext
  {
    public RecipesContext(DbContextOptions<RecipesContext> options) : base(options)
    {
      
    }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
  }
}