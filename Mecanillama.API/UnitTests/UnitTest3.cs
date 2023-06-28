using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mecanillama.API.Security.Domain.Services.Communication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Mecanillama.API.UnitTests
{
    [TestClass]
    public class UnitTest3
    {
        private HttpClient client;

        [TestInitialize]
        public void Setup()
        {
            // Configurar el cliente HTTP para realizar las solicitudes
            var webAppFactory = new WebApplicationFactory<Program>();
            client = webAppFactory.CreateClient();
        }

        [TestMethod]
        public async Task Register_ReturnsCreated()
        {
            // Crea una instancia de la clase RegisterRequest con los datos de registro
            var registerRequest = new RegisterRequest
            {
                Email = "carmen1@gmal.com",
                Password = "1234",
                Role = "customer",
                Name = "Carmen Gonzales"
            };

            // Serializa el objeto RegisterRequest a formato JSON
            var jsonRequest = JsonConvert.SerializeObject(registerRequest);

            // Crea un objeto HttpContent con el contenido JSON
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Realiza una solicitud HTTP POST al endpoint de registro
            HttpResponseMessage response = await client.PostAsync("api/v1/users/sign-up", content);

            // Verificar el código de estado de respuesta esperado
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Realizar limpieza después de cada prueba, si es necesario
            client.Dispose();
        }
    }
}


