using IdentityAuthLesson.DTOs;
using IdentityAuthLesson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuthLesson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateRole(RoleDTO role)
        {

            var result = await _roleManager.FindByNameAsync(role.RoleName);

            if (result == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(role.RoleName));

                return Ok(new ResponseDTO
                {
                    Message = "Role Created",
                    IsSuccess = true,
                    StatusCode = 201
                });
            }

            return Ok(new ResponseDTO
            {
                Message = "Role cann not created",
                StatusCode = 403
            });
        }


        [HttpGet]
        public async Task<ActionResult<List<IdentityRole>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }
        [HttpGet]

        public async Task<ActionResult<IdentityRole>> GetRoleById(Guid id)
        {
            var res = await _roleManager.FindByIdAsync(id.ToString());
            if (res != null)
            {
                return Ok(res);
            }
            return BadRequest("Role not found");
        }
        [HttpDelete]

        public async Task<ActionResult<bool>> DeleteRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                return Ok(true);
            }
            return BadRequest(false);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateRole(Guid id, RoleDTO role)
        {
            var storedRole = await _roleManager.FindByIdAsync(id.ToString());
            if(storedRole != null)
            {
                storedRole.Name = role.RoleName;
                return Ok(storedRole);
            }
            return Ok(false);
        }
    }
}
