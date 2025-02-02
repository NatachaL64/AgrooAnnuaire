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
    public class SitesController : ControllerBase
    {
        private readonly AgrooAnnuaireContext _context;

        public SitesController(AgrooAnnuaireContext context)
        {
            _context = context;
        }

        // GET: api/Sites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SitesDto>>> GetSites()
        {
            return await _context.Sites.Select(site => new SitesDto
            {
                Id = site.Id,
                NomVille = site.NomVille,


            }).ToListAsync();
        }

        //GET: api/Sites/searchnbUtilisateur/{id}
        [HttpGet("GetNombreUtilisateursBySiteId/{siteId}")]
        public async Task<IActionResult> GetNombreUtilisateursBySiteId(int siteId)
        {
            // Vérifier si le site existe
            var siteExiste = await _context.Sites.AnyAsync(s => s.Id == siteId);
            if (!siteExiste)
            {
                return NotFound($"Aucun site trouvé avec l'ID {siteId}");
            }

            // Compter les utilisateurs liés à ce site
            int nombreUtilisateurs = await _context.Utilisateurs.CountAsync(u => u.SitesId == siteId);

            return Ok(nombreUtilisateurs);
        }

        // GET: api/Sites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SitesDto>> GetSites(int id)
        {
            var site = await _context.Sites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return new SitesDto
            {
                Id = site.Id,
                NomVille = site.NomVille,

            };
        }

        // PUT: api/Sites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSites(int id, Sites sites)
        {
            if (id != sites.Id)
            {
                return BadRequest();
            }

            _context.Entry(sites).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SitesExists(id))
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

        // POST: api/Sites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sites>> PostSites(Sites sites)
        {
            _context.Sites.Add(sites);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSites", new { id = sites.Id }, sites);
        }

        // DELETE: api/Sites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSites(int id)
        {
            var sites = await _context.Sites.FindAsync(id);
            if (sites == null)
            {
                return NotFound();
            }

            _context.Sites.Remove(sites);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SitesExists(int id)
        {
            return _context.Sites.Any(e => e.Id == id);
        }
    }
}
