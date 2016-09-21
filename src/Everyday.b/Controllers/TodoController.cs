using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Models;
using Everyday.b.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Everyday.b.Controllers
{
    [Route("api/[controller]/[Action]")]
    [Authorize]
    public class TodoController : Controller
    {
        private readonly TodoManager _todoManager;

        public TodoController(TodoManager todoManager)
        {
            _todoManager = todoManager;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok("ok");
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] TodoItem item)
        {
            var result = await _todoManager.AddItemAsync(User.Identity.Name, item);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Today()
        {
            var result = await _todoManager.GetTodayItems(User.Identity.Name, DateTime.Today);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> All()
        {
            var result = await _todoManager.GetAllItems(User.Identity.Name);
            return Ok(result);
        }

        [HttpPost("{itemId}")]
        public async Task<ActionResult> Check(string itemId,[FromForm] string comment)
        {
            var item = await _todoManager.FindById(itemId);
            if (item == null)
                return NotFound();
            var check = item.Checks.FirstOrDefault(c => c.CheckedDate.Date == DateTime.Today);
            if (check == null)
            {
                item.Checks.Add(new Check
                {
                    Checked = true,
                    CheckedDate = DateTime.Today,
                    Comment = comment,
                    //TodoItemId = item.Id;
                });
            }
            else
            {
                check.Checked = true;
                check.Comment = comment;
            }

        }

    }
}
