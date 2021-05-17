using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseController
    {

        private readonly northwindContext _context;
        public AccountController(northwindContext context)
        {
            this._context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<Customer>> Register(RegisterDto registerDto)
        {
            if(await CustomerExists(registerDto.ContactName)) return BadRequest("ContactName already in the records");

            registerDto.CustomerId = await GenerateCustomerId();

            var customer = new Customer
            {
                CustomerId = registerDto.CustomerId,
                ContactName = registerDto.ContactName,
                CompanyName = registerDto.CompanyName,
                Address = registerDto.Address,
                City = registerDto.City,
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();


            return customer;

        }

        [HttpPost("login")]
        public async Task<ActionResult<Customer>> Login(LoginDto loginDto)
        {
            var user = await _context.Customers.SingleOrDefaultAsync(x => x.CustomerId == loginDto.CustomerId);

            if(user == null) return Unauthorized("Invalid Customer");

           return user;
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(LoginDto loginDto)
        {
            var user = await _context.Customers.SingleOrDefaultAsync(x => x.CustomerId == loginDto.CustomerId);

            if(user == null) return Unauthorized("Invalid Customer");

            _context.Customers.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

    
        private async Task<bool> CustomerExists(string contactName)
        {
            return await _context.Customers.AnyAsync(x => x.ContactName.ToLower() == contactName.ToLower());
        }

        private async Task<bool> CustomerIdExists(string customerId)
        {
            return await _context.Customers.AnyAsync(x => x.CustomerId.ToUpper() == customerId.ToUpper());
        }

        private async Task<string> GenerateCustomerId()
        {
            char[] letters = "qwertyuiopñljkhgfdsazxcvbnm".ToCharArray();
            Random r = new Random();
            string randomString;
            do
            {
                randomString = "";
                for (int i = 0; i < 5; i++)
                {
                    randomString += letters[r.Next(0,25)].ToString();
                }
                
            } while (await CustomerIdExists(randomString));

            return randomString.ToUpper();
        }
    }
}