using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using Ecommproject_utility127;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ecomm_Project127.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
                return View(company);
            company = _unitOfWork.company.Get(id.GetValueOrDefault());
            if (company == null) return NotFound();
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult upsert(Company company)
        {
            if (company == null) return BadRequest();
            if (!ModelState.IsValid) return View(company);
            if (company.Id == 0)
                _unitOfWork.company.Add(company);
            else
                _unitOfWork.company.update(company);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        public IActionResult GetAll()
        {
            return Json(new {data=_unitOfWork.company.GetAll()});   
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var companyInDb = _unitOfWork.company.Get(id);
            if (companyInDb == null)
                return Json(new { success = false, Message = "Something went wrong while Delete Data!!" });
            _unitOfWork.company.Remove(companyInDb);
            _unitOfWork.save();
            return Json(new { success = true, Message = "Data Deleted successfully!!" });
        }
        #endregion
    }
}
