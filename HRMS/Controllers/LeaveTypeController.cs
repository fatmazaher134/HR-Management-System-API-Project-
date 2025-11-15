using AutoMapper;
using HRMS.DTOs.LeaveType;
using HRMS.Interfaces.Services;
using HRMS.Models;
using HRMS.Models.Enums;
using HRMS.ViewModels;
using HRMS.ViewModels.LeaveType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeServices _leaveTypeServices;
        private readonly IMapper _mapper;

        public LeaveTypeController(ILeaveTypeServices leaveTypeServices, IMapper mapper)
        {
            _leaveTypeServices = leaveTypeServices;
            _mapper = mapper;
        }

        // GET: api/LeaveType
        [HttpGet]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<ResponseViewModel<IEnumerable<LeaveTypeDTO>>>> GetAll()
        {
            try
            {
                // Service returns DTOs directly
                var dtos = await _leaveTypeServices.GetAllAsync();

                return Ok(ResponseViewModel<IEnumerable<LeaveTypeDTO>>.Success(dtos, "Leave types retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<IEnumerable<LeaveTypeDTO>>.Failure(
                    "An error occurred while retrieving leave types",
                    ErrorCode.ServerError));
            }
        }

        // GET: api/LeaveType/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<ActionResult<ResponseViewModel<LeaveTypeDTO>>> GetById(int id)
        {
            try
            {
                // Service returns DTO directly
                var dto = await _leaveTypeServices.GetByIdAsync(id);

                if (dto == null)
                {
                    return NotFound(ResponseViewModel<LeaveTypeDTO>.NotFound("Leave type not found"));
                }

                return Ok(ResponseViewModel<LeaveTypeDTO>.Success(dto, "Leave type retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<LeaveTypeDTO>.Failure(
                    "An error occurred while retrieving leave type",
                    ErrorCode.ServerError));
            }
        }

        // POST: api/LeaveType
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseViewModel<LeaveTypeDTO>>> Create([FromBody] LeaveTypeFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseViewModel<LeaveTypeDTO>.ValidationError("Invalid data"));
                }

                // Map ViewModel to DTO
                var dto = _mapper.Map<CreateLeaveTypeDTO>(model);

                // Service handles DTO to Model mapping and returns DTO
                var resultDto = await _leaveTypeServices.AddAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = resultDto.LeaveTypeID },
                    ResponseViewModel<LeaveTypeDTO>.Success(resultDto, "Leave type created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<LeaveTypeDTO>.Failure(
                    "An error occurred while creating the leave type",
                    ErrorCode.ServerError));
            }
        }

        // PUT: api/LeaveType/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseViewModel<string>>> Update(int id, [FromBody] LeaveTypeFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseViewModel<string>.ValidationError("Invalid data"));
                }

                var existingDto = await _leaveTypeServices.GetByIdAsync(id);
                if (existingDto == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("Leave type not found"));
                }

                // Map ViewModel to DTO
                var dto = _mapper.Map<UpdateLeaveTypeDTO>(model);
                dto.LeaveTypeID = id;

                // Service handles DTO to Model mapping
                var result = await _leaveTypeServices.UpdateAsync(dto);

                if (!result)
                {
                    return BadRequest(ResponseViewModel<string>.Failure(
                        "Failed to update leave type",
                        ErrorCode.OperationFailed));
                }

                return Ok(ResponseViewModel<string>.Success("Leave type updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<string>.Failure(
                    "An error occurred while updating the leave type",
                    ErrorCode.ServerError));
            }
        }

        // DELETE: api/LeaveType/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseViewModel<string>>> Delete(int id)
        {
            try
            {
                var leaveType = await _leaveTypeServices.GetByIdAsync(id);
                if (leaveType == null)
                {
                    return NotFound(ResponseViewModel<string>.NotFound("Leave type not found"));
                }

                var result = await _leaveTypeServices.DeleteAsync(id);

                if (!result)
                {
                    return BadRequest(ResponseViewModel<string>.Failure(
                        "Failed to delete leave type. It may be in use.",
                        ErrorCode.OperationFailed));
                }

                return Ok(ResponseViewModel<string>.Success("Leave type deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseViewModel<string>.Failure(
                    "An error occurred while deleting the leave type",
                    ErrorCode.ServerError));
            }
        }
    }
}