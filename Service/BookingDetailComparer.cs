using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BookingDetailComparer : IEqualityComparer<BookingDetail>
    {
        public bool Equals(BookingDetail x, BookingDetail y)
        {
            if (x == null || y == null) return false;

            // Kiểm tra xem ServiceId và ScheduleId có giống nhau không
            return x.ServiceId == y.ServiceId && x.ScheduleId == y.ScheduleId;
        }

        public int GetHashCode(BookingDetail obj)
        {
            // Tạo mã băm cho ServiceId và ScheduleId
            return HashCode.Combine(obj.ServiceId, obj.ScheduleId);
        }
    }
    public class ServiceComparer : IEqualityComparer<HairService>
    {
        public bool Equals(HairService x, HairService y)
        {
            // So sánh ServiceId để xác định dịch vụ trùng lặp
            return x.ServiceId == y.ServiceId;
        }

        public int GetHashCode(HairService obj)
        {
            return obj.ServiceId.GetHashCode();
        }
    }

    public class ScheduleComparer : IEqualityComparer<Schedule>
    {
        public bool Equals(Schedule x, Schedule y)
        {
            // So sánh ScheduleId để xác định lịch trình trùng lặp
            return x.ScheduleId == y.ScheduleId;
        }

        public int GetHashCode(Schedule obj)
        {
            return obj.ScheduleId.GetHashCode();
        }
    }
}
