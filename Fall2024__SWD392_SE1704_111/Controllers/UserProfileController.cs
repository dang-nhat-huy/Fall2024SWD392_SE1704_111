using BusinessObject;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;
using System.IdentityModel.Tokens.Jwt;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/userprofile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllUserProfile()
        {
            var result = await _userProfileService.GetAllUserProfile();
            return Ok(result);
        }

        // GET: api/userprofile/current
        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var response = await _userProfileService.GetCurrentUserProfile();

            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response); 
            }

            return Ok(response); 
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var userProfile = await _userProfileService.GetUserProfileByIdAsync(id);

            if (userProfile == null)
            {
                return NotFound(new { message = $"User profile with ID {id} not found." });
            }

            return Ok(userProfile);
        }

        [Authorize]
        [HttpPost("updateCurrentProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Kiểm tra xem request có hợp lệ không
            if (request == null)
            {
                return BadRequest(new ResponseDTO(Const.FAIL_READ_CODE, "Invalid request."));
            }

            // Gọi service để cập nhật thông tin người dùng
            var response = await _userProfileService.UpdateUserProfileAsync(request);

            // Kiểm tra kết quả và trả về phản hồi phù hợp
            if (response.Status != Const.SUCCESS_READ_CODE)
            {
                return BadRequest(response); // Trả về mã lỗi 400 với thông báo lỗi từ ResponseDTO
            }

            return Ok(response); 
        }
        
    }
}