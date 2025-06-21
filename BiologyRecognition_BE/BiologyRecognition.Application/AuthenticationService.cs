using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.UserAccount;
using BiologyRecognition.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserAccountRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService() => _userRepository ??= new UserAccountRepository();

        public AuthenticationService(IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository ??= new UserAccountRepository();
            _configuration = configuration;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AccountResponseDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                var account = await _userRepository.GetUserAccountByNameOrEmailAsync(loginDTO.UserNameOrEmail);
                if (account == null)
                {
                    throw new ArgumentException("Invalid username or email.");
                }
                var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, account.Password);
                if (!isPasswordValid)
                {
                    throw new ArgumentException("Invalid password.");
                }
                var accountResponse = _mapper.Map<AccountResponseDTO>(account);
                accountResponse.AccessToken = await GenerateToken(accountResponse);
                return accountResponse;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<RegisterDTO> Register(RegisterDTO registerDTO)
        {
            try
            {
                var existingUsername = await _userRepository.GetUserAccountByNameOrEmailAsync(registerDTO.UserName);
                var existingEmail = await _userRepository.GetByEmailAsync(registerDTO.Email);
                var existingEmployeeCode = await _userRepository.GetByEmployeeCodeAsync(registerDTO.EmployeeCode);
                if (existingUsername != null || existingEmail != null)
                {
                    throw new ArgumentException("Username or email already exists.");
                }
                if (existingEmployeeCode != null)
                {
                    throw new ArgumentException("Employee code already exists.");
                }
                var newAccount = _mapper.Map<UserAccount>(registerDTO);
                newAccount.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
                newAccount.IsActive = true; // Set default active status
                newAccount.CreatedBy = "System"; // Default created by system
                newAccount.ModifiedDate = DateTime.Now; // Set created date to now
                newAccount.ModifiedBy = "System";
                newAccount.RoleId = 2; // Default role registered là user
                await _userRepository.CreateAsync(newAccount);
                return registerDTO;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public Task<string> GenerateToken(AccountResponseDTO accountResponseDTO)
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");
            if (jwtConfig == null)
            {
                throw new Exception("JWT configuration not found.");
            }
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            var key = jwtConfig["Key"];
            var expiryIn = DateTime.Now.AddDays(Double.Parse(jwtConfig["ExpireDays"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", accountResponseDTO.UserAccountId.ToString()),
                    new Claim(ClaimTypes.Email, accountResponseDTO.Email),
                    new Claim(ClaimTypes.Role, accountResponseDTO.RoleId.ToString())
                }),
                Expires = expiryIn,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            return Task.FromResult(accessToken);
        }

        public async Task<AccountResponseDTO> LoginWithGoolgle(string email, string name)
        {
            var account = await _userRepository.GetUserAccountByNameOrEmailAsync(email);
            string accessToken = string.Empty;
            if (account == null)
            {
                // Tạo tài khoản mới nếu chưa tồn tại
                var newAccount = new UserAccount
                {
                    Email = email,
                    UserName = email,
                    FullName = name,
                    IsActive = true,
                    ModifiedDate = DateTime.Now,
                    Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Tạo mật khẩu ngẫu nhiên
                    CreatedBy = "Google",
                    ModifiedBy = "Google",
                    RoleId = 2 // Default role registered là user
                };
                await _userRepository.CreateAsync(newAccount);
                accessToken = await GenerateToken(_mapper.Map<AccountResponseDTO>(newAccount));
                var accountResponse = _mapper.Map<AccountResponseDTO>(newAccount);
                accountResponse.AccessToken = accessToken;
                return accountResponse;
            }
            else
            {
                // Nếu đã tồn tại, chỉ cần tạo token mới
                accessToken = await GenerateToken(_mapper.Map<AccountResponseDTO>(account));
                var accountResponse = _mapper.Map<AccountResponseDTO>(account);
                accountResponse.AccessToken = accessToken;
                return accountResponse;
            }
        }

        public async Task<AccountResponseDTO> GetCurrentUser()
        {
            try
            {
                var userEmail = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }
                var userAccount = await _userRepository.GetByEmailAsync(userEmail);
                if (userAccount == null)
                {
                    throw new ArgumentException("User account not found.");
                }
                return _mapper.Map<AccountResponseDTO>(userAccount);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
