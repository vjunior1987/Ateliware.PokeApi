using Newtonsoft.Json;
using Pokeapi.Utils;

namespace Pokeapi.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly string _url;
        private readonly IHttpClientWrapper _client;
        public PokemonService(IHttpClientWrapper client)
        {
            _url = "https://pokeapi.co/api/v2/pokemon/";
            _client = client;
        }

        public PokemonService(IHttpClientWrapper client, string url)
        {
            _url = url;
            _client = client;
        }

        public async Task<string> GetStrongerPokemonNameAsync(string poke1, string poke2)
        {
            if (poke1 == poke2)
            {
                return $"Both pokémons are equal";
            }

            var poke1Hp = 0;
            var poke2Hp = 0;

            var poke1Result = await _client.GetAsync($"{_url}{poke1}");
            var poke2Result = await _client.GetAsync($"{_url}{poke2}");

            if (poke1Result.IsSuccessStatusCode && poke2Result.IsSuccessStatusCode)
            {
                var poke1Str = await poke1Result.Content.ReadAsStringAsync();
                var poke2Str = await poke2Result.Content.ReadAsStringAsync();

                var pokemon1 = JsonConvert.DeserializeObject<dynamic>(poke1Str);
                var pokemon2 = JsonConvert.DeserializeObject<dynamic>(poke2Str);

                if (pokemon1?.stats != null && pokemon1.stats[0].base_stat != null)
                {
                    poke1Hp = Convert.ToInt32(pokemon1.stats[0].base_stat);
                }
                if (pokemon2?.stats != null && pokemon2.stats[0].base_stat != null)
                {
                    poke2Hp = Convert.ToInt32(pokemon2.stats[0].base_stat);
                }
                if (poke1Hp == poke2Hp)
                {
                    return $"It is a tie between {poke1} and {poke2}!";
                }
                return poke1Hp > poke2Hp ? $"The stronger pokémon is {poke1} with {poke1Hp}HP" : $"The stronger pokémon is {poke2} with {poke2Hp}HP";
            }

            if (poke1Result.StatusCode == System.Net.HttpStatusCode.NotFound) throw new KeyNotFoundException($"Could not find pokémon {poke1}");
            if (poke2Result.StatusCode == System.Net.HttpStatusCode.NotFound) throw new KeyNotFoundException($"Could not find pokémon {poke2}");

            throw new InvalidOperationException("Unknown error when retrieving Pokémon data");
        }
    }
}
