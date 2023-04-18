using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Server.BOModels;
using Server.DbModels;
using Server.Helpers.Server.Helpers;
using Server.Services;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("PaySlip/")]
    [ApiController]
    public class PaySlipController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IPayslipService _payslipService;

        public PaySlipController(IPayslipService PayslipService, ISessionService SessionService)
        {
            _payslipService = PayslipService;
            _sessionService = SessionService;
        }
        #region Obsolete
        [HttpGet]
        [Route("GetPaySlipsObsolete")]
        public async Task<ActionResult<IEnumerable<PaySlipInfo>>> GetPaySlipsObsolete(string sessionToken)
        {
            SessionToken? session = await _sessionService.GetSession(sessionToken);

            // If there is no session token then should return a notfound result
            if (session == null)
            {
                return NotFound();
            }
            var payslips = await _payslipService.GetPayslips(session.User.Id);
            return payslips.OrderByDescending(x=> x.StartDate).ToList();
        }
        #endregion

        [HttpPost]
        [Route("GetPaySlips")]
        public async Task<ActionResult<string>> GetPaySlips(EncryptedData data)
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
            var payslips = await _payslipService.GetPayslips(session.User.Id);
            return EncryptionHelper.EncryptTLS(JsonConvert.SerializeObject(payslips.OrderByDescending(x => x.StartDate).ToList()));
        }
    }
}
