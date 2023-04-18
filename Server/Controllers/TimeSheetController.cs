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
    [Route("TimeSheet/")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly ITimeSheetService _timeSheetService;
        private readonly IPayslipService _payslipService;

        public TimeSheetController(ITimeSheetService timeSheetService, ISessionService sessionService, IPayslipService payslipService)
        {
            _timeSheetService = timeSheetService;
            _sessionService = sessionService;
            _payslipService = payslipService;
        }

        [HttpPut]
        [Route("LogHoursObsolete")]
        public async Task<ActionResult> LogHoursObsolete(IEnumerable<WorkHoursInfo> submission, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }
            if (!_timeSheetService.ValidateWorkHours(submission))
            {
                return BadRequest("The Submitted TimeBlocks do not all share the same date.");
            }

            if (await _timeSheetService.CheckDaysSubmission(session.User.Id, submission.First().Date))
            {
                return BadRequest("Logged Hours for this Date have already been submitted. If modifications are necessary, please wait until timesheet submission to make them");
            }

            await _timeSheetService.LogWorkHours(session.User.Id, submission);
            return Ok();
        }

         [HttpGet]
        [Route("GetHoursObsolete")]
        public async Task<ActionResult<ICollection<WorkHoursInfo>>> GetHoursObsolete(DateTime day, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            var workhours = await _timeSheetService.GetWorkHours(session.User.Id, day);

            return workhours.ToList();
        }

        [HttpGet]
        [Route("GetTimeSheetObsolete")]
        public async Task<ActionResult<IEnumerable<IEnumerable<WorkHoursInfo>>>> GetTimeSheetObsolete(DateTime date, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            var workhours = await _timeSheetService.GetTimeSheet(session.User.Id, date);

            return workhours.ToList();
        }

        [HttpPut]
        [Route("SubmitTimeObsolete")]
        public async Task<ActionResult> SubmitTimeSheetObsolete(string sessionToken, IEnumerable<WorkHoursInfo> submission)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (await _timeSheetService.CheckForApprovedTimeSheet(session.User.Id, submission.First().Date))
            {
                return BadRequest("Time Sheet for this week has already gone through final approval");
            }

            await _timeSheetService.SubmitTimeSheet(session.User.Id, submission);
            return Ok();
        }

        [HttpGet]
        [Route("GetSubmittedTimeSheetsObsolete")]
        public async Task<ActionResult<IEnumerable<TimeSheetSummary>>> GetSubmittedTimeSheetsObsolete(string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            var timesheets = await _timeSheetService.GetSubmittedTimeSheets(session.User.Id);

            return timesheets.ToList();
        }

        [HttpPost]
        [Route("ApproveTimeSheetObsolete")]
        public async Task<ActionResult> ApproveTimeSheetObsolete(string sessionToken, TimeSheetSummary timeSheet)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }
            if (!await _timeSheetService.ApproveTimeSheet(timeSheet.UserId, timeSheet.Date.Date))
            {
                return BadRequest("There was no Timesheet matching the submitted data");
            }
            await _payslipService.CreatePayslip(timeSheet);

            return Ok();
        }

        [HttpPut]
        [Route("LogHours")]
        public async Task<ActionResult> LogHours(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["submission"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            IEnumerable<WorkHoursInfo>? submission;
            try
            {
                submission = JS.JsonSerializer.Deserialize<IEnumerable<WorkHoursInfo>>(jobj["submission"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null || submission == null)
            {
                return NotFound("Session not found or Data was null");
            }
            if (!_timeSheetService.ValidateWorkHours(submission))
            {
                return BadRequest("The Submitted TimeBlocks do not all share the same date.");
            }

            if (await _timeSheetService.CheckDaysSubmission(session.User.Id, submission.First().Date))
            {
                return BadRequest("Logged Hours for this Date have already been submitted. If modifications are necessary, please wait until timesheet submission to make them");
            }

            await _timeSheetService.LogWorkHours(session.User.Id, submission);
            return Ok();
        }

        [HttpPost]
        [Route("GetHours")]
        public async Task<ActionResult<string>> GetHours( EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["day"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            DateTime day;
            try
            {
                day = DateTime.Parse(jobj["day"].ToString());
            }
            catch (FormatException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            var workhours = await _timeSheetService.GetWorkHours(session.User.Id, day);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(workhours.ToList()));
        }

        [HttpPost]
        [Route("GetTimeSheet")]
        public async Task<ActionResult<string>> GetTimeSheet(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["date"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            DateTime date;
            try
            {
                date = DateTime.Parse(jobj["date"].ToString());
            }
            catch (FormatException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            var workhours = await _timeSheetService.GetTimeSheet(session.User.Id, date);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(workhours.ToList()));
        }

        [HttpPut]
        [Route("SubmitTime")]
        public async Task<ActionResult> SubmitTimeSheet( EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["submission"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            IEnumerable<WorkHoursInfo>? submission;
            try
            {
                submission = JS.JsonSerializer.Deserialize<IEnumerable<WorkHoursInfo>>(jobj["submission"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null || submission == null)
            {
                return NotFound("Session not found or Data was null");
            }

            if (await _timeSheetService.CheckForApprovedTimeSheet(session.User.Id, submission.First().Date))
            {
                return BadRequest("Time Sheet for this week has already gone through final approval");
            }

            await _timeSheetService.SubmitTimeSheet(session.User.Id, submission);
            return Ok();
        }

        [HttpPost]
        [Route("GetSubmittedTimeSheets")]
        public async Task<ActionResult<string>> GetSubmittedTimeSheets(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null )
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            var timesheets = await _timeSheetService.GetSubmittedTimeSheets(session.User.Id);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(timesheets.ToList()));
        }

        [HttpPost]
        [Route("ApproveTimeSheet")]
        public async Task<ActionResult> ApproveTimeSheet( EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["timeSheet"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            TimeSheetSummary? timeSheet;
            try
            {
                timeSheet = JS.JsonSerializer.Deserialize<TimeSheetSummary>(jobj["timeSheet"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }
            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null || timeSheet == null)
            {
                return NotFound("Session not found or Data was null");
            }
            if (!await _timeSheetService.ApproveTimeSheet(timeSheet.UserId, timeSheet.Date.Date))
            {
                return BadRequest("There was no Timesheet matching the submitted data");
            }
            await _payslipService.CreatePayslip(timeSheet);

            return Ok();
        }
    }
}