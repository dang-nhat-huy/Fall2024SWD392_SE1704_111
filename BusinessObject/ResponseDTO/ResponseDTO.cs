using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseDTO
{
    public class ResponseDTO
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public ResponseDTO(int status, string? message, object? data = null)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public class PagedResult<T>
        {
            public List<T> Items { get; set; }
            public int TotalCount { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }

            public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        }
    }

    public class ServicesDTO
    {
        public string? ImageLink { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public TimeSpan? EstimateTime { get; set; }
    }

    public class ScheduleUserDTO
    {
        public string UserName { get; set; } = null!;
        public string? Phone { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}


