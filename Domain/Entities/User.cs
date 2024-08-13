using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public RoleEnum RoleType { get; set; }
        //public AddressEnum AddressType { get; set; } Bu kısım veri tabanında hep sıfır olarak görünmekte. sebebini sor
    }
}
