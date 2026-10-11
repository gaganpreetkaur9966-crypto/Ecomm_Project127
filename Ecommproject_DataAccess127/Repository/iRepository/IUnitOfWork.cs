using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecommproject_DataAccess127.Repository.iRepository;

namespace Ecommproject_DataAccess127.Repository.iRepository
{
    public interface IUnitOfWork
    {
        iCategoryRepository category { get; }
        iCoverTypeRepository coverType { get; }
        iProductRepository product { get; }
        iShoppingCartRepository shoppingcart { get; }
        iApplicationUserRepository applicationUser { get; }
        iCompanyRepository company { get; } 
        iOrderDetailsRepository orderDetails { get; }
        iOrderHeaderRepository orderHeader { get; }

        void save();
    }
}

