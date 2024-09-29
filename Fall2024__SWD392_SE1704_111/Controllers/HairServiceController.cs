using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/hairservice")]
    [ApiController]
    public class HairServiceController : ControllerBase
    {
        private readonly IHairServiceService _serviceManagementService;

        public HairServiceController(IHairServiceService serviceManagementService)
        {
            _serviceManagementService = serviceManagementService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetListServices(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _serviceManagementService.GetListServicesAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("getServices/{id}")]
        public async Task<IActionResult> GetServiceById([FromRoute]int id)
        {
            var services = await _serviceManagementService.GetServiceByIdAsync(id);
            return Ok(services);
        }
    }
}
