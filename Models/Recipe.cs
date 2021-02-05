using System;

namespace Recipes.Models
{
  public class Recipe
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Ingredient[] Ingredients { get; set; }
  }
}