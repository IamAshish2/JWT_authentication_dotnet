using JWT_AUTHENTICATION.Models;
using JWT_AUTHENTICATION.Models.Responses;
using JWT_AUTHENTICATION.Services.PasswordHasher;
using JWT_AUTHENTICATION.Services.RefreshTokenRepository;
using JWT_AUTHENTICATION.Services.TokenGenerator;
using JWT_AUTHENTICATION.Services.TokenValidator;
using JWT_AUTHENTICATION.Services.UserRepositories;
using Microsoft.AspNetCore.Identity.Data;
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
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public AuthenticationController(IUserRepository userRepository, IPasswordHasher passwordHasher,
            AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, RefreshTokenValidator refreshTokenValidator, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [HttpPost("register")]
        // Models.Requests.RegisterRequests to avoid conflict with MICROSOFT.ASPDOTNETCORE.IDENTITY.DATA.REGISTERREQUEST
        public async Task<IActionResult> Register([FromBody] Models.Requests.RegisterRequest registerRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (registerRequest.Password.Trim() != registerRequest.ConformPassword.Trim())
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

            // the jwt access token  => MoVE THIS PIECE OF CODE FOR generatin access and refresh tokens to a new file or method
            string accessToken = _accessTokenGenerator.GenerateAccessToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            RefreshToken refreshTokenDto = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id,
            };
            await _refreshTokenRepository.Create(refreshTokenDto);


            return Ok(new AuthenticatedUserResponse
            {
                AccessToken = accessToken,  
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool isValidRefresh = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefresh)
            {
                return BadRequest(new ErrorResponses("Invalid refresh token"));
            }

            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponses("Such a Token Not found"));   
            }

            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            if (user == null)
            {
                return NotFound(new ErrorResponses("User Not found"));
            }

            /// if the refresh token passes all the above checks then, generate a new JWT and refresh token
            string accessToken = _accessTokenGenerator.GenerateAccessToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            RefreshToken refreshTokenDto = new RefreshToken()
            {
                Token = accessToken,
                UserId = user.Id,
            };
            await _refreshTokenRepository.Create(refreshTokenDto);


            return Ok(new AuthenticatedUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponses(errorMessages));
        }

    }
}
