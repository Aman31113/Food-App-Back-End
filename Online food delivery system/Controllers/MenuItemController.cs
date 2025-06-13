using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class MenuItemController : ControllerBase
    {
        private readonly MenuItemService _menuItemService;

        public MenuItemController(MenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchMenuItem([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Menu item name cannot be empty");

            var menuItems = await _menuItemService.GetMenuItemsByNameAsync(name);
            if (!menuItems.Any())
                return NotFound("No menu items found with the specified name");

            return Ok(menuItems);
        }

        // GET: api/MenuItem
        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var menuItems = await _menuItemService.GetAllMenuItemsAsync();
            return Ok(menuItems);
        }

        // GET: api/MenuItem/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound("Menu item not found");
            return Ok(menuItem);
        }

        //// POST: api/MenuItem
        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuItemDTO menuItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                Price = menuItemDto.Price,
                RestaurantID = menuItemDto.RestaurantID,
                ImageUrl = menuItemDto.ImageUrl,
                Rating = null
            };

            await _menuItemService.AddMenuItemAsync(menuItem);
            return CreatedAtAction(nameof(GetMenuItemById), new { id = menuItem.ItemID }, menuItem);
        }


        //// PUT: api/MenuItem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem menuItem)
        {
            if (id != menuItem.ItemID)
                return BadRequest("Menu item ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingMenuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (existingMenuItem == null)
                return NotFound("Menu item not found");

            await _menuItemService.UpdateMenuItemAsync(menuItem);
            return NoContent();
        }
        [HttpPatch("{id}")]
        //[Authorize(Roles = "admin, agent")]
        public async Task<IActionResult> UpdatePhoneAddr(int id, [FromBody] MenuItemDTO upd)
        {
            var existing = await _menuItemService.GetMenuItemByIdAsync(id);
            if (existing == null)
                return NotFound("Menu Item  not found");
            existing.Name = upd.Name;
            existing.Description = upd.Description;
            existing.Price = upd.Price;
            //existing.Rating = upd.Rating;
            existing.ImageUrl = upd.ImageUrl;

            await _menuItemService.UpdateMenuItemAsync(existing);
            return NoContent();
        }

        //// DELETE: api/MenuItem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound("Menu item not found");

            await _menuItemService.DeleteMenuItemAsync(id);
            return NoContent();
        }
    }
}
