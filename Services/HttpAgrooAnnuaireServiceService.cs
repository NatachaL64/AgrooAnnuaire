using AgrooAnnauireModel.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgrooAnnuaireWPF.Services
{
    internal class HttpAgrooAnnuaireServiceService
    {
        private const string baseAddress = "https://localhost:7042/";
        private static HttpClient? client;
        private static CookieContainer cookieContainer = new();


        private static HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
                    client = new(handler) { BaseAddress = new Uri(baseAddress) };
                }
                return client;
            }
        }

        public static async Task<bool> Login(string username, string password)
        {
            string route = "login?useCookies=true&useSessionCookies=true";
            var jsonString = "{ \"email\": \"" + username + "\", \"password\": \"" + password + "\" }";

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(route, httpContent);

            var cookies = cookieContainer.GetCookies(new Uri(baseAddress));
            Debug.WriteLine(cookies);

            return response.IsSuccessStatusCode ? true :
                throw new Exception(response.ReasonPhrase);
        }

        // Méthode GET pour récupérer les service
        public static async Task<List<ServicesDto>> GetServices()
        {
            string route = "api/Services";
            var response = await Client.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                string resultat = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ServicesDto>>(resultat)
                    ?? throw new FormatException($"Erreur Http : {route}");
            }
            throw new Exception(response.ReasonPhrase);
        }



        // Méthode GET pour récupérer le nombre de salarié travaillant dans le service selon le serviceId
        public static async Task<int> GetNombreUtilisateursByServiceId(int serviceId)
        {
            string route = $"api/Services/GetNombreUtilisateursByServiceId/{serviceId}";

            try
            {
                var response = await Client.GetAsync(route);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erreur API: {response.StatusCode} - {response.ReasonPhrase}");
                }

                string resultat = await response.Content.ReadAsStringAsync();

                if (int.TryParse(resultat, out var nbUtilisateur))
                {
                    return nbUtilisateur;
                }

                throw new FormatException($"Réponse inattendue pour {serviceId}: {resultat}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erreur réseau lors de la récupération du nombre d'utilisateurs", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du traitement de la requête: {ex.Message}", ex);
            }
        }


        // Méthode POST pour créer un service
        public static async Task<bool> CreateService(ServicesDto service)
        {
            string route = "api/Services";
            var jsonContent = JsonConvert.SerializeObject(service);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(route, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la création du service : {response.ReasonPhrase}");
            }
        }

        //// Méthode PUT pour mettre à jour un service
        public static async Task<bool> UpdateService(int id, ServicesDto service)
        {
            string route = $"api/Services/{id}";
            var jsonContent = JsonConvert.SerializeObject(service);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await Client.PutAsync(route, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la mise à jour  du service : {response.ReasonPhrase}");
            }
        }

        // Méthode DELETE pour supprimer un service
        public static async Task<bool> DeleteService(int id)
        {
            string route = $"api/Services/{id}";
            var response = await Client.DeleteAsync(route);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la suppression du service  : {response.ReasonPhrase}");
            }
        }
    }
}
