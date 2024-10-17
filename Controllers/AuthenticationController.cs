using JWT_AUTHENTICATION.Models;
using JWT_AUTHENTICATION.Responses;
using JWT_AUTHENTICATION.Services.PasswordHasher;
using JWT_AUTHENTICATION.Services.TokenGenerator;
using JWT_AUTHENTICATION.Services.UserRepositories;
using Microsoft.AspNetCore.Mvc;

namespace JWT_AUTHENTICATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AccessTokenGenerator _accessTokenGenerator;

        public AuthenticationController(IUserRepository userRepository, IPasswordHasher passwordHasher, AccessTokenGenerator accessTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
        }

        [HttpPost("register")]
        // Models.Requests.RegisterRequests to avoid conflict with MICROSOFT.ASPDOTNETCORE.IDENTITY.DATA.REGISTERREQUEST
        public async Task<IActionResult> Register([FromBody] Models.Requests.RegisterRequest registerRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (registerRequest.Password != registerRequest.ConformPassword)
            {
                return BadRequest(new ErrorResponses("The password does not match with the confirm password"));
            }

            // check if the email provided already EXISTs in the system
            User existingUserByEmail = await _userRepository.GetByUserName(registerRequest.Email);
            if(existingUserByEmail !=  null)
            {
                return Conflict(new ErrorResponses("A user with such an email already exists"));
            }

            User existingUserByUserName = await _userRepository.GetByUserName(registerRequest.UserName);
            if (existingUserByUserName != null)
            {
                return Conflict(new ErrorResponses("A user with matching username already exists."));
            }

            // hash the password
            string hashedPassword = _passwordHasher.HashPassword(registerRequest.Password);

            User newUser = new User()
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                PasswordHash  = hashedPassword,
            };

            await _userRepository.CreateUser(newUser);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.Requests.LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User user = await  _userRepository.GetByUserName(loginRequest.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if(!isCorrectPassword)
            {
                return Unauthorized();
            }

            // the jwt access token 
            string accessToken = _accessTokenGenerator.GeneratorToken(user);
            return Ok(new AuthenticatedUserResponse
            {
                AccessToken = accessToken,  
            });

        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponses(errorMessages));
        }

    }
}
