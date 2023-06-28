using System.Net;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;

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
        string accessToken = ObtenerAccessToken();
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
    
    private string ObtenerAccessToken()
    {
        // Aquí debes implementar la lógica para obtener el token de autorización
        // Esto puede implicar hacer una solicitud de autenticación a tu API
        // o interactuar con algún servicio de autenticación (como Identity Server, Azure AD, etc.)
        // y extraer el token de autorización necesario para las pruebas

        // Ejemplo básico: retornar un token fijo para propósitos de prueba
        return "tu_token_de_autorizacion";
    }
}