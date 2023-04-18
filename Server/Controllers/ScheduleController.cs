using Server.Data;
using Server.DbModels;
using Server.Services.Interfaces;
using Server.Helpers.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JS = System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("Schedule/")]

    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly ISessionService _sessionService;
        private readonly IEmployerService _employerService;

        public ScheduleController(IScheduleService scheduleService, ISessionService sessionService, IEmployerService employerService)
        {
            _scheduleService = scheduleService;
            _sessionService = sessionService;
            _employerService = employerService;
        }

        #region Obsolete
        [HttpPost]
        [Route("ModifyScheduleObsolete")]
        public async Task<ActionResult> ModifyScheduleObsolete(ICollection<ScheduleInfo> scheduleSubmissions, string userId, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (!await _employerService.ValidateAuthorization(session.User.Id, userId))
            {
                return Unauthorized();
            }

            await _scheduleService.ModifySchedule(scheduleSubmissions, userId);

            return Ok();
        }

        // Employee getting all his schedules
        [HttpGet]
        [Route("GetAllSchedulesForEmployeeObsolete")]

        public async Task<ActionResult<ICollection<ScheduleInfo>>> GetAllSchedulesForEmployeeObsolete(string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            ICollection<ScheduleInfo> allScheduleInfoList = await _scheduleService.GetAllSchedulesForEmployee(session.User.Id);

            return allScheduleInfoList.ToList();
        }

        // Employer getting an employee's schedule for a set week
        [HttpGet]
        [Route("GetSchedulesForAnEmployeeObsolete")]
        public async Task<ActionResult<ICollection<ScheduleInfo>>> GetSchedulesForAnEmployeeObsolete(string employeeId, string sessionToken, DateTime date)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (! await _employerService.ValidateAuthorization(session.User.Id, employeeId))
            {
                return Unauthorized();
            }

            ICollection<ScheduleInfo> allScheduleInfoList = await _scheduleService.GetSchedulesForAnEmployee(employeeId, session.User.Id, date);

            return allScheduleInfoList.ToList();
        }
        #endregion

        [HttpPost]
        [Route("ModifySchedule")]
        public async Task<ActionResult> ModifySchedule(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["scheduleSubmissions"] == null || jobj["userId"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            ICollection<ScheduleInfo>? scheduleSubmissions;
            try
            {
                scheduleSubmissions = JS.JsonSerializer.Deserialize<ICollection<ScheduleInfo>>(jobj["scheduleSubmissions"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null || scheduleSubmissions == null)
            {
                return NotFound("Session was not found or data was null");
            }

            if (!await _employerService.ValidateAuthorization(session.User.Id, jobj["userId"].Value<string>()))
            {
                return Unauthorized();
            }

            await _scheduleService.ModifySchedule(scheduleSubmissions, jobj["userId"].Value<string>());

            return Ok();
        }

        // Employee getting all his schedules
        [HttpPost]
        [Route("GetAllSchedulesForEmployee")]

        public async Task<ActionResult<string>> GetAllSchedulesForEmployee(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            ICollection<ScheduleInfo> allScheduleInfoList = await _scheduleService.GetAllSchedulesForEmployee(session.User.Id);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(allScheduleInfoList.ToList()));
        }

        // Employer getting an employee's schedule for a set week
        [HttpPost]
        [Route("GetSchedulesForAnEmployee")]
        public async Task<ActionResult<string>> GetSchedulesForAnEmployee(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["employeeId"] == null || jobj["date"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (!await _employerService.ValidateAuthorization(session.User.Id, jobj["employeeId"].Value<string>()))
            {
                return Unauthorized();
            }

            ICollection<ScheduleInfo> allScheduleInfoList = await _scheduleService.GetSchedulesForAnEmployee(jobj["employeeId"].Value<string>(), session.User.Id, jobj["date"].Value<DateTime>());

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(allScheduleInfoList.ToList()));
        }
    }
}
