using AgrooAnnauireModel.Dto;
using AgrooAnnauireModel.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AgrooAnnuaireWPF.Services
{
    internal class HttpAgrooAnnuaireServiceSite
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

        // Méthode GET pour récupérer les sites
        public static async Task<List<SitesDto>> GetSites()
        {
            string route = "api/Sites";
            var response = await Client.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                string resultat = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SitesDto>>(resultat)
                    ?? throw new FormatException($"Erreur Http : {route}");
            }
            throw new Exception(response.ReasonPhrase);
        }

        // Méthode GET pour récupérer le nombre de salarié travaillant sur le site selon le siteId
        public static async Task<int> GetNombreUtilisateursBySiteId(int siteId)
        {
            string route = $"api/Sites/GetNombreUtilisateursBySiteId/{siteId}";

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

                throw new FormatException($"Réponse inattendue pour {siteId}: {resultat}");
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


        // Méthode POST pour créer un site
        public static async Task<bool> CreateSite(SitesDto site)
        {
            string route = "api/Sites";
            var jsonContent = JsonConvert.SerializeObject(site);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(route, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la création du site : {response.ReasonPhrase}");
            }
        }

        //// Méthode PUT pour mettre à jour un site
        public static async Task<bool> UpdateSite(int id, SitesDto site)
        {
            string route = $"api/Sites/{id}";
            var jsonContent = JsonConvert.SerializeObject(site);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await Client.PutAsync(route, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la mise à jour  du site : {response.ReasonPhrase}");
            }
        }

        // Méthode DELETE pour supprimer un site
        public static async Task<bool> DeleteSite(int id)
        {
            string route = $"api/Sites/{id}";
            var response = await Client.DeleteAsync(route);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la suppression du site  : {response.ReasonPhrase}");
            }
        }
    }
}
