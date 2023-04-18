using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("Login/")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ISessionService _sessionService;

        public LoginController(ILoginService loginService, ISessionService sessionService)
        {
           _loginService = loginService;
           _sessionService = sessionService;
        }

        [HttpPost]
        [Route("ValidateLoginObsolete")]
        public async Task<ActionResult> ValidateLoginObsolete(string email, string password)
        {
            User? user = await _loginService.GetUser(email);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                bool isValidLogin = await _loginService.ValidateLogin(email, password);

                if (isValidLogin)
                {
                    SessionToken session = await _sessionService.CreateSession(user);
                    return Ok(new { token = session.Token });
                }
                else {
                    return BadRequest();
                }
            }
        }

        [HttpPost]
        [Route("ValidateLogin")]
        public async Task<ActionResult<string>> ValidateLogin(EncryptedData data)
        {
            LoginInfo? login = JsonConvert.DeserializeObject<LoginInfo>(EncryptionHelper.DecryptTLS(data.Data));
            if (login == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            User? user = await _loginService.GetUser(login.Email);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                bool isValidLogin = await _loginService.ValidateLogin(login.Email, login.Password);

                if (isValidLogin)
                {
                    SessionToken token = await _sessionService.CreateSession(user);
                    SessionInfo session = new(token, user);

                    return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(session));
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPost]
        [Route("LogoutObsolete")]
        public async Task<ActionResult> LogoutObsolete(string token)
        {
            if (await _sessionService.EndSession(token))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> Logout(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["token"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            if (await _sessionService.EndSession(jobj["token"].Value<string>()))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}