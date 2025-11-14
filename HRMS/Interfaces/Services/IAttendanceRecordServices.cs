using HRMS.Models;

namespace HRMS.Interfaces.Services
{
    public interface IAttendanceRecordServices
    {
            Task<IEnumerable<AttendanceRecord>> GetAllAsync();
            Task<AttendanceRecord?> GetByIdAsync(int id);
            Task<IEnumerable<AttendanceRecord>> GetByEmployeeIdAsync(int employeeId);
            Task<AttendanceRecord> AddAsync(AttendanceRecord record);
            Task<bool> UpdateAsync(AttendanceRecord record);
            Task<bool> DeleteAsync(int id);
            Task<bool> CheckInAsync(int employeeId);
            Task<bool> CheckOutAsync(int employeeId);

    }
}
