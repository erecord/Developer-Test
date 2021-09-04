using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.DTOs;
using StoreBackend.Extensions;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketControllerFacade _controllerFacade;

        public BasketController(
            IBasketRepository repository, IBasketControllerFacade basketControllerFacade
        )
        {
            _basketRepository = repository;
            _controllerFacade = basketControllerFacade;
        }

        // GET: api/Basket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketDTO>>> GetBasket()
            => Ok((await _basketRepository.GetAllAsync()).Select(b => b.ToDTO()));

        // GET: api/Basket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDTO>> GetBasket(int id)
        {
            var basket = await _basketRepository.ToPopulatedBasket(id);

            if (basket == null)
            {
                return NotFound();
            }

            basket.Products = (await _controllerFacade.QueryProductsInBasketCommand.Execute(id)).ToList();

            return basket.ToDTO();
        }

        // PUT: api/Basket/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{basketId}")]
        public async Task<IActionResult> PutBasket(int basketId, UpdateBasketDTO updateBasketDTO)
        {
            if (basketId != updateBasketDTO.Basket.Id)
            {
                return BadRequest();
            }

            var productIdsInBasket = (await _controllerFacade.QueryProductIdsInBasketCommand.Execute(basketId)).ToList();

            var removedProductIds = productIdsInBasket.Except(updateBasketDTO.ProductIds);
            var newProductIds = updateBasketDTO.ProductIds.Except(productIdsInBasket);

            await _controllerFacade.AddProductsToBasketCommand.Execute((basketId, newProductIds));
            await _controllerFacade.RemoveProductsFromBasketCommand.Execute((basketId, removedProductIds));

            var entityFound = await _basketRepository.UpdateAsync(updateBasketDTO.Basket);

            if (!entityFound)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Basket
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Basket>> PostBasket(Basket basket)
        {
            await _basketRepository.CreateAsync(basket);

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        // DELETE: api/Basket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basket = await _basketRepository.GetOneAsync(id);
            if (basket == null)
            {
                return NotFound();
            }

            await _basketRepository.DeleteAsync(basket);

            return NoContent();
        }

    }
}
