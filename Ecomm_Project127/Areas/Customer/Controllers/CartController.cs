using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using Ecommproject_Model127.ViewModels;
using Ecommproject_utility127;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace Ecomm_Project127.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public ShoppingcartVM shoppingcartVM { get; set; }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)(User.Identity);
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null)
            {
                shoppingcartVM = new ShoppingcartVM()
                {
                    Listcart = new List<ShoppingCart>()
                };
                return View(shoppingcartVM);
            }
            shoppingcartVM = new ShoppingcartVM()
            {
                Listcart = _unitOfWork.shoppingcart.GetAll(sc => sc.Applicationuserid == claims.Value, includeProperties: "Product"),
                orderHeader = new OrderHeader()
            };
            shoppingcartVM.orderHeader.orderTotal = 0;
            shoppingcartVM.orderHeader.Applicationuser = _unitOfWork.applicationUser.FirstOrDefault(au => au.Id == claims.Value);
            foreach (var list in shoppingcartVM.Listcart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.count, list.Product.Price, list.Product.Price50);
                shoppingcartVM.orderHeader.orderTotal += (list.count * list.Price);
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }
            return View(shoppingcartVM);
        }

        public IActionResult Plus(int id)
        {
            var cart = _unitOfWork.shoppingcart.FirstOrDefault(sc => sc.Id == id);
            cart.count += 1;
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int id)
        {
            var cart = _unitOfWork.shoppingcart.FirstOrDefault(sc => sc.Id == id);
            if (cart.count == 1)
                cart.count = 1;
            else
                cart.count -= 1;
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var cart = _unitOfWork.shoppingcart.FirstOrDefault(sc => sc.Id == id);
            _unitOfWork.shoppingcart.Remove(cart);
            _unitOfWork.save();
            //session
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var count = _unitOfWork.shoppingcart.GetAll(sc => sc.Applicationuserid == claims.Value).ToList().Count;
            HttpContext.Session.SetInt32(SD.Ss_cartSessioncount, count);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcartVM = new ShoppingcartVM()
            {
                Listcart = _unitOfWork.shoppingcart.GetAll(sc => sc.Applicationuserid == claims.Value, includeProperties: "Product"),
                orderHeader = new OrderHeader()
            };
            shoppingcartVM.orderHeader.Applicationuser = _unitOfWork.applicationUser.FirstOrDefault(au => au.Id == claims.Value);
            foreach (var list in shoppingcartVM.Listcart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.count, list.Product.Price, list.Product.Price50);
                shoppingcartVM.orderHeader.orderTotal += list.Price * list.count;
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }
            shoppingcartVM.orderHeader.Name = shoppingcartVM.orderHeader.Applicationuser.Name;
            shoppingcartVM.orderHeader.streetAddress = shoppingcartVM.orderHeader.Applicationuser.streetAddress;
            shoppingcartVM.orderHeader.state = shoppingcartVM.orderHeader.Applicationuser.state;
            shoppingcartVM.orderHeader.city = shoppingcartVM.orderHeader.Applicationuser.city;
            shoppingcartVM.orderHeader.Postalcode = shoppingcartVM.orderHeader.Applicationuser.Postalcode;
            //shoppingcartVM.orderHeader.PhoneNumber = shoppingcartVM.orderHeader.Applicationuser.PhoneNumber;
            return View(shoppingcartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcartVM.orderHeader.Applicationuser = _unitOfWork.applicationUser.FirstOrDefault(au => au.Id == claims.Value);
            shoppingcartVM.Listcart = _unitOfWork.shoppingcart.GetAll(Sc => Sc.Applicationuserid == claims.Value, includeProperties: "Product");
            shoppingcartVM.orderHeader.orderStatus = SD.orderstatusPending;
            shoppingcartVM.orderHeader.PaymentStatus = SD.PaymentstatusPending;
            shoppingcartVM.orderHeader.orderDate = DateTime.Now;
            shoppingcartVM.orderHeader.ApplicationuserId = claims.Value;
            //_unitOfWork.orderHeader.Add(shoppingcartVM.orderHeader);
            _unitOfWork.save();
            foreach (var list in shoppingcartVM.Listcart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.count, list.Product.Price, list.Product.Price50);
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = list.Productid,
                    orderHeaderId = shoppingcartVM.orderHeader.Id,
                    Price = list.Price,
                    count = list.count
                };
                shoppingcartVM.orderHeader.orderTotal = (list.Price * list.count);
                _unitOfWork.orderDetails.Add(orderDetails);
                _unitOfWork.save();
            }
            _unitOfWork.shoppingcart.RemoveRange(shoppingcartVM.Listcart);
            _unitOfWork.save();

            //session
            HttpContext.Session.SetInt32(SD.Ss_cartSessioncount, 0);
            #region stripe
            if (stripeToken == null)
            {
                shoppingcartVM.orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                shoppingcartVM.orderHeader.PaymentStatus = SD.PaymentstatusDelayPayment;
                shoppingcartVM.orderHeader.orderStatus = SD.orderstatusApproved;
            }
            else
            {
                var options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(shoppingcartVM.orderHeader.orderTotal),
                    Currency = "usd",
                    Description = "order.id:" + shoppingcartVM.orderHeader.Id.ToString(),
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (charge.BalanceTransactionId == null)
                    shoppingcartVM.orderHeader.PaymentStatus = SD.PaymentstatusRejected;
                else
                    shoppingcartVM.orderHeader.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    shoppingcartVM.orderHeader.orderStatus = SD.orderstatusApproved;
                    shoppingcartVM.orderHeader.PaymentStatus = SD.PaymentstatusApproved;
                    shoppingcartVM.orderHeader.orderDate = DateTime.Now;
                }
                _unitOfWork.save();
            }
            #endregion
            return RedirectToAction("orderconfirmation", "cart", new { id = shoppingcartVM.orderHeader.Id });
        }
        public IActionResult orderconfirmation(int id)
        {
            return View(id);
        }
    }
}

