using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;


namespace Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> GetAll()
        {
            try
            {
                var listUser = await _unitOfWork.UserRepository.GetAllAsync();

                if (listUser == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "No users found.");
                }
                else
                {
                    return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, listUser);
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> Login(LoginRequestDTO request)
        {
            try
            {
                var account = _unitOfWork.UserRepository.GetAll()
                                .FirstOrDefault(x => x.UserName!.ToLower() == request.userName.ToLower()
                                && x.Password == request.password);

                if (account == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Invalid credentials.");
                }

                var jwt = GenerateToken(account);
                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, jwt);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private string GenerateToken(User account)
        {
            var tokenSecret = _config["Jwt:Key"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenSecret);

            var claims = new List<Claim>
        {
            new(ClaimTypes.Role, account.Status!.ToString()!.Trim()),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResponseDTO> Register(RegisterRequestDTO request)
        {
            try
            {
                if (await _unitOfWork.UserRepository.ExistsByNameAsync(request.userName))
                {
                    return new ResponseDTO(Const.FAIL_CREATE_CODE, "The username is already taken. Please choose a different username.");
                }

                //AutoMapper from RegisterRequestDTO => User
                var user = _mapper.Map<User>(request);

                user.CreateDate = DateTime.UtcNow; 
                user.Status = UserStatus.Active; 
                user.Role = UserRole.Customer; 

                await _unitOfWork.UserRepository.CreateAsync(user);
                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "User registered successfully", user);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
