using BusinessObject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;
using System.IdentityModel.Tokens.Jwt;

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

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> CheckToken()
        //{
        //    if (!Request.Headers.TryGetValue("Authorization", out var token))
        //    {
        //        return StatusCode(404, "Cannot find user");
        //    }
        //    token = token.ToString().Split()[1];
        //    var currentUser = await _userService.GetUserInToken(token);
        //    if (currentUser == null)
        //    {
        //        return StatusCode(404, "Cannot find user");
        //    }
        //    // Here goes your token validation logic
        //    if (string.IsNullOrWhiteSpace(token))
        //    {
        //        return BadRequest("Authorization header is missing or invalid.");
        //    }
        //    // Decode the JWT token
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(token);

        //    // Check if the token is expired
        //    if (jwtToken.ValidTo < DateTime.UtcNow)
        //    {
        //        return BadRequestException("Token has expired.");
        //    }

        //    string email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        //    var user = await _userService.GetUserByUserName(email);
        //    var customer = await _userService.GetCustomerById(currentUser.CustomerId);
        //    if (user.Data == null)
        //    {
        //        return BadRequest("username is in valid");
        //    }

        //    // If token is valid, return success response
        //    return Ok(ApiResult<CheckTokenResponse>.Succeed(new CheckTokenResponse
        //    {
        //        User = user.Data,
        //        Customer = customer
        //    }));
        //}
    }
}