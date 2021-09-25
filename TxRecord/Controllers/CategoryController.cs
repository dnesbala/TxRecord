using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TxRecord.Models;

namespace TxRecord.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Category);
        }

        [HttpGet] 
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            if(ModelState.IsValid)
            {
            _db.Category.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
