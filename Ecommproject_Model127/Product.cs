using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_Model127
{
    public  class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN {  get; set; }  
        [Required]
        [Range(1, 4000)]
        public string ListPrice {  get; set; }  
        [Required]
        [Range(1,3000)]
        public string Price {  get; set; }  
        [Required]
        [Range(1, 5000)]
        public string Price50 { get; set; }
        [Required]
        [Display(Name="Imageurl")]
        public string imageurl {  get; set; }   
        [Required]
        [Display(Name = "category")]
        public int categoryId {  get; set; }    
        public Category category { get; set; }
        [Required]
        [Display(Name = "coverType")]
        public int coverTypeId {  get; set; }   
        public CoverType coverType { get; set; }    
    }
}
