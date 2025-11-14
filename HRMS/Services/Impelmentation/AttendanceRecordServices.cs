using HRMS.Interfaces; 
using HRMS.Interfaces.Services;
using HRMS.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HRMS.Services.Impelmentation
{
    public class AttendanceRecordServices : IAttendanceRecordServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceRecordServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<bool> CheckInAsync(int employeeId)
        {
            var employeeExists = await _unitOfWork.Employee.IsExistAsync(e => e.EmployeeID == employeeId && e.IsActive);
            if (!employeeExists) return false;

            var activeShifts = await _unitOfWork.AttendanceRecord
                .FindAllAsync(r => r.EmployeeID == employeeId && r.CheckOutTime == null);

            var currentShift = activeShifts.OrderByDescending(r => r.CheckInTime).FirstOrDefault();
            if (currentShift != null) return false;

            var now = DateTime.UtcNow;
            var record = new AttendanceRecord
            {
                EmployeeID = employeeId,
                CheckInTime = now,
                Date = now.Date
            };

            await _unitOfWork.AttendanceRecord.AddAsync(record);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckOutAsync(int employeeId)
        {
            var employeeExists = await _unitOfWork.Employee.IsExistAsync(e => e.EmployeeID == employeeId && e.IsActive);
            if (!employeeExists) return false;

            var activeShifts = await _unitOfWork.AttendanceRecord
                 .FindAllAsync(r => r.EmployeeID == employeeId && r.CheckOutTime == null);

            var currentShift = activeShifts.OrderByDescending(r => r.CheckInTime).FirstOrDefault();
            if (currentShift == null) return false;

            currentShift.CheckOutTime = DateTime.UtcNow;

            await _unitOfWork.AttendanceRecord.UpdateAsync(currentShift);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }


        public async Task<AttendanceRecord> AddAsync(AttendanceRecord record)
        {
            return await _unitOfWork.AttendanceRecord.AddAsync(record);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _unitOfWork.AttendanceRecord.GetByIdAsync(id);
            if (record == null) return false;

            await _unitOfWork.AttendanceRecord.DeleteAsync(record);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAllAsync()
        {
            var includes = new string[] { "Employee" };
            var records = await _unitOfWork.AttendanceRecord.FindAllAsync(criteria: null, includes: includes);
            return records.OrderByDescending(r => r.CheckInTime);
        }

        public async Task<IEnumerable<AttendanceRecord>> GetByEmployeeIdAsync(int employeeId)
        {
            var includes = new string[] { "Employee" };
            var records = await _unitOfWork.AttendanceRecord
                .FindAllAsync(r => r.EmployeeID == employeeId, includes);
            return records.OrderByDescending(r => r.CheckInTime);
        }

        public async Task<AttendanceRecord?> GetByIdAsync(int id)
        {
            var includes = new string[] { "Employee" };
            return await _unitOfWork.AttendanceRecord.FindAsync(r => r.AttendanceID == id, includes);
        }

        public async Task<bool> UpdateAsync(AttendanceRecord record)
        {
            await _unitOfWork.AttendanceRecord.UpdateAsync(record);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}