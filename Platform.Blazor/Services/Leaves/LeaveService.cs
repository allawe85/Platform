using System.Net.Http.Json;
using Platform.Data.DTOs;

namespace Platform.Blazor.Services.Leaves
{
    public class LeaveService
    {
        private readonly HttpClient _httpClient;

        public LeaveService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Leave>> GetMyLeavesAndRequests(int employeeId)
        {
            return await _httpClient.GetFromJsonAsync<List<Leave>>($"api/Leaves/{employeeId}") ?? new List<Leave>();
        }

        public async Task<List<Leave>> GetPendingLeaves()
        {
            return await _httpClient.GetFromJsonAsync<List<Leave>>("api/Leaves/pending") ?? new List<Leave>();
        }

        public async Task<HttpResponseMessage> SubmitLeaveRequest(Leave leave)
        {
            return await _httpClient.PostAsJsonAsync("api/Leaves", leave);
        }

        public async Task<HttpResponseMessage> ApproveLeave(int leaveId)
        {
            return await _httpClient.PutAsync($"api/Leaves/{leaveId}/approve", null);
        }

        public async Task<HttpResponseMessage> RejectLeave(int leaveId)
        {
            return await _httpClient.PutAsync($"api/Leaves/{leaveId}/reject", null);
        }

        public async Task<List<LeaveBalance>> GetMyBalances(int employeeId)
        {
             return await _httpClient.GetFromJsonAsync<List<LeaveBalance>>($"api/LeaveBalance/{employeeId}") ?? new List<LeaveBalance>();
        }

        // Lookups
        public async Task<List<LeaveType>> GetLeaveTypes()
        {
             return await _httpClient.GetFromJsonAsync<List<LeaveType>>("api/LeaveTypes") ?? new List<LeaveType>();
        }
        
        public async Task<List<LeaveStatus>> GetLeaveStatuses()
        {
             return await _httpClient.GetFromJsonAsync<List<LeaveStatus>>("api/LeaveStatuses") ?? new List<LeaveStatus>();
        }
    }
}
