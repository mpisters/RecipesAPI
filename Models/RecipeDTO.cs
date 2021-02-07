namespace Recipes.Models
{
  public class RecipeDto
  {
    public long? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IngredientDto[] Ingredients { get; set; }
  }
}