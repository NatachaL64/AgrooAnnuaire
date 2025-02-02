using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrooAnnauireModel.Dto
{
    public class UtilisateursDto
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

        public string? ServiceNom { get; set; }
        public int ServiceId { get; set; }

        public string? SiteNom { get; set; }
        public int SiteId { get; set; }
    }
}
