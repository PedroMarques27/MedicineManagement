using Microsoft.AspNetCore.Mvc;
using Process.DTOs;
using Process.DTOs.Entities;
using Process.Providers;

namespace Process.Operations
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersProvider _usersProvider;
        private readonly IPrescriptionProvider _prescriptionProvider;

        public UserController(IUsersProvider usersProvider, IPrescriptionProvider prescriptionProvider)
        {
            _usersProvider = usersProvider;
            _prescriptionProvider = prescriptionProvider;
        }

        // POST: api/user/add
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserInputDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            var response = await _usersProvider.AddUser(new Process.DTOs.Entities.User { Name = user.Name, Email = user.Email});
            if (response.Success)
            {
                return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, response.Data);
            }

            return BadRequest(response.Error);
        }

        // DELETE: api/user/delete/{email}
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var response = await _usersProvider.DeleteUser(email);
            if (response.Success)
            {
                return NoContent();
            }

            return NotFound(response.Error);
        }

        // GET: api/user/{email}
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var response = await _usersProvider.GetUserByEmail(email);
            if (response.Success)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Error);
        }

        // GET: api/user/all
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var response = _usersProvider.GetUsers();
            if (response.Success)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Error);
        }

        // PUT: api/user/update/{email}
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UserInputDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            var response = await _usersProvider.UpdateUser(email, new Process.DTOs.Entities.User { Name = user.Name, Email = user.Email });
            if (response.Success)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Error);
        }

        [HttpPost("{email}/Prescription")]
        public async Task<IActionResult> CreatePrescription(string email, [FromBody] ICollection<string> medicines)
        {
            if (medicines == null || medicines.Count == 0)
            {
                return BadRequest("List of medicines is required.");
            }

            var response = await _prescriptionProvider.CreatePrescriptionAsync(email, medicines);
            if (response.Success)
            {
                return CreatedAtAction(nameof(GetPrescriptionByEmail), new { email }, response.Data);
            }

            return BadRequest(response.Error);
        }

        [HttpGet("{email}/Prescription")]
        public async Task<IActionResult> GetPrescriptionByEmail(string email)
        {
            var response = await _prescriptionProvider.GetPrescriptionByEmailAsync(email);
            if (response.Success)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Error);
        }
    }
}
