using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserAdress
    {
        public int Id { get; set; }
        public User UserName { get; set; } //burası userId olabilir. 
        public string Title { get; set; }
        public AddressEnum Type { get; set; }
        public string Address { get; set; }
    }
}
