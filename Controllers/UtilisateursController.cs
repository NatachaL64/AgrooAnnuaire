using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgrooAnnauireModel.Context;
using AgrooAnnauireModel.Entities;
using AgrooAnnauireModel.Dto;
using Microsoft.AspNetCore.Authorization;

namespace AgrooAnnuaireAPI.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly AgrooAnnuaireContext _context;

        public UtilisateursController(AgrooAnnuaireContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilisateursDto>>> GetUtilisateurs()
        {
            var utilisateurs = await _context.Utilisateurs
                .Include(a => a.Services)
                .Include(a => a.Sites)

                .Select(utilisateurs => new UtilisateursDto
                {
                    Id = utilisateurs.Id,
                    Nom = utilisateurs.Nom,
                    Prenom = utilisateurs.Prenom,
                    Email = utilisateurs.Email,
                    TelephonePortable = utilisateurs.TelephonePortable,
                    TelephoneFixe = utilisateurs.TelephoneFixe,
                    MotDePasse = utilisateurs.MotDePasse,
                    ServiceId = utilisateurs.Services.Id,
                    ServiceNom = utilisateurs.Services.Nom,
                    SiteId = utilisateurs.Sites.Id,
                    SiteNom = utilisateurs.Sites.NomVille,
                    EstAdmin = utilisateurs.EstAdmin,


                })
                .ToListAsync();

            return Ok(utilisateurs);
        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilisateursDto>> GetUtilisateurs(int id)
        {
            var utilisateur = await _context.Utilisateurs
                 .Include(a => a.Services)
                .Include(a => a.Sites)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return new UtilisateursDto
            {
                Id = utilisateur.Id,
                Nom = utilisateur.Nom,
                Prenom = utilisateur.Prenom,
                Email = utilisateur.Email,
                TelephonePortable = utilisateur.TelephonePortable,
                TelephoneFixe = utilisateur.TelephoneFixe,
                MotDePasse = utilisateur.MotDePasse,
                ServiceId = utilisateur.Services.Id,
                ServiceNom = utilisateur.Services.Nom,
                SiteId = utilisateur.Sites.Id,
                SiteNom = utilisateur.Sites.NomVille,
                EstAdmin = utilisateur.EstAdmin,
            };
        }


        // GET: api/Utilisateurs/search/{id}/siteId
        [HttpGet("search/{id}/sitesid")]
        public async Task<IActionResult> GetSiteIdbyUtilisateurId(int id)
        {
            var utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(a => a.Id == id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            // Retourner l'id du site ou travaille le salarié'
            return Ok(utilisateur.SitesId);
        }

        // GET: api/Utilisateurs/search/{id}/serviceId
        [HttpGet("search/{id}/servicesid")]
        public async Task<IActionResult> GetServiceIdbyUtilisateurId(int id)
        {
            var utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(a => a.Id == id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            // Retourner l'id du service ou travaille le salarié'
            return Ok(utilisateur.ServicesId);
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateurs(int id, Utilisateurs utilisateurs)
        {
            if (id != utilisateurs.Id)
            {
                return BadRequest();
            }

            _context.Entry(utilisateurs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateursExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilisateurs>> PostUtilisateurs(Utilisateurs utilisateurs)
        {
            _context.Utilisateurs.Add(utilisateurs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateurs", new { id = utilisateurs.Id }, utilisateurs);
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilisateurs(int id)
        {
            var utilisateurs = await _context.Utilisateurs.FindAsync(id);
            if (utilisateurs == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateurs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilisateursExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.Id == id);
        }
    }
}
