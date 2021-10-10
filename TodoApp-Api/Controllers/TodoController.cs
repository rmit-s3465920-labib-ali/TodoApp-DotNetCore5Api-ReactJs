using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_RestApi.Data;
using ToDo_RestApi.Models;

namespace ToDo_RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TodoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var items = await _context.Items.ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(TodoItem data)
        {
            if (ModelState.IsValid)
            {
                await _context.Items.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { data.Id }, data);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, TodoItem item)
        {
            if(id != item.Id)
            {
                return BadRequest();
            }

            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            //Todo: Setup AutoMapper for this later.
            existItem.Title = item.Title;
            existItem.Description = item.Description;
            existItem.Done = item.Done;

            // Implement the changes on the database level
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            _context.Items.Remove(existItem);
            await _context.SaveChangesAsync();

            return Ok(existItem);
        }

    }
}
