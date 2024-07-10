using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Demo.WebApi.Common.DTOs.InfrastructureDTOs;
using Demo.Infrastructure.JWT.Abstracts;

namespace Demo.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        #region PRIVATE INSTANCE FIELD
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtTokenManager _jwtToken;
        #endregion

        #region CONSTRUCTOR
        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager , IJwtTokenManager jwtToken)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtToken = jwtToken;
        }
        #endregion

        #region PUBLIC METHODS
        [HttpPost("Register")]
        public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await _userManager.CreateAsync(user, userCredentials.Password);
            if (result.Succeeded)
            {
               return _jwtToken.BuildToken(userCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email,userCredentials.Password,isPersistent:false, lockoutOnFailure:false);
            if (result.Succeeded)
            {
                return _jwtToken.BuildToken(userCredentials);
            }
            else
            {
                return BadRequest("Invalid Login.");
            }
        }
        #endregion

    }
}
