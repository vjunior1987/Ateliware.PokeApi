using Pokeapi.Services;
using NSubstitute;
using Pokeapi.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;

namespace Pokeapi.Tests
{
    public class PokemonControllerTest
    {
        private IPokemonService pokemonService;
        private PokemonController pokemonController;

        public PokemonControllerTest()
        {
            pokemonService = Substitute.For<IPokemonService>();
            pokemonController = new PokemonController(pokemonService);
        }

        [Fact]
        public async Task Should_Call_PokemonService_With_Correct_Parameters_When_Requesting_Stronger_Pokemon()
        {
            // Arrange
            const string poke1 = "Pikachu";
            const string poke2 = "Charmander";
            pokemonService.GetStrongerPokemonNameAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);

            // Act
            var result = await pokemonController.GetStrongerPokemon(poke1, poke2);

            // Assert
            await pokemonService.Received().GetStrongerPokemonNameAsync(Arg.Is<string>(poke1), Arg.Is<string>(poke2));
        }

        [Fact]
        public async Task Should_Return_Code_200_Ok_When_Request_Is_Successful()
        {
            // Arrange
            const string poke1 = "Pikachu";
            const string poke2 = "Charmander";
            pokemonService.GetStrongerPokemonNameAsync(Arg.Any<string>(), Arg.Any<string>()).Returns($"O pokémon {poke2} é o mais forte com 39HP");

            // Act
            var result = await pokemonController.GetStrongerPokemon(poke1, poke2) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Should_Return_Pokemon_Name_When_Request_Is_Successful()
        {
            // Arrange
            const string poke1 = "Pikachu";
            const string poke2 = "Charmander";
            pokemonService.GetStrongerPokemonNameAsync(Arg.Any<string>(), Arg.Any<string>()).Returns($"O pokémon {poke2} é o mais forte com 39HP");

            // Act
            var result = await pokemonController.GetStrongerPokemon(poke1, poke2) as ObjectResult;

            // Assert
            Assert.Contains(poke2, result?.Value as string);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public async void Should_Return_Code_400_BadRequest_If_Parameters_Are_Missing(string poke1, string poke2)
        {
            // Arrange N/A
            // Act
            var result = await pokemonController.GetStrongerPokemon(poke1, poke2) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Should_Return_NotFound_If_One_Pokemon_Could_Not_Be_Found()
        {
            // Arrange
            const string poke1 = "Pikanchu";
            const string poke2 = "Charmanderino";
            pokemonService.GetStrongerPokemonNameAsync(Arg.Any<string>(), Arg.Any<string>()).Throws(new KeyNotFoundException("Pokemon not found"));

            // Act
            var result = await pokemonController.GetStrongerPokemon(poke1, poke2) as ObjectResult;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
