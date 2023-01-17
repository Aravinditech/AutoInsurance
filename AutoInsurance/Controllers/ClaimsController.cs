using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AutoInsurance.Models;
using AutoInsurance.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.IO;

namespace AutoInsurance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ClaimsContext _context;

        public ClaimsController(ClaimsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimRegistration>>> GetClaims()
        {
            return await _context.Claims.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ClaimRegistration>> RegisterClaim(ClaimRegistration claimRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                claimRegistration.CreatedDate = DateTime.Now;
                claimRegistration.UpdatedDate = DateTime.Now;
                claimRegistration.CreatedBy = "Admin";
                claimRegistration.Status = "Initiated";
                _context.Claims.Add(claimRegistration);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            // file handling logic can be added here

            return claimRegistration;
        }

        [HttpPost]
        [Route("addDocument")]
        public async Task<ActionResult> ConvertToBase64(int id, IFormFile document)
        {
            try
            {
                byte[] documentBytes;
                using (var ms = new MemoryStream())
                {
                    document.CopyTo(ms);
                    documentBytes = ms.ToArray();
                }
                var documentBase64 = Convert.ToBase64String(documentBytes);
                ClaimRegistration claim = _context.Claims.FirstOrDefault(e => e.Id == id);
                if (claim == null)
                {
                    return NotFound();
                }
                else
                {
                    claim.Documents = documentBase64;
                    claim.DocumentName = document.FileName;
                    _context.Entry(claim).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return Ok(new { id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateStatus")]
        public async Task<ActionResult> UpdateStatus(int id, string status)
        {
            ClaimRegistration claim = _context.Claims.FirstOrDefault(e => e.Id == id);
            if (claim == null)
            {
                return NotFound();
            }
            else
            {
                claim.Status = status;
                _context.Entry(claim).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return Ok(new { id = id });
        }

        [HttpPost]
        [Route("returnFile")]
        public IActionResult ReturnFile(int id)
        {
            try
            {
                ClaimRegistration claim = _context.Claims.FirstOrDefault(e => e.Id == id);
                if (claim == null || claim.Documents == null)
                {
                    return NotFound();
                }
                byte[] fileBytes = Convert.FromBase64String(claim.Documents);
                var file = File(fileBytes, "application/octet-stream", claim.DocumentName);
                return file;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClaim(int id, ClaimRegistration claimRegistration)
        {
            if (id != claimRegistration.Id)
            {
                return BadRequest();
            }

            _context.Entry(claimRegistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ClaimExists(int id)
        {
            return _context.Claims.Any(e => e.Id == id);
        }
    }

}
