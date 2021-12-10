using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RR_hookah.Data;
using RR_hookah.Models;
using RR_hookah.Models.ViewModels;

namespace RR_hookah.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // best practice
            IEnumerable<Product> objList = _db.Product.Include(u=> u.Category);


            // bad practice
            /* foreach(var obj in objList)
               {
                   obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
               }*/


            return View(objList);
        }

        // get - upsert 
        public IActionResult Upsert(int? id)
        {
            // ViewBag ViewData

            /* IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            // ViewBag.CategoryDropDown = CategoryDropDown;
            ViewData["CategoryDropDown"] = CategoryDropDown; */


            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),    
            };

            if (id == null)
            {
                return View(productVM);
            } else
            {
                productVM.Product = _db.Product.Find(id);
                if(productVM == null)
                {
                    return NotFound();
                } else
                {
                    return View(productVM);
                }
            }
        }

        // post - upsert
        [HttpPost]
        // токен защиты от взлома для форм
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(productVM.Product.Id == 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    // копирование файла в новое место
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    // обновляем ссылку
                    productVM.Product.Image = fileName + extension;

                    // добавляем товар
                    _db.Product.Add(productVM.Product);
                } else
                {
                    // AsNoTracking объяснение в MyFile.txt 37-38 пункт
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);

                    // true если новый файл уже был получен для сущ продукта
                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        // удаление старого файла
                        var oldFile = Path.Combine(upload, objFromDb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        // копирование файла в новое место
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        // сохранение ссылки на файл
                        productVM.Product.Image = fileName + extension;
                    } else
                    {
                        // файл не менялся, но обновились другие св-ва товара
                        productVM.Product.Image = objFromDb.Image;

                    }
                    // обновление бд
                    _db.Product.Update(productVM.Product);
                }

                // сохранение изменений в бд
                _db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                // фикс состояние модели недействительно
                productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productVM);
            }
        }

        // get - delete 
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = _db.Product.Include(u=>u.Category).FirstOrDefault(u=>u.Id==id);
            
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // post - delete 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            } else
            {
                string upload =  _webHostEnvironment.WebRootPath + WC.ImagePath;
                var oldFile = Path.Combine(upload, obj.Image);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }


                _db.Product.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
