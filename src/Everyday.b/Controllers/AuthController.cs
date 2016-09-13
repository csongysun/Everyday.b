using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Identity;
using Everyday.b.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Everyday.b.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager _userManager;

        public AuthController(UserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("signup")]
        public async Task<ActionResult> SignUp([FromForm]User user)
        {
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm]LoginModel model)
        {
            var result = await _userManager.PasswordSignInAsync(model.UserKey, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(result.Obj);
        }

    }
}
