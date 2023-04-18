using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JS = System.Text.Json;

namespace Server.Controllers
{
    [Route("Employer/")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IEmployerService _employerService;

        public EmployerController(IEmployerService EmployerService, ISessionService SessionService)
        {
            _employerService = EmployerService;
            _sessionService = SessionService;
        }

        #region Obsolete

        [HttpGet]
        [Route("GetEmployeeObsolete")]
        public async Task<ActionResult<UserInfo>> GetEmployeeObsolete(string userId, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result, i.e employee cannot get employees 
            if (session == null)
            {
                return NotFound();
            }

            UserInfo? employee = await _employerService.GetEmployee(userId);

            if (employee == null)
            {
                return BadRequest(); // If employee does not exist should return badrequest result 
            }
            else // checking for user accesslevel 
            {
                // If user accesslevel is lower than employer return Unauthorized result 
                if (session.User.AccessLevel <= employee.AccessLevel)
                {
                    return Unauthorized();
                }
                else
                {
                    // If user accesslevel is of employer level return UserInfo
                    return employee;
                }
            }
        }

        [HttpGet]
        [Route("GetListOfEmployeesObsolete")]

        public async Task<ActionResult<ICollection<UserInfo>>> GetListOfEmployeesObsolete(string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            ICollection<UserInfo>? allEmployeeInfoList = await _employerService.GetListOfEmployees(session.User.Id);

            if (allEmployeeInfoList == null)
            {
                return BadRequest("The list of employees you are retrieving doesn't exist.");
            }
            else
            {
                return allEmployeeInfoList.ToList();
            }
        }

        [HttpPost]
        [Route("CreateEmployeeObsolete")]
        public async Task<ActionResult> CreateEmployeeObsolete(UserInfo employee, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a NotFound result, i.e employee cannot create employees 
            if (session == null)
            {
                return NotFound();
            }
            // check access level 
            if (session.User.AccessLevel <= employee.AccessLevel)
            {
                return Unauthorized(); // If user accesslevel is lower than employer return Unauthorized result 
            }
            else
            {
                bool isSuccess = await _employerService.CreateEmployee(employee, session.User.Id);

                if (isSuccess)
                {
                    return Ok(); // If user accesslevel is of employer level return OkResult 
                }
                else
                {
                    return BadRequest(); // If employee already exists return Badrequest result 
                }
            }
        }

        [HttpPut]
        [Route("UpdateEmployeeObsolete")]
        public async Task<ActionResult<UserInfo>> UpdateEmployeeObsolete(UserInfo newEmployee, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);
            // If there is no session token then should return a notfound result, i.e employee update employees 
            if (session == null)
            {
                return NotFound();
            }
            // check accesslevel

            if (session.User.AccessLevel <= newEmployee.AccessLevel)
            {
                return Unauthorized(); // If user accesslevel is lower than employer return Unauthorized result 
            }
            else
            {
                UserInfo? employee = await _employerService.UpdateEmployee(newEmployee);

                if (employee == null)
                {
                    return BadRequest(); // If employee does not exists return badrequest result  
                }
                else
                {
                    return employee; // If user accesslevel is of employer level return updatedemployee 
                }
            }
        }

        [Route("DeleteEmployeeObsolete")]
        public async Task<ActionResult<UserInfo?>> DeleteEmployeeObsolete(string userId, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);
            // If there is no session token then should return a NotFound result, i.e employee delete employees 
            if (session == null)
            {
                return NotFound();
            }
            UserInfo? employee = await _employerService.GetEmployee(userId);

            if (employee == null)
            {
                return BadRequest(); //if employee dne return notfound result 
            }

            if (session.User.AccessLevel <= employee.AccessLevel)
            {
                return Unauthorized();// If user accesslevel is lower than employer return Unauthorized result   
            }

            UserInfo? deletedEmployee = await _employerService.DeleteEmployee(userId);

