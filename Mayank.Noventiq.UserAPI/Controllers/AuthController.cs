using Azure;
using Mayank.Noventiq.UserAPI.Models.DTO;
using Mayank.Noventiq.UserAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Mayank.Noventiq.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IAuthService _authService;
        private readonly IStringLocalizer<AuthController> _localizer;

        public AuthController(IAuthService authService, IJwtTokenGenerator jwtTokenGenerator, IStringLocalizer<AuthController> localizer)
        {
            _authService = authService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _localizer = localizer;

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var loginResponse = await _authService.Login(request);
            var _response = new ResponseDto();  
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = _localizer["InvalidCredentials"];
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            _response.Message = _localizer["LoginSuccess"];
            return Ok(_response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var message = await _authService.Register(model);
            if (message!= "Registration Successful")
            {
                return BadRequest(_localizer["InvalidRegistration"]);
            }
            else
            {
                return Ok(_localizer["RegisterSuccess"]);
            }
                
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh(TokenRequestDto request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest(_localizer["InvalidRefreshReq"]);

            var tokenResponse = await _authService.Refresh(request);
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                return Unauthorized(_localizer["InvalidRefershToken"]);

            return Ok(new
            {
                accessToken = tokenResponse.AccessToken,
                refreshToken = tokenResponse.RefreshToken,
                message = _localizer["TokenGenSuccess"]
            });
        }




    }
}
