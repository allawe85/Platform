using Platform.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Platform.Blazor.Services.TimeAttendance
{
    public class TimeAttendanceService : ITimeAttendanceService
    {
        private readonly HttpClient _http;

        public TimeAttendanceService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Platform.Data.DTOs.TimeAttendance> CheckInAsync(Platform.Data.DTOs.TimeAttendance attendance)
        {
            var response = await _http.PostAsJsonAsync("api/TimeAttendance/checkin", attendance);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Platform.Data.DTOs.TimeAttendance>();
        }

        public async Task<Platform.Data.DTOs.TimeAttendance> CheckOutAsync(Platform.Data.DTOs.TimeAttendance attendance)
        {
            var response = await _http.PostAsJsonAsync("api/TimeAttendance/checkout", attendance);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Platform.Data.DTOs.TimeAttendance>();
        }

        public async Task<List<Platform.Data.DTOs.TimeAttendance>> GetByEmployeeAsync(int employeeId)
        {
            return await _http.GetFromJsonAsync<List<Platform.Data.DTOs.TimeAttendance>>($"api/TimeAttendance/employee/{employeeId}");
        }

        public async Task<List<Platform.Data.DTOs.TimeAttendance>> GetReportAsync(DateTime startDate, DateTime endDate, int? employeeId = null)
        {
            string url = $"api/TimeAttendance/report?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            if (employeeId.HasValue)
            {
                url += $"&employeeId={employeeId}";
            }
            return await _http.GetFromJsonAsync<List<Platform.Data.DTOs.TimeAttendance>>(url);
        }
    }
}
