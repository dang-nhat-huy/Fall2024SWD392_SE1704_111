using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.VoucherEnum;

namespace Repository.IRepository
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        Task<Voucher> GetVoucherById(int voucherId);

        Task<List<Voucher>> GetVocherByStatus(VoucherStatusEnum voucherStatus);
    }
}
