using Microsoft.AspNetCore.Mvc;
using Process.DTOs;
using Process.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Process.Operations
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionProvider _prescriptionProvider;

        public PrescriptionController(IPrescriptionProvider prescriptionProvider)
        {
            _prescriptionProvider = prescriptionProvider;
        }


        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(Guid id)
        {
            var response = await _prescriptionProvider.DeletePrescriptionByIdAsync(id);
            if (response.Success)
            {
                return NoContent();
            }

            return NotFound(response.Error);
        }

        
        [HttpGet]
        public IActionResult GetAllPrescriptions()
        {
            var response = _prescriptionProvider.GetAllPrescriptions();
            if (response.Success)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Error);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetPrescriptionById(Guid id)
        {
            var response = _prescriptionProvider.GetPrescriptionById(id);
            if (response.Success)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Error);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(Guid id, [FromBody] ICollection<string> medicines)
        {
            if (medicines == null || medicines.Count == 0)
            {
                return BadRequest("List of medicines is required.");
            }

            var response = await _prescriptionProvider.UpdatePrescriptionAsync(id, medicines);
            if (response.Success)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Error);
        }
    }
}
