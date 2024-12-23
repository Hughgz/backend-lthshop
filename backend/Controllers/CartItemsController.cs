using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Repositories.EntitiesRepo;
using backend.Dtos;  // Assuming DTOs are in this namespace
using AutoMapper;
using backend.Entities;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly CartItemRepo _cartItemRepo;
        private readonly IMapper _mapper;

        public CartItemsController(CartItemRepo cartItemRepo, IMapper mapper)
        {
            _cartItemRepo = cartItemRepo;
            _mapper = mapper;
        }

        // GET: api/CartItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemReadDto>>> GetCartItems()
        {
            var cartItems = await _cartItemRepo.GetAllAsync();
            var cartItemsDto = _mapper.Map<IEnumerable<CartItemReadDto>>(cartItems);
            return Ok(cartItemsDto);
        }

        // GET: api/CartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CartItemReadDto>>> GetCartItem(int id)
        {
            var cartItems = await _cartItemRepo.GetCardItemByCustomerId(id);

            if (cartItems == null || !cartItems.Any())
            {
                return Ok(null);
            }

            var cartItemsDto = _mapper.Map<IEnumerable<CartItemReadDto>>(cartItems);
            return Ok(cartItemsDto);
        }

        // PUT: api/CartItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItemUpdateDto cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest("Cart item is required.");
            }

            if (id != cartItem.CartItemID)
            {
                return BadRequest("Cart item ID does not match the ID in the request.");
            }

            // Check if the cart item exists by ID
            var existingCartItem = await _cartItemRepo.GetByIdAsync(id);

            if (existingCartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Map the updates from DTO to the existing entity
            _mapper.Map(cartItem, existingCartItem);

            // Update the cart item in the database
            var updatedCartItem = await _cartItemRepo.UpdateAsync(existingCartItem);

            if (updatedCartItem == null)
            {
                return BadRequest("Failed to update the cart item.");
            }

            return Ok(_mapper.Map<CartItemReadDto>(updatedCartItem));
        }



        // POST: api/CartItems
        [HttpPost]
        public async Task<ActionResult<CartItemReadDto>> PostCartItem(CartItemCreateDto cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest("Cart item is required.");
            }

            // Check if item exists
            var existingCartItem = await _cartItemRepo.GetCartItemByCustomerIdAndProductIdAsync(
                cartItem.CustomerID, cartItem.ProductSizeID);

            if (existingCartItem != null)
            {
                // Increase quantity if item exists
                existingCartItem.Quantity += cartItem.Quantity;
                await _cartItemRepo.UpdateAsync(existingCartItem);
                return NoContent();
            }

            // Add new item if not exists
            var newCartItem = _mapper.Map<CartItem>(cartItem);
            var result = await _cartItemRepo.AddAsync(newCartItem);

            var cartItemDto = _mapper.Map<CartItemReadDto>(result);
            return CreatedAtAction("GetCartItem", new { id = cartItemDto.CartItemID }, cartItemDto);
        }


        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var result = await _cartItemRepo.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/CartItems/removeCart/5
        [HttpDelete("removeCart/{customerId}")]
        public async Task<IActionResult> RemoveCartByCustomerId(int customerId)
        {
            var cartItems = await _cartItemRepo.GetCardItemByCustomerId(customerId);
            foreach (var cartItem in cartItems)
            {
                await _cartItemRepo.DeleteAsync(cartItem.CartItemID);
            }
            return NoContent();
        }

        [HttpPost("updateSession")]
        public async Task<IActionResult> UpdateCartItemFromSession(List<CartItemCreateDto> cartItemsFromSession)
        {
            if (cartItemsFromSession == null || !cartItemsFromSession.Any())
            {
                return BadRequest("No cart items provided.");
            }

            foreach (var sessionItem in cartItemsFromSession)
            {
                // Check if the cart item exists in the database
                var existingCartItem = await _cartItemRepo.GetCartItemByCustomerIdAndProductIdAsync(
                    sessionItem.CustomerID, sessionItem.ProductSizeID);

                if (existingCartItem == null)
                {
                    // If not exists, add a new item
                    var newCartItem = _mapper.Map<CartItem>(sessionItem);
                    await _cartItemRepo.AddAsync(newCartItem);
                }
                else
                {
                    // If exists, increase the quantity
                    existingCartItem.Quantity += sessionItem.Quantity;
                    await _cartItemRepo.UpdateAsync(existingCartItem);
                }
            }

            return NoContent();
        }

    }
}
