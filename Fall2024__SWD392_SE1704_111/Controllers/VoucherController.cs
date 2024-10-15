using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace Fall2024__SWD392_SE1704_111.Controllers
{
    [Route("api/v1/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("listVoucher")]
        public async Task<IActionResult> GetListServices()
        {
            var result = await _voucherService.GetListVoucherAsync();
            return Ok(result);
        }
    }
}
