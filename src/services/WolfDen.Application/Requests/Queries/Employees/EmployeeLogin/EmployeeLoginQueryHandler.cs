using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Domain.ConfigurationModel;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.EmployeeLogin
{
    public class EmployeeLoginQueryHandler(IOptionsMonitor<JwtKey> optionsMonitort, UserManager<User> userManager, WolfDenContext context) : IRequestHandler<EmployeeLoginQuery, LoginResponseDTO>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IOptionsMonitor<JwtKey> _optionsMonitor = optionsMonitort;
        private readonly WolfDenContext _context = context;


        public async Task<LoginResponseDTO> Handle(EmployeeLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.UserId == user.Id);
                var secretKey = _optionsMonitor.CurrentValue.Key;
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesString = string.Join(",", currentRoles);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("EmployeeId",employee.Id.ToString()),
                        new Claim(ClaimTypes.Role, rolesString),
                        new Claim("Email",employee.Email.ToString()),
                        new Claim("FirstName",employee.FirstName.ToString())

                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return new LoginResponseDTO { Token = token };
            }
            return new LoginResponseDTO { ErrorMessage = "Invalid UserName or Password" };

        }
    }
}
