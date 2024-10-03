using BusinessObject.Model;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

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

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var userProfile = await _userProfileService.GetUserProfileByIdAsync(id);

            if (userProfile == null)
            {
                return NotFound(new { message = $"User profile with ID {id} not found." });
            }

            return Ok(userProfile);
        }
    }
}