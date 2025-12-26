using Platform.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.TimeAttendance
{
    public interface ITimeAttendanceService
    {
        Task<Platform.Data.DTOs.TimeAttendance> CheckInAsync(Platform.Data.DTOs.TimeAttendance attendance);
        Task<Platform.Data.DTOs.TimeAttendance> CheckOutAsync(Platform.Data.DTOs.TimeAttendance attendance);
        Task<List<Platform.Data.DTOs.TimeAttendance>> GetByEmployeeAsync(int employeeId);
        Task<List<Platform.Data.DTOs.TimeAttendance>> GetReportAsync(DateTime startDate, DateTime endDate, int? employeeId = null);
    }
}
