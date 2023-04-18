using Server.Data;
using Server.DbModels;
using Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;

namespace Server.Controllers
{
    [Route("SysAdmin/")]
    [ApiController]
    public class SysAdminController : ControllerBase
    {
        private readonly ISysAdminService _sysAdminService;

        public SysAdminController(ISysAdminService sysAdminService)
        {
            _sysAdminService = sysAdminService;
        }

        [HttpGet]
        [Route("GetUser")]
        public ActionResult<User> GetUser(string id)
        {
            User? user = _sysAdminService.GetUser(id);

            return user ?? (ActionResult<User>)NotFound();
        }

        [HttpPost]
        [Route("CreateUser")]
        public ActionResult CreateUser(UserInfo user)
        {
            bool isSuccess = _sysAdminService.CreateUser(user);

            if (isSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public ActionResult<User> UpdateUser(UserInfo updatedUser)
        {
            User? user = _sysAdminService.UpdateUser(updatedUser);

            return user ?? (ActionResult<User>)NotFound();
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public ActionResult<User> DeleteUser(string id)
        {
            User? user = _sysAdminService.DeleteUser(id);

            return user ?? (ActionResult<User>)NotFound();
        }

        [HttpPut]
        [Route("SetManagerEmployeeRelationship")]
        public ActionResult SetManagerEmployeeRelationship(string managerId, string employeeId)
        {
            if (!_sysAdminService.SetManagerEmployeeRelationship(managerId, employeeId))
            {
                return BadRequest("Failed to set manager");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("RemoveManagerEmployeeRelationship")]
        public ActionResult RemoveManagerEmployeeRelationship(string managerId, string employeeId)
        {
            if (!_sysAdminService.RemoveManagerEmployeeRelationship(managerId, employeeId))
            {
                return BadRequest("Failed to remove manager");
            }

            return Ok();
        }
    }
}
