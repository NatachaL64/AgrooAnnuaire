using AgrooAnnauireModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrooAnnuaireWPF.ViewModels
{
    internal class AccueilViewModel
    {
        public string Titre { get; set; } = "Page d'accueil";

        // Rendre la propriété 'utilisateur' publique
        
        public string BienvenueMessage => $"Bonjour sur AgrooAnnuaire";

    }
}
