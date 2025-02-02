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

    public class ServicesController : ControllerBase
    {
        private readonly AgrooAnnuaireContext _context;

        public ServicesController(AgrooAnnuaireContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicesDto>>> GetServices()
        {
            return await _context.Services.Select(service => new ServicesDto
            {
                Id = service.Id,
                Nom = service.Nom,


            }).ToListAsync();
        }


        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicesDto>> GetServices(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return new ServicesDto
            {
                Id = service.Id,
                Nom = service.Nom,

            };
        }

        //GET: api/Services/searchnbUtilisateur/{id}
        [HttpGet("GetNombreUtilisateursByServiceId/{serviceId}")]
        public async Task<IActionResult> GetNombreUtilisateursByServiceId(int serviceId)
        {
            // Vérifier si le site existe
            var serviceExiste = await _context.Services.AnyAsync(s => s.Id == serviceId);
            if (!serviceExiste)
            {
                return NotFound($"Aucun service trouvé avec l'ID {serviceId}");
            }

            // Compter les utilisateurs liés à ce site
            int nombreUtilisateurs = await _context.Utilisateurs.CountAsync(u => u.ServicesId == serviceId);

            return Ok(nombreUtilisateurs);
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServices(int id, Services services)
        {
            if (id != services.Id)
            {
                return BadRequest();
            }

            _context.Entry(services).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Services>> PostServices(Services services)
        {
            _context.Services.Add(services);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServices", new { id = services.Id }, services);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServices(int id)
        {
            var services = await _context.Services.FindAsync(id);
            if (services == null)
            {
                return NotFound();
            }

            _context.Services.Remove(services);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicesExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
