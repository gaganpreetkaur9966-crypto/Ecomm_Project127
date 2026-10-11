using Ecommproject_DataAccess127.Data;
using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using Ecommproject_utility127;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ecomm_Project127.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class userController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public userController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var userlist = _context.ApplicationUsers.ToList();  // AspNetUser
            var roles = _context.Roles.ToList();  //AspNetRoles
            var UserRoles = _context.UserRoles.ToList();  //AspnetUserRoles
            foreach (var user in userlist)
            {
                var roleId = UserRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                if (user.companyid == null)
                {
                    user.company = new Company()
                    {
                        Name = ""
                    };
                }
                if (user.companyid != null)
                {
                    user.company = new Company()
                    {
                        Name = _unitOfWork.company.Get(Convert.ToInt32(user.companyid)).Name
                    };
                }
            }
            //Remove admin role user
            var adminuser = userlist.FirstOrDefault(u => u.Role == SD.Role_Admin);
            userlist.Remove(adminuser);
            return Json(new { data = userlist });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            bool islocked = false;
            var userInDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (userInDb == null)
                return Json(new { success = false, Message = "something went wrong while lock or unlock user" });
            if (userInDb != null && userInDb.LockoutEnd > DateTime.Now)
            {
                userInDb.LockoutEnd = DateTime.Now;
                islocked = false;
            }
            else
            {
                userInDb.LockoutEnd = DateTime.Now.AddYears(100);
                islocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, Message = islocked == true ? "user sucessfully locked" : "user successfullly unlocked" });
        }
        #endregion
    }
}
