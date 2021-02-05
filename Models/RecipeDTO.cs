namespace Recipes.Models
{
  public class RecipeDTO
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IngredientDTO[] Ingredients { get; set; }
  }
}