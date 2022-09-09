using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model;
using BulkyBook.Model.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }

        //Get
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                    }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                    c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                    }),
            };
            if (id == null || id == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {
                //update product
                productVM.product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var perfix = @$"images\products\{DateTime.Now.Year}\{DateTime.Now.Month}\{DateTime.Now.Day}";
                    var uploads = Path.Combine(wwwRootPath, perfix);
                    var extension = Path.GetExtension(file.FileName);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    if (obj.product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl).TrimStart('\\');
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create);
                    file.CopyTo(fileStream);

                    obj.product.ImageUrl = perfix + "\\" + fileName + extension;
                }
                if (obj.product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.product);
                    TempData["success"] = "محصول باموفقیت ایجاد شد.";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.product);
                    TempData["success"] = "محصول باموفقیت ویرایش شد";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
                return Json(new { success = false, message = "محصول حذف نشد" });

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl).TrimStart('\\');
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "محصول باموفقیت حذف شد" });
        }
    }
}
#endregion