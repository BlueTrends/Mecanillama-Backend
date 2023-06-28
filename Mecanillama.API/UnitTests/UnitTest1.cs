using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Mecanillama.API.Security.Domain.Services.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Mecanillama.API.UnitTests;

[TestClass]
public class ApiTests
{
    
    private HttpClient client;

    [TestInitialize]
    public void Setup()
    {
        // Configurar el cliente HTTP para realizar las solicitudes
        client = new HttpClient();
        var webAppFactory = new WebApplicationFactory<Program>();
        client = webAppFactory.CreateClient();
        
        // Obtener el token de autorización
        string accessToken = ObtenerAccessToken().GetAwaiter().GetResult();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
    
    
    [TestMethod]
    public async Task GetEndpoint_ReturnsOk()
    {
        
        // Hacer una solicitud GET al endpoint
        HttpResponseMessage response = await client.GetAsync("/api/v1/reviews");

        // Verificar el código de estado de respuesta esperado
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        // Puedes realizar más aserciones según el contenido de la respuesta si es necesario
        // Por ejemplo, verificar el contenido JSON devuelto por el endpoint
        //string responseBody = await response.Content.ReadAsStringAsync();
        //Assert.IsTrue(responseBody.Contains("expected_value"));
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        // Realizar limpieza después de cada prueba, si es necesario
        client.Dispose();
    }
    
    private async Task<string> ObtenerAccessToken()
    {
        

        // Crea una instancia de la clase AuthenticateRequest para enviar las credenciales de autenticación
        var authenticateRequest = new AuthenticateRequest
        {
            Email = "lucas@gmail.com",
            Password = "1234"
        };

        // Serializa el objeto AuthenticateRequest a formato JSON
        var jsonRequest = JsonConvert.SerializeObject(authenticateRequest);

        // Crea un objeto HttpContent con el contenido JSON
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Realiza una solicitud HTTP POST al endpoint de autenticación para obtener el token de acceso
        var response = await client.PostAsync("api/v1/users/sign-in", content);

        // Verificar el código de estado de respuesta esperado
        response.EnsureSuccessStatusCode();

        // Lee la respuesta JSON del servidor
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserializa la respuesta JSON en un objeto AuthenticateResponse
        var authenticateResponse = JsonConvert.DeserializeObject<AuthenticateResponse>(jsonResponse);

        // Retorna el token de acceso
        return authenticateResponse.Token;
    }
    
    
}