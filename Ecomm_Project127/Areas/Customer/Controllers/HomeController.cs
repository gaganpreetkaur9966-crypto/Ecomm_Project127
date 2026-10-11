using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using Ecommproject_Model127.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecomm_Project127.Areas.Customer.Controllers
{
    [Area("customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;   

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork    )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var productlist = _unitOfWork.product.GetAll(includeProperties: "category,coverType");
            return View(productlist);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Details(int id)
        {
            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                var count = _unitOfWork.shoppingcart.GetAll(sc => sc.Applicationuserid == claims.Value).ToList().Count;
                //HttpContext.Session.SetInt32(SD.Ss_cartSessioncount, count);
            }
            var productInDb = _unitOfWork.product.FirstOrDefault(p => p.Id == id, includeProperties: "category,coverType");
            if (productInDb == null) return NotFound();
            var shoppingCart = new ShoppingCart()
            {
                Product = productInDb,
                Productid = productInDb.Id
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingcart)
        {
            shoppingcart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity; //  find user identity
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // find user identity
                if (claims == null) return NotFound();
                shoppingcart.Applicationuserid = claims.Value;
                var shoppingcartInDb = _unitOfWork.shoppingcart.FirstOrDefault(sc => sc.Applicationuserid == claims.Value && sc.Productid == shoppingcart.Productid);
                if (shoppingcartInDb == null)
                    _unitOfWork.shoppingcart.Add(shoppingcart);
                else
                    shoppingcartInDb.count += shoppingcart.count;
                _unitOfWork.save();
                return RedirectToAction("Index");
            }
            else
            {
                var productInDb = _unitOfWork.product.FirstOrDefault(p => p.Id == shoppingcart.Id, includeProperties: "category,coverType");
                if (productInDb == null) return NotFound();
                var ShoppingcartEdit = new ShoppingCart()
                {
                    Product = productInDb,
                    Productid = productInDb.Id
                };
                return View(ShoppingcartEdit);
            }
        }
    }
}
