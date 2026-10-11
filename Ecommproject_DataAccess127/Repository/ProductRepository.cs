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
    public class ProductRepository : Repository<Product>, iProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
    
}
