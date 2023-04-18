using Server.Data;
using Server.DbModels;
using Server.Services.Interfaces;
using Server.Helpers.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using Server.BOModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JS = System.Text.Json;
using Server.Migrations;

namespace Server.Controllers
{
    [ApiController]
    [Route("Request/")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly ISessionService _sessionService;

        public RequestController(IRequestService requestService, ISessionService sessionService)
        {
            _requestService = requestService;
            _sessionService = sessionService;
        }

        #region Obsolete
        [HttpPost]
        [Route("RequestTimeOffObsolete")]
        public async Task<ActionResult> RequestTimeOffObsolete(RequestInfo requestInfo, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (!await _requestService.RequestTimeOff(requestInfo, session.User.Id))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("RequestMultipleTimesOffObsolete")]
        public async Task<ActionResult> RequestMultipleTimesOffObsolete(ICollection<RequestInfo> listOfRequests, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (!await _requestService.RequestMultipleTimesOff(listOfRequests, session.User.Id))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("CustomRequestObsolete")]
        public async Task<ActionResult> CustomRequestObsolete(RequestInfo requestInfo, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (!await _requestService.CustomRequest(requestInfo, session.User.Id))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetOwnRequestsObsolete")]
        public async Task<ActionResult<ICollection<RequestInfo>>> GetOwnRequestsObsolete(string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null || session.User.Id == null)
            {
                return NotFound();
            }

            ICollection<RequestInfo> listOfRequests = await _requestService.GetOwnRequests(session.User.Id);

            return listOfRequests.OrderByDescending(x => x.Date).ToList();
        }

        [HttpGet]
        [Route("EmployerGetAllRequestsObsolete")]
        public async Task<ActionResult<ICollection<RequestInfo>>> EmployerGetAllRequestsObsolete(string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            ICollection<RequestInfo> listOfRequests = await _requestService.EmployerGetAllRequests(session.User.Id);

            return listOfRequests.OrderByDescending(x => x.Date).ToList();
        }

        [HttpPost]
        [Route("ApproveRequestObsolete")]
        public async Task<ActionResult> ApproveRequestObsolete(int requestId, string newStatus, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (!await _requestService.ApproveRequest(requestId, newStatus))
            {
                return BadRequest();
            }

            return Ok();
        }
        #endregion

        [HttpPost]
        [Route("RequestTimeOff")]
        public async Task<ActionResult> RequestTimeOff(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["requestInfo"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            RequestInfo? requestInfo;
            try
            {
                requestInfo = JS.JsonSerializer.Deserialize<RequestInfo>(jobj["requestInfo"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null || requestInfo == null)
            {
                return NotFound("Session not found or Data was null");
            }

            if (!await _requestService.RequestTimeOff(requestInfo, session.User.Id))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("RequestMultipleTimesOff")]
        public async Task<ActionResult> RequestMultipleTimesOff(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["listOfRequests"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            ICollection<RequestInfo>? listOfRequests;
            try
            {
                listOfRequests = JS.JsonSerializer.Deserialize<ICollection<RequestInfo>>(jobj["listOfRequests"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null || listOfRequests == null)
            {
                return NotFound("Sesion not found or Data was null");
            }

            if (!await _requestService.RequestMultipleTimesOff(listOfRequests, session.User.Id))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("CustomRequest")]
        public async Task<ActionResult> CustomRequest(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["requestInfo"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            RequestInfo? requestInfo;
            try
            {
                requestInfo = JS.JsonSerializer.Deserialize<RequestInfo>(jobj["requestInfo"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null || requestInfo == null)
            {
                return NotFound("Session not found or Data was null");
            }

            if (!await _requestService.CustomRequest(requestInfo, session.User.Id))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetOwnRequests")]
        public async Task<ActionResult<string>> GetOwnRequests(EncryptedData data)
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

            ICollection<RequestInfo>? listOfRequests = await _requestService.GetOwnRequests(session.User.Id);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(listOfRequests.OrderByDescending(x => x.Date).ToList()));
        }

        [HttpGet]
        [Route("EmployerGetAllRequests")]
        public async Task<ActionResult<string>> EmployerGetAllRequests(EncryptedData data)
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

            ICollection<RequestInfo>? listOfRequests = await _requestService.EmployerGetAllRequests(session.User.Id);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(listOfRequests.OrderByDescending(x => x.Date).ToList()));
        }

        [HttpPost]
        [Route("ApproveRequest")]
        public async Task<ActionResult> ApproveRequest(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["requestId"] == null || jobj["newStatus"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }

            if (!await _requestService.ApproveRequest(jobj["requestId"].Value<int>(), jobj["newStatus"].Value<string>()))
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
