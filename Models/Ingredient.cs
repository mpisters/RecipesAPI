namespace Recipes.Models
{
  public class Ingredient
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    private string Unit { get; set; }
  }
}