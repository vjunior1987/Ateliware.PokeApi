namespace Pokeapi.Services
{
    public interface IPokemonService
    {
        Task<string> GetStrongerPokemonNameAsync(string poke1, string poke2);
    }
}
