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
    class HtppAgrooAnnuaireServiceSalarie
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

        // Méthode GET pour récupérer les salariés
        public static async Task<List<UtilisateursDto>> GetSalaries()
        {
            string route = "api/Utilisateurs";
            var response = await Client.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                string resultat = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UtilisateursDto>>(resultat)
                    ?? throw new FormatException($"Erreur Http : {route}");
            }
            throw new Exception(response.ReasonPhrase);
        }

        // Méthode POST pour créer un salarie
        public static async Task<bool> CreateSalarie(UtilisateursDto salarie)
        {
            string route = "api/Utilisteurs";
            var jsonContent = JsonConvert.SerializeObject(salarie);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(route, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la création du salarié : {response.ReasonPhrase}");
            }
        }

        // Méthode GET pour récupérer l'id du site ou le salarie travaille avec l'id du salarie
        public static async Task<int> GetSiteIdbyUtilisateurId(int id)
        {
            string route = $"api/Utilisateurs/search/{id}/siteId"; 

            var response = await Client.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                
                string resultat = await response.Content.ReadAsStringAsync();
 
                if (int.TryParse(resultat, out var siteId))
                {
                    return siteId;
                }

                throw new FormatException($"Erreur de format lors de la conversion de l'id du site pour {id}");
            }

            throw new Exception(response.ReasonPhrase);
        }


        // Méthode GET pour récupérer l'id du service ou le salarie travaille avec l'id du salarie
        public static async Task<int> GetServiceIdbyUtilisateurId(int id)
        {
            string route = $"api/Utilisateurs/search/{id}/serviceId";

            var response = await Client.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {

                string resultat = await response.Content.ReadAsStringAsync();

                if (int.TryParse(resultat, out var serviceId))
                {
                    return serviceId;
                }

                throw new FormatException($"Erreur de format lors de la conversion de l'id du service pour {id}");
            }

            throw new Exception(response.ReasonPhrase);
        }


        //// Méthode PUT pour mettre à jour un salarié
        public static async Task<bool> UpdateSalarie(int id, UtilisateursDto salarie)
        {
            string route = $"api/Utilisateurs/{id}";
            var jsonContent = JsonConvert.SerializeObject(salarie);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await Client.PutAsync(route, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la mise à jour  du salarié : {response.ReasonPhrase}");
            }
        }

        // Méthode DELETE pour supprimer un salairé
        public static async Task<bool> DeleteSalarie(int id)
        {
            string route = $"api/Utilisateurs/{id}";
            var response = await Client.DeleteAsync(route);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Erreur lors de la suppression du salarie  : {response.ReasonPhrase}");
            }
        }
    }
}
