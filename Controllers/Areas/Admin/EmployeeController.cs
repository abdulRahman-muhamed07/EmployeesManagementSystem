using EmployeeManagement.Services;
using EmployeesManagementSystem.DTOs.Request;
using EmployeesManagementSystem.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        // GET: api/employee
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            return Ok(res);
        }

        // GET: api/employee/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null) return NotFound(new { success = false, message = "Employee not found" });
            return Ok(res);
        }

        // POST: api/employee
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            var res = await _service.CreateAsync(dto);
            return Ok(res);
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var res = await _service.UpdateAsync(id, dto);
            return Ok(res);
        }

        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _service.DeleteAsync(id);
            return Ok(res);
        }
    }
}
