using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;
        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request) 
        {
            //check id user is authenticated
            //check username and password
            var user = await userRepository.AuthenticateAsync(request.UserName, request.PassWord);

            if (user != null)
            {
                //Generate Jwt Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }

            return BadRequest("Username or password is incorrect");
        }
    }
}
