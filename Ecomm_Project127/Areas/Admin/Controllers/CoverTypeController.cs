using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using Ecommproject_utility127;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_Project127.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
          public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null) return View(coverType);   //create

            coverType = _unitOfWork.coverType.Get(id.GetValueOrDefault());  //edit
            //DynamicParameters param = new DynamicParameters();
            //param.Add("id",id.GetValueOrDefault());
            //coverType = _unitOfWork.SPCall.onerecord<coverType>(SD.proc_GetcoverType, param);

            if (coverType == null) return NotFound();
            return View(coverType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null) return BadRequest();
            if (!ModelState.IsValid) return View(coverType);
            //DynamicParameters param= new DynamicParameters();
            //param.Add("name",coverType.Name);
            if (coverType.Id == 0)
                _unitOfWork.coverType.Add(coverType);
            //_unitOfWork.SPCall.Execute(SD.proc_createcoverType, param);

            else
                //{
                //    param.Add("id", coverType.id);
                //    _unitOfWork.SPCall.Execute(SD.proc_updatecoverType, param);
                //}
                _unitOfWork.coverType.update(coverType);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.coverType.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var coverTypeinDb = _unitOfWork.coverType.Get(id);
            if (coverTypeinDb == null)
                return Json(new { success = false, Message = "Something went wrong while Delete Data!!" });
            //DynamicParameters param = new DynamicParameters();
            //param.Add("id", id);
            //_unitOfWork.SPCall.Execute(SD.proc_DeletecoverType, param);
            else
                _unitOfWork.coverType.Remove(coverTypeinDb);
            _unitOfWork.save();
            return Json(new { success = true, Message = "Data Deleted Successfully" });
        }
        #endregion
    }
}
