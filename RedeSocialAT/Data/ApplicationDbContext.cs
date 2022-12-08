using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedeSocialAT.Models;
using System.Drawing.Drawing2D;

namespace RedeSocialAT.Data {
    public class ApplicationDbContext : IdentityDbContext {

        public DbSet<RedeSocialAT.Models.Perfil> Perfil { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }
        
    }
}