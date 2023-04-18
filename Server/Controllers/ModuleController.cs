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
    [Route("Module/")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly ISessionService _sessionService;

        public ModuleController(IModuleService ModuleService, ISessionService SessionService)
        {
            _moduleService = ModuleService;
            _sessionService = SessionService;
        }

        #region CreateModule
        [HttpPost]
        [Route("CreateModule")]
        public async Task<ActionResult> CreateModule(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["moduleInfo"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            ModuleInfo? moduleInfo;
            try
            {
                moduleInfo = JS.JsonSerializer.Deserialize<ModuleInfo>(jobj["moduleInfo"].ToString());
            }
            catch (JS.JsonException)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null || moduleInfo == null)
            {
                return NotFound();
            }

            moduleInfo.EmployerId = session.User.Id;
            await _moduleService.CreateModule(moduleInfo);
            return Ok();
        }
        #endregion

        #region DeleteModule
        [HttpDelete]
        [Route("DeleteModule")]
        public async Task<ActionResult> DeleteModule(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["moduleId"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null)
            {
                return NotFound();
            }

            if (!await _moduleService.DeleteModule(jobj["moduleId"].Value<int>()))
            {
                return BadRequest();
            }

            return Ok();
        }
        #endregion

        #region GetAllModulesForEmployer
        [HttpPost]
        [Route("GetAllModulesForEmployer")]
        public async Task<ActionResult<string>> GetAllModulesForEmployer(EncryptedData data)
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

            ICollection<ModuleSummary>? modules = await _moduleService.GetAllModulesForEmployer(session.User.Id);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(modules.ToList()));
        }
        #endregion

        #region GetAllModulesForAnEmployee
        [HttpPost]
        [Route("GetAllModulesForAnEmployee")]
        public async Task<ActionResult<string>> GetAllModulesForAnEmployee(EncryptedData data)
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

            ICollection<ModuleInfo> modules = await _moduleService.GetAllModulesForAnEmployee(session.User.Id);

            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(modules.ToList()));
        }
        #endregion

        #region UpdateModuleStatusEmployee
        [HttpPost]
        [Route("UpdateModuleStatusEmployee")]
        public async Task<ActionResult> UpdateModuleStatusEmployee(EncryptedData data)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(EncryptionHelper.DecryptTLS(data.Data));

            if (jobj == null || jobj["sessionToken"] == null || jobj["moduleStatusId"] == null || jobj["status"] == null)
            {
                return BadRequest("Passed data did not follow the expected format.");
            }

            SessionToken? session = await _sessionService.GetSession(jobj["sessionToken"].Value<string>());

            if (session == null)
            {
                return NotFound();
            }

            if (await _moduleService.UpdateModuleStatusEmployee(jobj["moduleStatusId"].Value<int>(), jobj["status"].Value<string>()))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
