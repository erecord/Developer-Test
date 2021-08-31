using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Auth;
using StoreBackend.Models;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public BasketController(StoreDbContext context)
        {
            _context = context;
        }

        // GET: api/Basket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Basket>>> GetBasket()
        {
            return await _context.Basket.Include(p => p.Products).ToListAsync();
        }

        // GET: api/Basket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> GetBasket(int id)
        {
            var basket = await _context.Basket.FindAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            return basket;
        }

        // PUT: api/Basket/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasket(int id, Basket basket)
        {
            if (id != basket.Id)
            {
                return BadRequest();
            }


            basket.Products.ForEach(p => _context.Entry(p).State = EntityState.Modified);
            _context.Entry(basket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(basket);
        }

        // POST: api/Basket
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Basket>> PostBasket(Basket basket)
        {
            _context.Basket.Add(basket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        // DELETE: api/Basket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basket = await _context.Basket.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }

            _context.Basket.Remove(basket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasketExists(int id)
        {
            return _context.Basket.Any(e => e.Id == id);
        }
    }
}
