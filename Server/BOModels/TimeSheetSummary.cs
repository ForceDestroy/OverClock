using Server.DbModels;
using Server.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Server.BOModels
{
    public class TimeSheetSummary
    {
        public string UserId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Salary { get; set; }
        public double HoursWorked { get; set; } = 0;
        public double OvertimeWorked { get; set; } = 0;

        public double DoubleOvertimeWorked {get; set; } = 0;

        public double PaidSick { get; set; } = 0;

        public double PaidHoliday { get; set; } = 0;

        public DateTime Date { get; set; }

        public TimeSheetSummary() { }
        public TimeSheetSummary(List<WorkHours> workHours, User user)
        {
            UserId = user.Id;
            Name = user.Name;
            Salary = user.Salary;

            Date = workHours[0].Date.AddDays(-1 * (int)workHours[0].Date.DayOfWeek).Date;
            for (int i = 0; i < workHours.Count; i++)
            {
                TimeSpan ts = workHours[i].EndTime - workHours[i].StartTime;

                switch (workHours[i].Function)
                {
                    case nameof(WorkHourFunctions.Work):
                        HoursWorked += ts.TotalHours;
                        break;
                    case nameof(WorkHourFunctions.Sick):
                        PaidSick += ts.TotalHours;
                        break;
                    case nameof(WorkHourFunctions.Holiday):
                        PaidHoliday += ts.TotalHours;
                        break;
                    default:
                        break;
                }
            }

            if (HoursWorked > 40)
            {
                OvertimeWorked = HoursWorked - 40;
                HoursWorked = 40;
            }

            if (OvertimeWorked > 20)
            {
                DoubleOvertimeWorked = OvertimeWorked - 20;
                OvertimeWorked = 20;
            }
        }
    }
}
