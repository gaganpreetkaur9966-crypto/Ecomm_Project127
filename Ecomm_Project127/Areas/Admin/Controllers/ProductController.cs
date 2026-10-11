using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using Ecommproject_Model127.ViewModels;
using Ecommproject_utility127;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecomm_Project127.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class productController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public productController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                product = new Product(),
                categorylist = _unitOfWork.category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                coverTypelist = _unitOfWork.coverType.GetAll().Select(ct => new SelectListItem()
                {
                    Text = ct.Name,
                    Value = ct.Id.ToString()
                })
            };
            if (id == null) return View(productVM);
            productVM.product = _unitOfWork.product.Get(id.GetValueOrDefault());
            if (productVM.product == null) return NotFound();
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult upsert(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    var filesName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var uploads = Path.Combine(webRootPath, @"Images\Products"); //store image in folder
                    if (productVM.product.Id != 0) //update
                    {
                        var imageExists = _unitOfWork.product.Get(productVM.product.Id).imageurl;
                        productVM.product.imageurl = imageExists; //in update case checking image is already exist or not
                    }
                    if (productVM.product.imageurl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.product.imageurl.Trim('\\'));
                        if (System.IO.File.Exists(imagePath)) //if image already exist then delete it
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, filesName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream); //save new image in folder
                    }
                    productVM.product.imageurl = @"\Images\Products\" + filesName + extension; //url of image which is saved on folder
                }
                else //if not image is there in update case then save new 
                {
                    if (productVM.product.Id != 0)
                    {
                        var imageExists = _unitOfWork.product.Get(productVM.product.Id).imageurl;
                        productVM.product.imageurl = imageExists;
                    }
                }
                if (productVM.product.Id == 0) //simple Add
                    _unitOfWork.product.Add(productVM.product);
                else
                    _unitOfWork.product.update(productVM.product); //simple update
                _unitOfWork.save(); //save in table
                return RedirectToAction(nameof(Index)); //after saving going to index
            }
            else //if we can't select any file then category or coverType id going to server
            {
                productVM = new ProductVM() //if id is not null then
                {
                    product = new Product(),
                    categorylist = _unitOfWork.category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()
                    }),
                    coverTypelist = _unitOfWork.coverType.GetAll().Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString()
                    })
                };
                if (productVM.product.Id != 0)
                {
                    productVM.product = _unitOfWork.product.Get(productVM.product.Id);
                }
                return View(productVM);
            }
        }
        #region APIs
        [HttpGet] 
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.product.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productInDb = _unitOfWork.product.Get(id);
            if (productInDb == null)
                return Json(new { success = false, Message = "something went wrong while delete data!!" });
            var imageurl = _unitOfWork.product.Get(id).imageurl;
            var WebRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(WebRootPath, productInDb.imageurl.Trim('\\'));
            if (System.IO.File.Exists(imagePath)) //if image already exist then delete it
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.product.Remove(productInDb);
            //_unitOfWork.save();
            return Json(new { success = true, Message = "data deleted successfully!!" });
        }
        #endregion
    }
}
