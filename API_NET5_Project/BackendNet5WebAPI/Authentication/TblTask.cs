using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendNet5WebAPI.Authentication
{
    public class TblTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }


    }
}
