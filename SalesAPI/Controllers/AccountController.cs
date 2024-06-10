using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SalesAPI.Interfaces;
using SalesAPI.Models;

namespace SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;

        public AuthenticationController(IConfiguration configuration, IAccountRepository accountRepository)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountDTO request)
        {
            if (_accountRepository.FindByUsername(request.Username) != null)
            {
                return BadRequest("Username already taken");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var account = new Account
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _accountRepository.Register(account);
            await _accountRepository.SaveAsync();

            return Ok("Registration successful. Please log in.");
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AccountDTO request)
        {
            var account = _accountRepository.FindByUsername(request.Username);
            if (account == null)
                return BadRequest("Account not found");

            if (!VerifyPasswordHash(request.Password, account.PasswordHash, account.PasswordSalt))
                return BadRequest("Wrong password");

            var token = CreateToken(account);
            account.RefreshToken = token;

            _accountRepository.Update(account.Username, account);
            await _accountRepository.SaveAsync();

            return Ok(token);
        }


        private string CreateToken(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }


        [HttpPut("update/{username}")]
        public async Task<IActionResult> Update(string username, AccountDTO accountDTO)
        {
            var account = _accountRepository.FindByUsername(username);
            if (account == null)
            {
                return NotFound("Account not found");
            }

            CreatePasswordHash(accountDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            account.Username = accountDTO.Username;
            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;

            _accountRepository.Update(username, account);
            await _accountRepository.SaveAsync();

            return Ok("Success");
        }

        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var account = _accountRepository.FindByUsername(username);
            if (account == null)
            {
                return NotFound("Account not found");
            }
            _accountRepository.Delete(account.AccountNumber);
            await _accountRepository.SaveAsync();
            return Ok("Success");
        }
    }
}
