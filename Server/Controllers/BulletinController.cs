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
    [Route("Bulletin/")]
    [ApiController]
    public class BulletinController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IBulletinService _bulletinService;

        public BulletinController(ISessionService sessionService, IBulletinService bulletinService)
        {
            _sessionService = sessionService;
            _bulletinService = bulletinService;
        }

        [HttpPost]
        [Route("CreateAnnouncement")]
        public async Task<ActionResult> CreateAnnouncement(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["announcement"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            AnnouncementInfo? announcement;
            try
            {
                announcement = JS.JsonSerializer.Deserialize<AnnouncementInfo>(jobj["announcement"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null || announcement == null)
            {
                return NotFound("Session not found or Data was null");
            }

            if (session.User.AccessLevel < 1)
            {
                return Unauthorized();
            }

            await _bulletinService.CreateAnnouncement(announcement, session.User.Id);
            return Ok();
        }

        [HttpPost]
        [Route("GetMyAnnouncements")]
        public async Task<ActionResult<string>> GetMyAnnouncements(EncryptedData data)
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

            var response = await _bulletinService.GetMyAnnouncements(session.User.Id);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("GetAllAnnouncements")]
        public async Task<ActionResult<string>> GetAllAnnouncements(EncryptedData data)
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

            var response = await _bulletinService.GetAllAnnouncements();

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("DeleteAnnouncement")]
        public async Task<ActionResult> DeleteAnnouncement(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["announcementId"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null)
            {
                return NotFound();
            }

            if (session.User.AccessLevel < 1)
            {
                return Unauthorized();
            }

            if (!await _bulletinService.DeleteAnnouncement(jobj["announcementId"].Value<int>()))
            {
                return BadRequest("Announcement either could not be found");
            }

            return Ok();
        }
    }
}
