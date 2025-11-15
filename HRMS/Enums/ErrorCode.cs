// ============================================
// File Path: Models/Enums/ErrorCode.cs
// ============================================

namespace HRMS.Models.Enums
{
    public enum ErrorCode
    {
        // Success
        NoError = 0,

        // General Errors (1-99)
        GeneralError = 1,
        OperationFailed = 2,
        InvalidInput = 3,

        // HTTP Status Code Errors (400-599)
        ValidationError = 400,      // Bad Request
        Unauthorized = 401,         // Not authenticated
        Forbidden = 403,            // Not authorized (no permission)
        NotFound = 404,             // Resource not found
        Conflict = 409,             // Conflict (e.g., duplicate)
        ServerError = 500,          // Internal server error

        // Business Logic Errors (1000+)
        EmailAlreadyExists = 1001,
        InvalidCredentials = 1002,
        UserNotFound = 1003,
        EmployeeNotFound = 1004,
        DepartmentNotFound = 1005,
        JobTitleNotFound = 1006,
        LeaveTypeNotFound = 1007,

        // Employee Related (1100+)
        EmployeeInactive = 1101,
        EmployeeAlreadyExists = 1102,
        CannotDeleteEmployee = 1103,

        // Leave Related (1200+)
        InsufficientLeaveBalance = 1201,
        InvalidLeaveDates = 1202,
        LeaveAlreadyApproved = 1203,
        LeaveAlreadyRejected = 1204,

        // Authentication/Authorization (1300+)
        InvalidToken = 1301,
        TokenExpired = 1302,
        InsufficientPermissions = 1303
    }
}
