using Newtonsoft.Json;
using NSubstitute;
using Pokeapi.Services;
using Pokeapi.Utils;
using System.Text;

namespace Pokeapi.Tests
{
    public class PokemonServiceTest
    {
        private IHttpClientWrapper client;
        private IPokemonService service;

        struct stat
        {
            public int base_stat { get; set; }
        }

        struct pokemon
        {
            public stat[] stats { get; set; }
        }

        public PokemonServiceTest()
        {
            client = Substitute.For<IHttpClientWrapper>();
            service = new PokemonService(client, string.Empty);
        }

        [Fact]
        public async Task Should_Call_API_With_CorrectUrl()
        {
            // Arrange
            const string pokeName1 = "pokemon 1";
            const string testurl = @"http://here.com/";
            service = new PokemonService(client, testurl);
            client.GetAsync(Arg.Any<string>()).Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK));

            // Act
            var result = await service.GetStrongerPokemonNameAsync(pokeName1, string.Empty);

            // Assert
            await client.Received().GetAsync(Arg.Is<string>(testurl));
        }


        [Fact]
        public async Task Should_Call_API_With_Pokemon_Names()
        {
            // Arrange
            const string pokeName1 = "pokemon 1";
            const string pokeName2 = "pokemon 2";
            client.GetAsync(Arg.Any<string>()).Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK));

            // Act
            var result = await service.GetStrongerPokemonNameAsync(pokeName1, pokeName2);

            // Assert
            await client.Received().GetAsync(Arg.Is<string>(pokeName1));
            await client.Received().GetAsync(Arg.Is<string>(pokeName2));
        }

        [Fact]
        public async Task Should_Return_Stonger_Pokemon_Name_If_Request_Is_Successful()
        {
            // Arrange
            var pokemon1 = GetPokemonJson(50);
            var pokemon2 = GetPokemonJson(49);
            const string pokeName1 = "pokemon 1";
            const string pokeName2 = "pokemon 2";

            client.GetAsync(pokeName1).Returns(GetHttpResponse(pokemon1));
            client.GetAsync(pokeName2).Returns(GetHttpResponse(pokemon2));

            // Act
            var result = await service.GetStrongerPokemonNameAsync(pokeName1, pokeName2);

            // Assert
            Assert.Contains(pokeName1, result);
        }

        [Fact]
        public async Task Should_Return_A_Tie_If_Both_Pokemons_Have_The_Same_HP()
        {
            // Arrange
            var pokemon1 = GetPokemonJson(50);
            var pokemon2 = GetPokemonJson(50);
            const string pokeName1 = "pokemon 1";
            const string pokeName2 = "pokemon 2";

            client.GetAsync(pokeName1).Returns(GetHttpResponse(pokemon1));
            client.GetAsync(pokeName2).Returns(GetHttpResponse(pokemon2));

            // Act
            var result = await service.GetStrongerPokemonNameAsync(pokeName1, pokeName2);

            // Assert
            Assert.Contains("tie", result);
        }


        [Fact]
        public async Task Should_Return_Message_Both_Pokemons_Are_Equal_If_The_Same_Name_Is_Informed()
        {
            // Arrange
            const string pokeName1 = "pokemon 1";

            // Act
            var result = await service.GetStrongerPokemonNameAsync(pokeName1, pokeName1);

            // Assert
            Assert.Contains("equal", result);
        }

        // Arrange
        const string WrongName1 = "pokewrong 1";
        const string WrongName2 = "pokewrong 2";
        const string RightName1 = "pokemon 1";
        const string RightName2 = "pokemon 2";

        [Theory]
        [InlineData(WrongName1, RightName2)]
        [InlineData(RightName1, WrongName2)]
        public async Task Should_Throw_KeyNotFoundException_If_Pokemons_Are_Not_Found(string pokeName1, string pokeName2)
        {
            // Arrange
            var pokemon1 = GetPokemonJson(20);
            var pokemon2 = pokemon1;

            client.GetAsync(RightName1).Returns(GetHttpResponse(pokemon1));
            client.GetAsync(RightName2).Returns(GetHttpResponse(pokemon2));
            client.GetAsync(WrongName1).Returns(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            client.GetAsync(WrongName2).Returns(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));

            // Act
            Task result () => service.GetStrongerPokemonNameAsync(pokeName1, pokeName2);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Should_Throw_InvalidOperationException_If_Fringe_Case_Occurs()
        {
            // Arrange
            const string pokeName1 = "pokemon 1";
            const string pokeName2 = "pokemon 2";
            client.GetAsync(Arg.Any<string>()).Returns(new HttpResponseMessage(System.Net.HttpStatusCode.GatewayTimeout));

            // Act
            Task result() => service.GetStrongerPokemonNameAsync(pokeName1, pokeName2);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(result);
        }

        private static string GetPokemonJson(int hp)
        {
            return JsonConvert.SerializeObject(new pokemon { stats = [new stat { base_stat = hp }] });
        }

        private static HttpResponseMessage GetHttpResponse(string pokemon1)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(pokemon1, Encoding.UTF8, "application/json") };
        }

    }
}