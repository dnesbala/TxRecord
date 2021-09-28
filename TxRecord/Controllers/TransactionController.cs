using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TxRecord.Models;

namespace TxRecord.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TransactionController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _webHostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
            return View(_db.Transaction);
        }

        public IActionResult Insight()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            IEnumerable<SelectListItem> CategoryDropdown = _db.Category.Select(cat => new SelectListItem
            {
                Text = cat.Name,
                Value = cat.Id.ToString(),
            });
            ViewBag.CategoryDropdown = CategoryDropdown;
            return View();
        }

        [HttpPost]
        public IActionResult Add(Transaction Tx)
        {
            if (ModelState.IsValid)
            {
                if(Tx.Image != null)
                {

                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + WC.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                Tx.Image = fileName + extension;
                }
                Tx.DateAdded = DateTime.Now;
                _db.Transaction.Add(Tx);
                _db.SaveChanges();
                 
            }
            return RedirectToAction("Index");
        }
    }
}
