
using HRMS.Models;
<<<<<<< HEAD
=======
using Humanizer;
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e

namespace HRMS.Services.Impelmentation
{
    public class PayrollService : IPayrollService
    {
<<<<<<< HEAD
        private readonly IUnitOfWork _unitOfWork;

        public PayrollService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, string ErrorMessage)> GeneratePayrollAsync(int month, int year)
        {
            try
            {
                // 1. التحقق إذا تم إنشاء الرواتب لهذا الشهر من قبل
                bool alreadyExists = await _unitOfWork.Payslip.IsExistAsync(p => p.Month == month && p.Year == year);
=======

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PayrollService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }
        public async Task<(bool Success, string ErrorMessage)> GeneratePayrollAsync(GeneratePayrollDto dto)
        {
            try
            {
                // استخدم dto.Month و dto.Year
                bool alreadyExists = await _unitOfWork.Payslip.IsExistAsync(p => p.Month == dto.Month && p.Year == dto.Year);
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
                if (alreadyExists)
                {
                    return (false, "salaries for this month and year assigned before");
                }

                // 2. جلب كل الموظفين النشطين
                var employees = await _unitOfWork.Employee.FindAllAsync(e => e.IsActive);

                // 3. جلب كل مكونات الراتب (لإعادة استخدامها بدلاً من جلبها في كل لفة)
                var components = await _unitOfWork.SalaryComponent.GetAllAsync();
                var basicSalaryComponent = components.FirstOrDefault(c => c.ComponentName == "Basic Salary" && c.ComponentType == ComponentType.Allowance);

                if (basicSalaryComponent == null)
                {
                    return (false, "not found basic salary component");
                }

                var newPayslips = new List<Payslip>();

                // 4. المرور على كل موظف
                foreach (var employee in employees)
                {
                    var payslip = new Payslip
                    {
                        EmployeeID = employee.EmployeeID,
<<<<<<< HEAD
                        Month = month,
                        Year = year,
=======
                        Month = dto.Month,
                        Year = dto.Year,
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
                        GeneratedDate = DateTime.UtcNow
                    };

                    decimal grossSalary = 0;
                    decimal totalDeductions = 0;
                    var details = new List<PayslipDetail>();

                    // --- 5. حساب البدلات (Allowances) ---

                    // إضافة الراتب الأساسي
                    decimal basicSalaryAmount = employee.BasicSalary;
                    details.Add(new PayslipDetail
                    {
                        Payslip = payslip,
                        ComponentID = basicSalaryComponent.ComponentID,
                        Amount = basicSalaryAmount
                    });
                    grossSalary += basicSalaryAmount;


                    var AllowanceComp = components.FirstOrDefault(c => c.ComponentName == "Allowance");
                    if (AllowanceComp != null)
                    {
                        decimal Amount = CalculateAllowance(employee); // ميثود لحساب البدل
                        details.Add(new PayslipDetail
                        {
                            Payslip = payslip,
                            ComponentID = AllowanceComp.ComponentID,
                            Amount = Amount
                        });
                        grossSalary += Amount;
                    }

                    // --- 6. حساب الاستقطاعات (Deductions) ---

                    // مثال: حساب الضريبة (نفترض وجوده في المكونات)
                    var taxComp = components.FirstOrDefault(c => c.ComponentName == "Tax");
                    if (taxComp != null)
                    {
                        decimal taxAmount = CalculateTax(grossSalary); // ميثود لحساب الضريبة
                        details.Add(new PayslipDetail
                        {
                            Payslip = payslip,
                            ComponentID = taxComp.ComponentID,
                            Amount = taxAmount
                        });
                        totalDeductions += taxAmount;
                    }

                    // 7. الحساب النهائي
                    payslip.GrossSalary = grossSalary;
                    payslip.TotalDeductions = totalDeductions;
                    payslip.NetSalary = grossSalary - totalDeductions;
                    payslip.PayslipDetails = details;

                    newPayslips.Add(payslip);
                }

                // 8. إضافة كل قسائم الرواتب دفعة واحدة
                await _unitOfWork.Payslip.AddRangeAsync(newPayslips);

                await _unitOfWork.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        // --- ميثودات مساعدة لحسابات الراتب ---
        // (يجب أن تضع بها منطق الحساب الفعلي الخاص بشركتك)
        private decimal CalculateAllowance(Employee employee)
        {
            return employee.BasicSalary * 0.10m;
        }

        private decimal CalculateTax(decimal grossSalary)
        {
            return grossSalary * 0.05m;
        }



<<<<<<< HEAD
        public async Task<PayslipDetailsViewModel?> GetPayslipDetailsAsync(int payslipId)
        {
            Payslip payslip = await _unitOfWork.Payslip.GetPayslipWithDetailsAsync(payslipId);

            if (payslip == null) return null;
            return MapToDetailsViewModel(payslip);
        }

        public async Task<PayslipDetailsViewModel?> GetMyPayslipDetailsAsync(int payslipId, string applicationUserId)
        {
            Payslip payslip = await _unitOfWork.Payslip.GetMyPayslipWithDetailsAsync(payslipId, applicationUserId);

            if (payslip == null)
            {
                return null; // Not found, or does not belong to this user
            }

            return MapToDetailsViewModel(payslip);
        }


        public async Task<IEnumerable<PayslipViewModel>> GetMyPayslipsAsync(string applicationUserId)
=======
        

        public async Task<IEnumerable<PayslipSummaryDto>> GetAllAsync()
        {
            var payslips = await _unitOfWork.Payslip.FindAllAsync(includes: new[] { "Employee" });
            return _mapper.Map<IEnumerable<PayslipSummaryDto>>(payslips);
        }

        public async Task<IEnumerable<PayslipDto>> GetMyPayslipsAsync(string applicationUserId)
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        {
            var payslips = await _unitOfWork.Payslip.FindAllAsync(
                criteria: p => p.Employee.ApplicationUserId == applicationUserId,
                includes: new[] { "Employee" }
            );
<<<<<<< HEAD

            return payslips.Select(p => new PayslipViewModel
            {
                PayslipID = p.PayslipID,
                EmployeeName = p.Employee?.FirstName + " " + p.Employee?.LastName,
                Month = p.Month,
                Year = p.Year,
                NetSalary = p.NetSalary,
                PayDate = p.GeneratedDate
            });
=======
            return _mapper.Map<IEnumerable<PayslipDto>>(payslips);
        }

        public async Task<PayslipDetailsDto?> GetPayslipDetailsAsync(int payslipId)
        {
            Payslip payslip = await _unitOfWork.Payslip.GetPayslipWithDetailsAsync(payslipId); 
            if (payslip == null) return null;

            return _mapper.Map<PayslipDetailsDto>(payslip);
        }

        public async Task<PayslipDetailsDto?> GetMyPayslipDetailsAsync(int payslipId, string applicationUserId)
        {
            Payslip payslip = await _unitOfWork.Payslip.GetMyPayslipWithDetailsAsync(payslipId, applicationUserId); // (افترض أن هذه الميثود موجودة في الريبو)
            if (payslip == null) return null;

            return _mapper.Map<PayslipDetailsDto>(payslip);
        }


        public async Task DeletePayslip(int id)
        {
            Payslip payslip = await _unitOfWork.Payslip.GetByIdAsync(id);
            if (payslip == null)
            {
                throw new KeyNotFoundException("Payslip not found");
            }
            await _unitOfWork.Payslip.DeleteAsync(payslip);
            await _unitOfWork.SaveChangesAsync();
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
        }

        private PayslipDetailsViewModel MapToDetailsViewModel(Payslip payslip)
        {
            return new PayslipDetailsViewModel
            {
                PayslipID = payslip.PayslipID,
                Month = payslip.Month,
                Year = payslip.Year,
                GeneratedDate = payslip.GeneratedDate,
                GrossSalary = payslip.GrossSalary,
                TotalDeductions = payslip.TotalDeductions,
                NetSalary = payslip.NetSalary,
                EmployeeFullName = $"{payslip.Employee?.FirstName} {payslip.Employee?.LastName}",
                EmployeeEmail = payslip.Employee?.Email ?? "N/A",
                Details = payslip.PayslipDetails.Select(pd => new PayslipDetailItem
                {
                    ComponentName = pd.SalaryComponent?.ComponentName ?? "N/A",
                    ComponentType = pd.SalaryComponent?.ComponentType ?? ComponentType.Allowance,
                    Amount = pd.Amount
                }).ToList()
            };
        }

<<<<<<< HEAD
        public async Task DeletePayslip(int id)
        {
            Payslip payslip = await _unitOfWork.Payslip.GetByIdAsync(id);
            if (payslip == null) {
                throw new KeyNotFoundException("Payslip not found");
            }
            await _unitOfWork.Payslip.DeleteAsync(payslip);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<PayslipSummaryViewModel>> GetAllAsync()
        {
            var payslips = await _unitOfWork.Payslip.FindAllAsync(includes: ["Employee"]);
            return payslips.Select(p => new PayslipSummaryViewModel
            {
                PayslipID = p.PayslipID,
                EmployeeFullName = $"{p.Employee?.FirstName} {p.Employee?.LastName}",
                Month = p.Month,
                Year = p.Year,
                NetSalary = p.NetSalary,
                GeneratedDate = p.GeneratedDate
            });
        }
=======
        
>>>>>>> ff52cd07578c21d0f60fe695abe4524e021a4e1e
    }
}
