using back.Data;
using back.Models.Domain;
using back.Models.DTO;
using back.Repositories.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;

namespace back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(UserCredentials _user)
        {
            var user = await _userRepository.Authenticate(_user);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenResponse _tokenResponse)
        {

            var tokenResponse = await _userRepository.RefresshToken(_tokenResponse);
            if (tokenResponse == null)
            {
                return Unauthorized();
            }
            return Ok(tokenResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userRepository.UserAsync(id);
            if (user == null)
            {
                return NotFound($"User with this id: {id} not Found!!");
            }
            var response = new GetUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Credentials= user.Credentials,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            //Map DTO to domain Model
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Credentials = "",
                Avatar = userDTO.Avatar,
                RoleId = userDTO.RoleId
            };

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Email + ":" + user.Password));

            user.Credentials = credentials;

            await _userRepository.CreateAsync(user);

            // Domain model to DTO
            var response = new GetUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Credentials = credentials,
                Avatar = user.Avatar,
                RoleId = user.RoleId
            };
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserDTO userDTO)
        {
            //Map DTO to domain Model
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Credentials = "",
                Avatar = userDTO.Avatar,
            };

            var usr = await _userRepository.UpdateAsync(user);

            if (usr == null)
            {
                return NotFound($"User not exist with this id {user.Id}");
            }

            // Domain model to DTO
            var response = new GetUserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Credentials = usr.Credentials,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userRepository.DeleteAsync(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            var response = new GetUserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Credentials = user.Credentials,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
            };
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {
            var _users = await _userRepository.UsersAsync();
            if (_users == null)
            {
                return NotFound("User Not Found");
            }

            List<GetUserDTO> users = new List<GetUserDTO>();
            foreach (User user in _users)
            {
                var _user = new GetUserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Credentials = user.Credentials,
                    Avatar = user.Avatar,
                    RoleId = user.RoleId,
                };
                users.Add(_user);
            }
            return Ok(users);
        }
    }
}
