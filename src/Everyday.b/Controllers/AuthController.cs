using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Common;
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
        public async Task<ActionResult> SignUp([FromBody]SignUpModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(new[] {ErrorDescriber.ModelNotValid});

            var result = await _userManager.CreateAsync(model);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]LoginModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(new[] { ErrorDescriber.ModelNotValid });
            var result = await _userManager.PasswordSignInAsync(model.UserKey, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            var user = result.Obj as User;
            if (user == null) return BadRequest(new[] {ErrorDescriber.DefaultError});
            return Ok(new
            {
                user.UserName,
                user.Email,
                user.Token,
                user.TokenExpires,
                user.RefreshToken,
                user.RefreshTokenExpires
            });
        }

        [HttpGet("refresh")]
        public async Task<ActionResult> TokenRefresh([FromQuery]TokenRefreshModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(new[] { ErrorDescriber.ModelNotValid });
            var result = await _userManager.TokenRefresh(model.refresh_token);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            var user = result.Obj as User;
            if (user == null) return BadRequest(new[] { ErrorDescriber.DefaultError });
            return Ok(new
            {
                user.UserName,
                user.Email,
                user.Token,
                user.TokenExpires,
                user.RefreshToken,
                user.RefreshTokenExpires
            });
        }
    }
}
