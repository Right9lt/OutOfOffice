using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Context;
using OutOfOffice.DTO.Auth;
using OutOfOffice.Entities;
using OutOfOffice.Helpers;
using System.Security.Claims;

namespace OutOfOffice.Services
{
    public class AuthService(OutOfOfficeContext context)
    {

        private readonly OutOfOfficeContext _context = context;


        public async Task<Result<ClaimsIdentity>> Login(LoginDTO model)
        {

            var emp = await _context.Employees.FirstOrDefaultAsync(e => e.Email == model.Email);
            

            if (emp == null || !VerifyPassword(emp, model.Password))
            {
                return Result.Failure<ClaimsIdentity>("Wrong credentials");
            }

            if (!emp.Status)
            {
                return Result.Failure<ClaimsIdentity>("Account has been deactivated. Contact your supervisor");
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, emp.Position.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, emp.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "OutOfOfficeCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return Result.Success(claimsIdentity);
        }

        public bool VerifyPassword(Employee emp, string password)
        {
            var hashedPassword = PasswordHelper.HashPassword(password, emp.Salt);
            return hashedPassword == emp.Password;
        }
    }
}
