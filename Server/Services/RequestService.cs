using Microsoft.EntityFrameworkCore;
using Server.BOModels;
using Server.Data;
using Server.DbModels;
using Server.Enums;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class RequestService : IRequestService
    {
        private readonly DatabaseContext _databaseContext;

        public RequestService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> RequestTimeOff(RequestInfo requestInfo, string employeeId)
        {
            User? employee = await _databaseContext.Users.Include(x => x.SentRequests).Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
            {
                return false;
            }

            if (employee.Manager == null)
            {
                return false;
            }

            requestInfo.Date = requestInfo.Date.ToLocalTime();
            requestInfo.StartTime = requestInfo.StartTime.ToLocalTime();
            requestInfo.EndTime = requestInfo.EndTime.ToLocalTime();

            requestInfo.FromId = employeeId;
            requestInfo.ToId = employee.Manager.Id;
            requestInfo.Status = nameof(RequestStatus.Pending);

            var request = new Request(requestInfo)
            {
                From = employee,
                To = employee.Manager
            };

            await _databaseContext.Requests.AddAsync(request);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RequestMultipleTimesOff(ICollection<RequestInfo> listOfRequests, string employeeId)
        {
            User? employee = await _databaseContext.Users.Include(x => x.SentRequests).Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
            {
                return false;
            }

            if (employee.Manager == null)
            {
                return false;
            }

            foreach(RequestInfo requestInfo in listOfRequests)
            {
                requestInfo.FromId = employeeId;
                requestInfo.ToId = employee.Manager.Id;
                requestInfo.Status = nameof(RequestStatus.Pending);

                requestInfo.Date = requestInfo.Date.ToLocalTime();
                requestInfo.StartTime = requestInfo.StartTime.ToLocalTime();
                requestInfo.EndTime = requestInfo.EndTime.ToLocalTime();

                Request request = new(requestInfo)
                {
                    From = employee,
                    To = employee.Manager
                };

                await _databaseContext.Requests.AddAsync(request);
            }
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CustomRequest(RequestInfo requestInfo, string employeeId)
        {
            User? employee = await _databaseContext.Users.Include(x => x.SentRequests).Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
            {
                return false;
            }

            if (employee.Manager == null)
            {
                return false;
            }

            requestInfo.Date = requestInfo.Date.ToLocalTime();
            requestInfo.StartTime = requestInfo.StartTime.ToLocalTime();
            requestInfo.EndTime = requestInfo.EndTime.ToLocalTime();

            requestInfo.FromId = employeeId;
            requestInfo.ToId = employee.Manager.Id;
            requestInfo.Status = nameof(RequestStatus.Pending);
            requestInfo.StartTime = DateTime.Now;
            requestInfo.EndTime = DateTime.Now;

            var request = new Request(requestInfo)
            {
                From = employee,
                To = employee.Manager
            };

            await _databaseContext.Requests.AddAsync(request);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<RequestInfo>> GetOwnRequests(string employeeId)
        {
            User? user = await _databaseContext.Users.Include(x => x.SentRequests).Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == employeeId);

            if (user == null)
            {
                return new List<RequestInfo>();
            }

            ICollection<RequestInfo> listOfRequests = new List<RequestInfo>();

            foreach(Request r in user.SentRequests)
            {
                listOfRequests.Add(new RequestInfo(r));
            }

            return listOfRequests;
        }

        public async Task<ICollection<RequestInfo>> EmployerGetAllRequests(string employerId)
        {
            User? manager = await _databaseContext.Users.Include(x => x.ReceivedRequests).Include(x => x.Manages).FirstOrDefaultAsync(x => x.Id == employerId);

            if (manager == null)
            {
                return new List<RequestInfo>();
            }

            ICollection<RequestInfo> listOfRequests = new List<RequestInfo>();

            foreach (Request r in manager.ReceivedRequests)
            {
                listOfRequests.Add(new RequestInfo(r));
            }

            return listOfRequests;
        }

        public async Task<bool> ApproveRequest(int requestId, string newStatus)
        {
            Request? requestInfo = await _databaseContext.Requests.Include(x => x.From).Include(x => x.To).FirstOrDefaultAsync(x => x.Id == requestId);

            if (requestInfo == null)
            {
                return false;
            }

            User? user = await _databaseContext.Users.Include(x => x.ReceivedRequests).Include(x => x.Manages).FirstOrDefaultAsync(x => x.Id == requestInfo.ToId);

            if (user == null || user.Manages == null)
            {
                return false;
            }

            // Checking for manager/employee relationship
            if (user.Manages.FirstOrDefault(x => x.Id == requestInfo.FromId) == null)
            {
                return false;
            }

            if (StringComparison.Equals(newStatus.ToLower(), nameof(RequestStatus.Approved).ToLower()))
            {
                if (!await ApprovedRequestLogic(requestInfo.FromId, requestId))
                    return false;

                requestInfo.Status = nameof(RequestStatus.Approved);
            }
            else if (StringComparison.Equals(newStatus.ToLower(), nameof(RequestStatus.Denied).ToLower()))
            {
                requestInfo.Status = nameof(RequestStatus.Denied);
            }
            else if (StringComparison.Equals(newStatus.ToLower(), nameof(RequestStatus.Acknowledged).ToLower()))
            {
                requestInfo.Status = nameof(RequestStatus.Acknowledged);
            }
            else
            {
                return false;
            }
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        #region helpers
        public async Task<bool> ApprovedRequestLogic(string userId, int requestId)
        {
            User? user = await _databaseContext.Users.Include(x => x.SentRequests).FirstOrDefaultAsync(x => x.Id == userId);

            Request? request = await _databaseContext.Requests.Include(x => x.From).FirstOrDefaultAsync(x => x.Id == requestId);

            if (request == null || user == null)
            {
                return false;
            }

            // Take all schedules in the request's time frame and submit workhours for each schedule

            TimeSpan interval = request.EndTime - request.StartTime;

            if(StringComparison.Equals(request.Type.ToLower(), nameof(RequestType.Vacation).ToLower())){
                if(interval.Days > user.VacationDays)
                {
                    return false;
                }

                for(DateTime day = request.StartTime; day.Date <= request.EndTime.Date; day = day.AddDays(1))
                {
                    user.VacationDays --;
                    await _databaseContext.WorkHours.AddAsync(new WorkHours()
                    {
                        Date = day,
                        StartTime = new DateTime(day.Year, day.Month, day.Day, 09, 00, 00),
                        EndTime = new DateTime(day.Year, day.Month, day.Day, 17, 00, 00),
                        User = user,
                        Function = nameof(WorkHourFunctions.Holiday),
                        Status = nameof(WorkHourStatus.Submitted)
                    });
                }
            }
            else if(StringComparison.Equals(request.Type.ToLower(), nameof(RequestType.Sick).ToLower())){
                if (interval.Days > user.SickDays)
                {
                    return false;
                }

                for (DateTime day = request.StartTime; day.Date <= request.EndTime.Date; day = day.AddDays(1))
                {
                    user.SickDays--;
                    await _databaseContext.WorkHours.AddAsync(new WorkHours()
                    {
                        Date = day,
                        StartTime = new DateTime(day.Year, day.Month, day.Day, 09, 00, 00),
                        EndTime = new DateTime(day.Year, day.Month, day.Day, 17, 00, 00),
                        User = user,
                        Function = nameof(WorkHourFunctions.Sick),
                        Status = nameof(WorkHourStatus.Submitted)
                    });
                }
            }

            await _databaseContext.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
