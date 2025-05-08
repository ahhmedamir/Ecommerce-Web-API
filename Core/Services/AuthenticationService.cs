using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;

namespace Services
{
    internal class AuthenticationService(UserManager<User> userManager,IConfiguration configuration,IOptions<JwtOptions> options,IMapper mapper)
        : IAuthenticationService
    {
        public async Task<bool> CheckEmailExist(string email)
        {
            var User = await userManager.FindByEmailAsync(email);
            return User != null;
        }

        public async Task<AddressDto> GetUserAddress(string email)
        {
            var User = await userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new UserNotFoundException(email);
            return mapper.Map<AddressDto>(User.Address);
        }

        public async Task<UserResultDto> GetUserByEmail(string email)
        {
            var User = await userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);
            return new UserResultDto
              (
                User.DisplayName,
                User.Email,
                await CreateTokenAsync(User)
              );
        }



        public  async Task<AddressDto> UpdateUserAddress(AddressDto address , string email)
        {
            var User = await userManager.Users.Include(s => s.Address)
               .FirstOrDefaultAsync(u => u.Email == email)
               ?? throw new UserNotFoundException(email);
            if (User.Address != null)
            {
                User.Address.FirstName = address.FirstName;
                User.Address.LastName = address.LastName;
                User.Address.Street = address.Street;
                User.Address.City = address.City;
                User.Address.Country = address.Country;


            }
            else
            {
                var UserAddress = mapper.Map<Address>(address);
                User.Address = UserAddress;
            }
            await userManager.UpdateAsync(User);
            return address;
             
        }
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //Check If The User Under This Email
            var User = await userManager.FindByEmailAsync(loginDto.Email);
            if (User == null) throw new UnAuthorizedException("Email Doesn't Exist");
            //Check If The Password is Correct For This Email
            var Result = await userManager.CheckPasswordAsync(User, loginDto.Password);
            if (!Result) throw new UnAuthorizedException("Password Is InCorrect");
            //Create Token And Return Response
            return new UserResultDto
               (
                User.DisplayName,
                User.Email,
                 await CreateTokenAsync(User)
                );
        }

        public  async Task<UserResultDto> RegisterAsync(UserRegisterDto registerDto)
        {
            var User = new User()
            {
Email= registerDto.Email,
DisplayName= registerDto.DisplayName,
PhoneNumber= registerDto.PhoneNumber,
UserName= registerDto.UserName,
            };
            var Result = await userManager.CreateAsync(User, registerDto.Password);
            if(!Result.Succeeded)
            {
                var errors = Result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }
            return new UserResultDto(
                User.DisplayName,
                User.Email,
                await CreateTokenAsync(User));
           
        }

       

        private async Task<string>CreateTokenAsync(User user)
        {
            var jwtoptions = options.Value;
            //Private Claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!)
            };
            //Add Roles To Claims
            var roles = await userManager.GetRolesAsync(user);
            foreach( var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            }
            // SecurityKey
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey));
            var signingCreds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            // Data For Token
            var Token = new JwtSecurityToken(
                audience:jwtoptions.Audience,
                issuer: jwtoptions.Issuer,
                expires:DateTime.UtcNow.AddDays(jwtoptions.DurationInDays),
                claims:authClaims,
                signingCredentials:signingCreds

                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        
        
        }


    }
}
