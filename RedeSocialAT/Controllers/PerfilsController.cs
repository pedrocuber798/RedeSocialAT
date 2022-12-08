using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RedeSocialAT.Data;
using RedeSocialAT.Models;
using RedeSocialAT.ViewModel;

namespace RedeSocialAT.Controllers
{
    public class PerfilsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationDbContext Context { get; set; }

        public PerfilsController(ApplicationDbContext context)
        {
            _context = context;
        }

            // GET: Perfils
            public async Task<IActionResult> Index()
        {
              return View(await _context.Perfil.ToListAsync());
        }

        // GET: Perfils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Perfil == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfil
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }


        private string UploadToAzureBlob(IFormFile image) {

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=cargalleryinfnetpc;AccountKey=72Adkm+pGR+6uxpgrvI0QOeT3LaXxAnAUaJaFeq/Uwb3iZdUsnFmVDUsRMtyIK/9xu0SceG4SCtn+ASthTnyIg==;EndpointSuffix=core.windows.net";
            string containerName = "images";
            string fileName = $"{Guid.NewGuid().ToString()}.jpg";

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            BlobClient blob = container.GetBlobClient(fileName);

            MemoryStream ms = new MemoryStream();
            image.CopyTo(ms);

            ms.Position = 0;
            blob.Upload(ms);

            return $"https://cargalleryinfnetpc.blob.core.windows.net/images/{fileName}";

        }
        private AddNewProfile CarregarPerfil() {
            var model = new AddNewProfile();
           model.Perfil = Context.Perfil.ToList();
            return model;
        }

        // GET: Perfils/Create
        public IActionResult Create() {
            //  AddNewProfile model = CarregarPerfil();
            return View();
        }

        // POST: Perfils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
 /*       [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (Perfil perfil) {
            Save(perfil);
            return View();
        }
*/
        [HttpPost]
        public IActionResult Create(Perfil perfil) {

            if (ModelState.IsValid == false) {
                var addForm = CarregarPerfil();
                return View("Add", addForm);
            }

            var p = new Perfil();
            p.Nome = perfil.Nome;



            p.Fotourl = UploadToAzureBlob(perfil.Fotourl);


            Save(perfil);


            return RedirectToAction("List");
        }
        public void Save(Perfil perfil) {
            Context.Perfil.Add(perfil);
            Context.SaveChanges();

        }
        // GET: Perfils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Perfil == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfil.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }
            return View(perfil);
        }

        // POST: Perfils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Fotourl")] Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilExists(perfil.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(perfil);
        }

        // GET: Perfils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Perfil == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfil
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }

        // POST: Perfils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Perfil == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Perfil'  is null.");
            }
            var perfil = await _context.Perfil.FindAsync(id);
            if (perfil != null)
            {
                _context.Perfil.Remove(perfil);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfilExists(int id)
        {
          return _context.Perfil.Any(e => e.Id == id);
        }
    }
}
