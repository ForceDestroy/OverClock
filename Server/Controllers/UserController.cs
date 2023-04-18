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
    [Route("User/")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public UserController(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [HttpPut]
        [Route("UpdateAccountObsolete")]
        public async Task<ActionResult<UserProfile?>> UpdateAccountObsolete(UserProfile newUser, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            if (session == null)
            {
                return NotFound();
            }

            if (session.User.Id != newUser.Id)
            {
                return Unauthorized();
            }

            return await _userService.UpdateAccount(newUser);
        }

        [HttpPut]
        [Route("UpdateThemeObsolete")]
        public async Task<ActionResult<UserProfile?>> UpdateThemeObsolete(int themeColor, string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            if (session == null)
            {
                return NotFound();
            }

            return await _userService.UpdateTheme(session.User.Id, themeColor);
        }

        [HttpGet]
        [Route("GetAccountInfoObsolete")]
        public async Task<ActionResult<UserProfile?>> GetAccountInfoObsolete(string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            if (session == null)
            {
                return NotFound();
            }

            return await _userService.GetAccountInfo(session.User.Id);
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<ActionResult<string>> UpdateAccount( EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["newUser"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            UserProfile? newUser;
            try
            {
                newUser = JS.JsonSerializer.Deserialize<UserProfile>(jobj["newUser"].ToString());
            }
            catch(JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null || newUser == null)
            {
                return NotFound("Session not found or Data was null");
            }

            if (session.User.Id != newUser.Id)
            {
                return Unauthorized();
            }

            var response = await _userService.UpdateAccount(newUser);
            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("GetAccountInfo")]
        public async Task<ActionResult<string>> GetAccountInfo(EncryptedData data)
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

            var response = await _userService.GetAccountInfo(session.User.Id);
            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(response));
        }

        [HttpPut]
        [Route("UpdateTheme")]
        public async Task<ActionResult<string>> UpdateTheme( EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["themeColor"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null)
            {
                return NotFound();
            }

            var response = await _userService.UpdateTheme(session.User.Id, jobj["themeColor"].Value<int>());
            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(response));
        }
    }
}
