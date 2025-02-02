using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrooAnnauireModel.Entities
{
    public class Utilisateurs
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public int TelephoneFixe { get; set; }

        public int TelephonePortable { get; set; }

        [StringLength(100)]
        public string MotDePasse { get; set; } = string.Empty;

        public bool EstAdmin { get; set; }



        //relation  avec l'entité Services
        [ForeignKey(nameof(Services))]
        public int ServicesId { get; set; }
        public Services? Services { get; set; }

        //relation  avec l'entité Sites
        [ForeignKey(nameof(Sites))]
        public int SitesId { get; set; }
        public Sites? Sites { get; set; }
    }
}
