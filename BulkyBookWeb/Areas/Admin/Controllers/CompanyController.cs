using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model;
using BulkyBook.Model.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Company> objCompanyList = _unitOfWork.Company.GetAll();
            return View(objCompanyList);
        }

        //Get
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null || id == 0)
            {
                //create product
                return View(company);
            }
            else
            {
                //update product
                company = _unitOfWork.Company.GetFirstOrDefault(p => p.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "شرکت باموفقیت ایجاد شد.";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "شرکت باموفقیت ویرایش شد";
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
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
                return Json(new { success = false, message = "شرکت حذف نشد" });
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "شرکت باموفقیت حذف شد" });
        }
    }
}
#endregion