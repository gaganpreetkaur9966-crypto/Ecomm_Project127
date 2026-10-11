using Ecommproject_DataAccess127.Data;
using Ecommproject_DataAccess127.Repository.iRepository;
using Ecommproject_Model127;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_DataAccess127.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context; 
            category=new CategoryRepository(_context);
            coverType= new CoverTypeRepository(_context);
            product = new ProductRepository(_context);
            shoppingcart = new ShoppingCartRepository(_context);
            applicationUser=new ApplicationuserRepository(_context);
            company = new CompanyRepository(_context);
            orderDetails=new orderDetailsRepository(_context);
            orderHeader=new orderHeaderRepository(_context);    
        }
        public iCategoryRepository category { private set; get; }

        public iCoverTypeRepository coverType { private set; get; }

        public iProductRepository product {  private set; get; }

        public iShoppingCartRepository shoppingcart {  private set; get; }

        public iApplicationUserRepository applicationUser {  private set; get; }

        public iCompanyRepository company {  private set; get; }

        public iOrderDetailsRepository orderDetails { private set; get; }

        public iOrderHeaderRepository OrderHeader { private set; get; }

        public iOrderHeaderRepository orderHeader {  private set; get; }    

        public void save()
        {
            _context.SaveChanges();
        }
    }
}
