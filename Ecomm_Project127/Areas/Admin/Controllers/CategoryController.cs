using Ecommproject_DataAccess127.Repository;
using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using Ecommproject_utility127;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_Project127.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null) return View(category); //create
            category = _unitofwork.category.Get(id.GetValueOrDefault());
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View(category);
            if (category.Id == 0)
                _unitofwork.category.Add(category);
            else
                _unitofwork.category.update(category);
            _unitofwork.save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var categorylist=_unitofwork.category.GetAll(); 
            return Json(new {data=categorylist});   
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryInDb = _unitofwork.category.Get(id);
            if (categoryInDb == null)
                return Json(new { success = false, Message = "Something went wrong while Delete Data!!!" });
            _unitofwork.category.Remove(categoryInDb);
            _unitofwork.save();
            return Json(new { success = true, Message = "Data Deleted Successfully!!" });
        }
        #endregion

    }
}

