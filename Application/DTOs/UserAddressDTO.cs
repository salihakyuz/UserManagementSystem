

using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs
{
    public class UserAddressDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } //burası userId olabilir. 
        public string Title { get; set; }
        public AddressEnum Type { get; set; }
        public string Address { get; set; }

    }
}
