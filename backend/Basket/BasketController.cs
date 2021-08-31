using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.DbContexts;
using StoreBackend.Facade;
using StoreBackend.Models;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly BasketControllerFacade _controllerFacade;

        public BasketController(StoreDbContext context, BasketControllerFacade basketControllerFacade)
        {
            _context = context;
            _controllerFacade = basketControllerFacade;
        }

        // GET: api/Basket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Basket>>> GetBasket()
        {
            return await _context.Basket.ToListAsync();
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

            _controllerFacade.QueryProductsInBasketCommand.Execute(id);
            basket.Products = _controllerFacade.QueryProductsInBasketCommand.ProductsInBasket.ToList();

            return basket;
        }

        // PUT: api/Basket/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasket(int id, UpdateBasketDTO updateBasketDTO)
        {
            if (id != updateBasketDTO.Basket.Id)
            {
                return BadRequest();
            }

            _controllerFacade.QueryProductIdsInBasketCommand.Execute(id);
            var productIdsInBasket = _controllerFacade.QueryProductIdsInBasketCommand.ProductIdsInBasket.ToList();

            var removedProductIds = productIdsInBasket.Except(updateBasketDTO.ProductIds);
            var newProductIds = updateBasketDTO.ProductIds.Except(productIdsInBasket);

            newProductIds.ToList().ForEach(productId =>
            {
                _context.BasketProducts.Add(new BasketProduct { BasketId = id, ProductId = productId });
            });

            var removedBasketProducts = _context.BasketProducts.Where(bp => removedProductIds.Contains(bp.ProductId)).ToList();
            _context.BasketProducts.RemoveRange(removedBasketProducts);

            _context.Entry(updateBasketDTO.Basket).State = EntityState.Modified;

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

            return Ok(updateBasketDTO.Basket);
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
