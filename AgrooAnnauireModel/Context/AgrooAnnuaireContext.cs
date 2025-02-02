using AgrooAnnauireModel.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrooAnnauireModel.Context
{
    public class AgrooAnnuaireContext(DbContextOptions<AgrooAnnuaireContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Services> Services { get; set; }
        public DbSet<Sites> Sites { get; set; }

        public DbSet<Utilisateurs> Utilisateurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connString = "server=localhost;database=agrooannuaire;user=root; password=";
            optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
        }
    }
}
