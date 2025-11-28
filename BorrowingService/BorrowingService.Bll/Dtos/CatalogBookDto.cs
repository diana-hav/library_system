using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorrowingService.Bll.Dtos
{
    public class CatalogBookDto
    {
        public int Id { get; set; }          
        public required string Title { get; set; }    
        public required string AuthorName { get; set; } 
    }
}
