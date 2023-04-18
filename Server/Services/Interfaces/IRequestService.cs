using Server.Data;
using Server.DbModels;
using Server.BOModels;

namespace Server.Services.Interfaces
{
    public interface IRequestService
    {
        public Task<bool> RequestTimeOff(RequestInfo requestInfo, string employeeId);

        public Task<bool> RequestMultipleTimesOff(ICollection<RequestInfo> listOfRequests, string employeeId);

        public Task<bool> CustomRequest(RequestInfo requestInfo, string employeeId);

        public Task<ICollection<RequestInfo>> GetOwnRequests(string employeeId);

        public Task<ICollection<RequestInfo>> EmployerGetAllRequests(string employerId);

        public Task<bool> ApproveRequest(int requestId, string newStatus);

        #region helpers
        public Task<bool> ApprovedRequestLogic(string userId, int requestId);
        #endregion

    }
}
