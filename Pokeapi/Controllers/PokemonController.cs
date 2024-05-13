using Microsoft.AspNetCore.Mvc;
using Pokeapi.Services;

namespace Pokeapi.Controllers
{

    [Route("api/[controller]")]
    public class PokemonController : Controller
    {
        private readonly IPokemonService _service;

        public PokemonController(IPokemonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetStrongerPokemon(string poke1, string poke2)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(poke1) || string.IsNullOrWhiteSpace(poke2))
                {
                    return BadRequest("Both pokemón names are requirerd"); // Returning 400 bad request when the required parameters are not informed
                }

                return Ok(await _service.GetStrongerPokemonNameAsync(poke1, poke2));
            }
            catch (KeyNotFoundException ex) // Exception treatment for 404 Not Found exceptions, for when one of the pokémon names return no results
            {
                return NotFound(ex.Message);
            }
        }
    }
}
