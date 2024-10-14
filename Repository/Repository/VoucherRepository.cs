using BusinessObject;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.VoucherEnum;

namespace Repository.Repository
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository() { }

        public VoucherRepository(HairSalonBookingContext context) => _context = context;

        public async Task<List<Voucher>> GetVocherByStatus(VoucherStatusEnum voucherStatus)
        {
            var listVoucher = await _context.Vouchers
        .Where(v => v.Status.HasValue && v.Status.Value == voucherStatus)
        .ToListAsync();

            return listVoucher;
        }

        public async Task<Voucher> GetVoucherById(int voucherId)
        {
            return await GetByIdAsync(voucherId);
        }
    }
}