            return deletedEmployee;
        }
        #endregion

        #region GetEmployee 

        [HttpPost]
        [Route("GetEmployee")]
        public async Task<ActionResult<string>> GetEmployee(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["userId"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            UserInfo? employee = await _employerService.GetEmployee(jobj["userId"].Value<string>());

            if (employee == null)
            {
                return BadRequest(); // If employee does not exist should return badrequest result 
            }
            else // checking for user accesslevel 
            {
                // If user accesslevel is lower than employer return Unauthorized result 
                if(session.User.AccessLevel <= employee.AccessLevel)
                {
                    return Unauthorized();
                }
                else
                {
                    // If user accesslevel is of employer level return UserInfo
                    return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(employee));
                }
            }
        }

        #endregion

        #region GetListOfEmployees
        [HttpGet]
        [Route("GetListOfEmployees")]

        public async Task<ActionResult<string>> GetListOfEmployees(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null)
            {
                return NotFound();
            }

            ICollection<UserInfo>? allEmployeeInfoList = await _employerService.GetListOfEmployees(session.User.Id);

            if (allEmployeeInfoList == null)
            {
                return BadRequest("The list of employees you are retrieving doesn't exist.");
            }
            else
            {
                return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(allEmployeeInfoList.ToList()));
            }
        }

        #endregion

        #region CreateEmployee

        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<ActionResult> CreateEmployee(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["employee"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            UserInfo? employee;

            try
            {
                employee = JS.JsonSerializer.Deserialize<UserInfo>(jobj["employee"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a NotFound result, i.e employee cannot create employees 
            if (session == null || employee == null)
            {
                return NotFound("Session not found or Data was null");
            }
            // check access level 
            if(session.User.AccessLevel <= employee.AccessLevel)
            {
                return Unauthorized(); // If user accesslevel is lower than employer return Unauthorized result 
            }
            else
            {
                bool isSuccess = await _employerService.CreateEmployee(employee, session.User.Id);

                if (isSuccess)
                {
                    return Ok(); // If user accesslevel is of employer level return OkResult 
                }
                else
                {
                    return BadRequest(); // If employee already exists return Badrequest result 
                }
            }
        }

        #endregion

        #region UpdateEmployee

        [HttpPost]
        [Route("UpdateEmployee")]
        public async Task<ActionResult<string>> UpdateEmployee(EncryptedData data)

        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["employee"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            UserInfo? newEmployee;

            try
            {
                newEmployee = JS.JsonSerializer.Deserialize<UserInfo>(jobj["employee"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());
            // If there is no session token then should return a notfound result, i.e employee update employees 
            if (session == null || newEmployee == null)
            {
                return NotFound("Session not found or Data was null");
            }
            // check accesslevel

            if (session.User.AccessLevel <= newEmployee.AccessLevel)
            {
                return Unauthorized(); // If user accesslevel is lower than employer return Unauthorized result 
            }
            else
            {
                UserInfo? employee = await _employerService.UpdateEmployee(newEmployee);

                if (employee == null)
                {
                    return BadRequest(); // If employee does not exists return badrequest result  
                }
                else
                {
                    return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(employee)); // If user accesslevel is of employer level return updatedemployee 
                }
            }
        }

        #endregion

        #region DeleteEmployee

        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<ActionResult<string>> DeleteEmployee(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["userId"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            UserInfo? employee = await _employerService.DeleteEmployee(jobj["userId"].Value<string>());

            if (employee == null)
            {
                return BadRequest(); // If employee does not exist should return badrequest result 
            }
            else // checking for user accesslevel 
            {
                // If user accesslevel is lower than employer return Unauthorized result 
                if (session.User.AccessLevel <= employee.AccessLevel)
                {
                    return Unauthorized();
                }
                else
                {
                    // If user accesslevel is of employer level return UserInfo
                    return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(employee));
                }
            }
        }
        #endregion
    }
}