using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mecanillama.API.Reviews.Domain.Models;
using Mecanillama.API.Security.Domain.Services.Communication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Mecanillama.API.UnitTests

{
    [TestClass]
    public class UnitTest4
    {
        private HttpClient client;

        [TestInitialize]
        public void Setup()
        {
            // Configurar el cliente HTTP para realizar las solicitudes
            var webAppFactory = new WebApplicationFactory<Program>();
            client = webAppFactory.CreateClient();

            // Obtener el token de autorización
            string accessToken = ObtenerAccessToken().GetAwaiter().GetResult();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        [TestMethod]
        public async Task CreateReview_ReturnsCreated()
        {
            // Crear un objeto ReviewRequest con los datos de la reseña a crear
            var review = new Review
            {
                Comment = "muy bueno todo en verdad",
                Score = 5,
                MechanicId = 1
            };

            // Serializar el objeto ReviewRequest a formato JSON
            var jsonRequest = JsonConvert.SerializeObject(review);

            // Crear un objeto HttpContent con el contenido JSON
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Realizar una solicitud HTTP POST al endpoint de creación de reseñas
            HttpResponseMessage response = await client.PostAsync("/api/v1/reviews", content);

            // Verificar el código de estado de respuesta esperado
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Puedes realizar más aserciones según la respuesta recibida si es necesario
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
            var response = await client.PostAsync("/api/v1/users/sign-in", content);

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
}