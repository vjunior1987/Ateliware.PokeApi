namespace Pokeapi.Utils
{
    /// <summary>
    /// This wrapper was created in order to mock the http requests when unit testing. This decision was made in order to have the tests cover managed code, as well as removing the need for the 
    /// testing environment to be online to work.
    /// </summary>
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
