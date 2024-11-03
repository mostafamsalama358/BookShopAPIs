using Bl.UnitOfWork;
using Domains.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BookShopAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetRolesWithMembers")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetRolesWithMembers()
        {
            try
            {
                var roles = await _unitOfWork.RoleManager.Roles.ToListAsync();
                var rolewithUser = new List<RoleWithUsersDto>();

                foreach (var role in roles)
                {
                    var usersInRole = await _unitOfWork.UserManager.GetUsersInRoleAsync(role.Name);
                    var userNames = usersInRole.Select(u => u.UserName).ToList();

                    rolewithUser.Add(new RoleWithUsersDto
                    {
                        RoleName = role.Name,
                        Users = userNames
                    });
                }
                return Ok(rolewithUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

