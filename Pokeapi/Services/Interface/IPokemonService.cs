namespace Pokeapi.Services
{
    /// <summary>
    /// Using service architecture to implement the API call in a separate class, to comply with SOLID principles for better software design
    /// </summary>
    public interface IPokemonService
    {
        Task<string> GetStrongerPokemonNameAsync(string poke1, string poke2);
    }
}
