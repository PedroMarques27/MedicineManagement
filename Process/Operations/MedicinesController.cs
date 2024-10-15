using Microsoft.AspNetCore.Mvc;
using Process.DTOs;
using Process.DTOs.Entities;
using Process.Providers;
using System.Threading.Tasks;

namespace Process.Operations
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineProvider _medicineProvider;

        public MedicinesController(IMedicineProvider medicineProvider)
        {
            _medicineProvider = medicineProvider;
        }

        // Get all medicines
        [HttpGet]
        public IActionResult GetAllMedicines()
        {
            var result = _medicineProvider.GetAll();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }

        // Get a medicine by name
        [HttpGet("{name}")]
        public async Task<IActionResult> GetMedicineByName(string name)
        {

            var result = await _medicineProvider.GetByNameAsync(name);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Error);
        }

        // Add a new medicine
        [HttpPost]
        public async Task<IActionResult> AddMedicine([FromBody] Medicine medicine)
        {

            var result = await _medicineProvider.AddMedicine(medicine);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetMedicineByName), new { name = medicine.Name }, result.Data);
            }
            return BadRequest(result.Error);
        }

        // Update an existing medicine
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateMedicine(string name, [FromBody] Medicine medicine)
        {

            var result = await _medicineProvider.UpdateMedicine(name, medicine);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Error);
        }

        // Delete a medicine by name
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteMedicine(string name)
        {
            var result = await _medicineProvider.DeleteMedicine(name);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Error);
        }
    }
}
