using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendNet5WebAPI.ResponseModels
{
    public class RegisterViewModel
    {
        public RegisterUser user { get; set; }
    }


    public class RegisterUser
    {
        public string id { get; set; }
        public string email { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }

}
