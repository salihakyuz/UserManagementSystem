using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories
{
    internal class UserRepostory : IUser
    {
        private readonly AppDbContext appDbContext; //burada neden _context'in işe yaramadıpını sor. yapıldığı takdirde login dto içine yazılanlar çalışmıyor
        private readonly IConfiguration configuration;

        public UserRepostory(AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;  //nedne _context= context; aşağıda çalışmıyor sor...
            this.configuration = configuration;
        }
        public async Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO)
        {
            var getUser = await FindUserByEmail(loginDTO.UserEmail!);
            if (getUser == null)
                return new LoginResponse(false, "Kullanıcı Bulunamadı!");


            bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password); //password check buradan yapılıyor
            if (checkPassword)
                return new LoginResponse(true, "Giriş Başarılı!", GenerateToken(getUser), roleType:getUser.RoleType);//eğer doğruysa giriş için token üretilecek
            else
                return new LoginResponse(false, "Giriş bilgileri uyuşmamakta!");
        }

        private string GenerateToken(User user)//token üretmek için method ürettik  UserAdress userAdress
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)); //configuration kullanmak için IConfiguration configuration eklemen lazım en üste
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.UserEmail!),
                new Claim(ClaimTypes.Role,Convert.ToString(user.RoleType)) //jwtnin içine roletype kyamıyorum. güncelleme: tostrinng koydum,sonuç çalışıyor
                //new Claim(ClaimTypes.NameIdentifier,Convert.ToString(userAdress.Address))//kontrol et ve aşağıda da belirt
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> FindUserByEmail(string email) =>
            await appDbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == email);

        public async Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var getUser = await FindUserByEmail(registerUserDTO.UserEmail!);
            if (getUser != null)
                return new RegistrationResponse(false, "Kullanıcı zaten mevcut");

            var newUser = new User
            {
                UserName = registerUserDTO.UserName,
                UserEmail = registerUserDTO.UserEmail,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password),
                RoleType =RoleEnum.Basic
            };

            await appDbContext.Users.AddAsync(newUser);

            var newUserAddress = new UserAdress
            {
                UserName = newUser,
                Title = registerUserDTO.AddressTitle!,
                Type = registerUserDTO.AddressType!.Value,
                Address = registerUserDTO.Address!
            };

            await appDbContext.Adresses.AddAsync(newUserAddress);
            await appDbContext.SaveChangesAsync();

            return new RegistrationResponse(true, "Kullanıcı kaydı tamamlanmıştır");
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await appDbContext.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    UserEmail = u.UserEmail,
                    //AddressType = u.AddressType,//gerekli olup olmadığını sor!!
                    RoleType = u.RoleType
                })
                .ToListAsync();
            
            return users;
           
        }
        
        public async Task<bool> ChangeUserRoleAsync(int userId, RoleEnum newRole)
        {
            var user = await appDbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.RoleType = newRole;
            
            await appDbContext.SaveChangesAsync();
            return true;
        }

        /*addressenum değerlerine göre seçim yaparak kullanıcı adres bilgilerini ve ismini çekebiliyoruz*/
        public async Task<IEnumerable<UserAddressDTO>> GetUsersByAddressAsync(AddressEnum addressType) //tüm userradressler useradressdto ile değişti
        {
            var users = await appDbContext.Adresses
                .Where(ua => ua.Type == addressType)
                .Select(ua => new UserAddressDTO
                {
                    Id = ua.Id,
                    UserName = ua.UserName.UserName,
                    Title = ua.Title,
                    Type = ua.Type,
                    Address = ua.Address

                    //DENEME NOTLARI: ADDRES BİLGİLERİNDE MAİL VE ROL TUTMAYA GEREK YOK 
                    //Id = ua.UserName.Id,
                    //UserName = ua.UserName.UserName,
                    //UserEmail = ua.UserName.UserEmail,
                    //RoleType = ua.UserName.RoleType
                })
                .ToListAsync();
            return (IEnumerable<UserAddressDTO>)users;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user= await appDbContext.Users.FindAsync(userId);
            if (user == null) return false;

            appDbContext.Users.Remove(user);
           
            //userla ilişkili addres varsa onu da sil
            var userAddreses = await appDbContext.Adresses.Where(ua=>ua.UserName.UserName == user.UserName).ToListAsync();
            appDbContext.Adresses.RemoveRange(userAddreses);
            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
