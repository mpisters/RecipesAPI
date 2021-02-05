using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;

namespace Recipes.Controllers
{
  [Route("api/Recipes")]
  [ApiController]
  public class RecipesController : ControllerBase
  {
    private readonly RecipesContext _context;

    public RecipesController(RecipesContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
    {
      return await _context.Recipes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetRecipeById(long id)
    {
      var recipe = await _context.Recipes.FindAsync(id);
      if (recipe == null)
      {
        return NotFound();
      }

      return recipe;
    }
    //
    // [HttpPut("{id}")]
    // public async Task<ActionResult<Recipe>> PutRecipe(long id, Recipe updatedRecipe)
    // {
    //   if (id != updatedRecipe.Id)
    //   {
    //     return BadRequest();
    //   }
    //
    //   var currentRecipe = await _context.Recipes.FindAsync(id);
    //   if (currentRecipe == null)
    //   {
    //     return NotFound();
    //   }
    //
    //   currentRecipe.Name = updatedRecipe.Name;
    //   currentRecipe.Description = updatedRecipe.Description;
    //   var ingredients = updatedRecipe.Ingredients;
    //   foreach (var ingredient in ingredients)
    //   {
    //     var newIngredient = new Ingredient
    //     {
    //         Name = ingredient.Name,
    //         Amount = ingredient.Amount,
    //         Unit = ingredient.Unit
    //     };
    //     _context.Ingredients.Add(newIngredient);
    //  
    //   }
    //   await _context.SaveChangesAsync();
    //   {
    //     
    //   }
    //
    //   _context.Entry(currentRecipe).State = EntityState.Modified;
    //
    //   try
    //   {
    //     await _context.SaveChangesAsync();
    //   }
    //   catch (DbUpdateConcurrencyException)
    //   {
    //     if (!RecipeExists(id))
    //     {
    //       return NotFound();
    //     }
    //     throw;
    //   }
    //   return currentRecipe;
    // }

    [HttpPost]
    public async Task<ActionResult<Recipe>> PostRecipe(RecipeDTO newRecipe)
    {
      var ingredients = newRecipe.Ingredients;
      var newIngredients = new List<Ingredient>();
      foreach (var ingredient in ingredients)
      {
        var newIngredient = new Ingredient
        {
            Name = ingredient.Name,
            Amount = ingredient.Amount,
            Unit = ingredient.Unit
        };
        await _context.Ingredients.AddAsync(newIngredient);
        await _context.SaveChangesAsync();
        newIngredients.Add(newIngredient);
      }

      var recipe = new Recipe
      {
          Name = newRecipe.Name,
          Description = newRecipe.Description,
          Ingredients = newIngredients
      };

      await _context.Recipes.AddAsync(recipe);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetRecipeById), new {id = recipe.Id}, recipe);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteRecipe(long id)
    {
      var recipe = await _context.Recipes.FindAsync(id);
      if (recipe == null)
      {
        return NotFound();
      }

      _context.Recipes.Remove(recipe);
      await _context.SaveChangesAsync();
      return NoContent();
    }

    private bool RecipeExists(long id)
    {
      return _context.Recipes.Any(item => item.Id == id);
    }
  }
}