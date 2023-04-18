class BaseAPI {
  static String baseUrl = '';
  static String validateLoginEndpoint = '/Login/ValidateLogin?';
  static String getAccountInfoEndpoint = '/User/GetAccountInfo?';
  static String updateAccountInfoEndpoint = '/User/UpdateAccount?';
  static String logHoursEndpoint = '/TimeSheet/LogHours?';
  static String getHoursEndpoint = '/TimeSheet/GetHours?';
  static String updateThemeEndpoint = '/User/UpdateTheme?';
  static String getAssignedSchedule = '/Schedule/GetAllSchedulesForEmployee?';
  static String scheduleChangeEndpoint = '/Request/RequestTimeOff?';
  static String customRequestEndpoint = '/Request/CustomRequest?';
  static String timeOffRequestEndpoint = '/Request/RequestTimeOff?';
  static String getPayslips = '/PaySlip/GetPaySlips?';
  static String getTimeSheetEndpoint = '/TimeSheet/GetTimeSheet?';
  static String submitTimeEndpoint = '/TimeSheet/SubmitTime?';
  static String logoutEndpoint = '/Login/Logout';
}
