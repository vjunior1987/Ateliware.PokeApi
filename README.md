PokeAPI test for Ateliware
-

This project has been developed in .Net 8.0 with the purpose of demonstrating my technical skills. The stack of choice is .Net Core as chosen during the live coding. The test involves querying two pokémon for the purpose of comparing which of the two wins with the highest HP.

Requirements
-

To execute the project, you'll need the following:

- .Net 8+. You can obtain the latest version [https://dotnet.microsoft.com/en-us/download/dotnet](here).
- Alternatively, you can install Visual Studio 2022 with the necessary SDKs bundled in [https://visualstudio.microsoft.com/vs/](here).

Running the API project
-

To execute the VS Project that will query the pokeApi for the stronger pokémon, you can use command prompt by navigating to the api project folder and using the following:
### `dotnet run`
Then open [http://localhost:5238/swagger/](http://localhost:5238/swagger) to view it in your browser.

Or you can run the project using Visual Studio IDE. You can follow the steps to run webAPI projects as described [https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/run-program?view=vs-2022](here).

Running the Test project
-

Unit tests were implemented using XUnit and NSubstitute. To execute the tests, you can use command prompt by navigating to the test project folder and using the following:

### `dotnet test`

Or you can run the project using Visual Studio IDE. You can follow the steps to run test projects as described [https://learn.microsoft.com/en-us/visualstudio/test/getting-started-with-unit-testing?view=vs-2022&tabs=dotnet%2Cmstest#run-unit-tests](here).

Using Swagger
-

The API documentation was implemented with swagger as part of the Visual Studio webAPI project. 

Testing the endpoint can be done by expanding the tab of the endpoint documentation and clicking the `try it out` button. Swagger will ask for two pokémon names as parameters and handle the http request.

![image](https://github.com/vjunior1987/Ateliware.Pokeapi/assets/45671294/e826a541-a550-4ae0-b3ce-2efb96b68fff)

Input the two pokémon names and click execute

![image](https://github.com/vjunior1987/Ateliware.Pokeapi/assets/45671294/7edf8fa3-1105-4b8f-8e8d-6a894568fa43)
