﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAfternoon.Dtos;
using WebApiAfternoon.Repositories.Abstract;

namespace WebApiAfternoon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IConfiguration _configuration;

        public AccountController(IStudentRepository studentRepository, IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _configuration = configuration;
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<string>> Post([FromBody]SignInDto dto)
        {
            var student = await _studentRepository.Get(s => s.Username == dto.Username && s.Password == dto.Password);
            if (student != null)
            {
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject=new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name,dto.Username),new Claim("Fullname",student.Fullname)}),
                    Expires=DateTime.UtcNow.AddHours(1),
                    SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenData = tokenHandler.WriteToken(token);
                return tokenData;
            }
            return "";
        }   
    }
}
