// ignore_for_file: prefer_interpolation_to_compose_strings, dead_code

import 'package:flutter/material.dart';
import 'package:mobile/themes/colors.dart';
import 'package:mobile/utils/api_service.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:date_picker_timeline/date_picker_timeline.dart';
import 'package:time_interval_picker/time_interval_picker.dart';
import 'package:mobile/themes/check_mark_animation.dart';
import 'dart:async';

List<TimeSheetWeekData> dateObjects = [];
DateTime currentDate = DateTime.now();
var currentMonth = DateTime.now().month;
var currentDay = DateTime.now().day;
var currentYear = DateTime.now().year;

var startTime = DateTime.now();
var endTime = DateTime.now();
var workDate = DateTime.now();

int count = 0;

Color headerColor = Colors.white;
Color gradientColor1 = Colors.white;
Color gradientColor2 = Colors.white;
Color animationColor = Colors.white;

bool editTimeInterval = false;
var currentWorkDay = [];
void removePeriod() {
  period.removeAt(period.length - 1);
  // print(period.length);
}

void submitTime() async {
  for (var i = 0; i < dateObjects.length; i++) {
    dateObjects[i].setDate(dateObjects[i].getDate().toUtc().toIso8601String());
    dateObjects[i]
        .setStartTime(dateObjects[i].getStartTime().toUtc().toIso8601String());
    dateObjects[i]
        .setEndTime(dateObjects[i].getEndTime().toUtc().toIso8601String());
  }
  //print("sending work day");
  final jsonDates = jsonEncode(dateObjects);
  await ApiService()
      .submitTime(jsonDates, http.Client(), prefs.getString('token'));
}

List<AddPeriod> period = [];

fromJson(Map<String, dynamic> jsonData) {
  return TimeSheetWeekData(
      jsonData['Date'], jsonData['StartTime'], jsonData['EndTime']);
}

Map<String, dynamic> toMap(TimeSheetWeekData dateObject) => {
      'Date': dateObject.getDate().toString(),
      'StartTime': dateObject.getStartTime().toString(),
      'EndTime': dateObject.getEndTime().toString(),
    };

String encode(List<TimeSheetWeekData> dateObject) => json.encode(
      dateObject
          .map<Map<String, dynamic>>((dateObject) => toMap(dateObject))
          .toList(),
    );
List<TimeSheetWeekData> decode(String dateObject) =>
    (json.decode(dateObject) as List<dynamic>)
        .map<TimeSheetWeekData>((item) => fromJson(item))
        .toList();

late SharedPreferences prefs;
late SharedPreferences pref;

//Create the Profile Page
class TimeSheet extends StatefulWidget {
  const TimeSheet({Key? key}) : super(key: key);
  static http.Client? httpClient;
  @override
  State<TimeSheet> createState() => _TimeSheetState();
}

class _TimeSheetState extends State<TimeSheet> {
  final double coverHeight = 250;
  int themeColor = 0;
  http.Client httpClient = http.Client();
  final DatePickerController _controller = DatePickerController();

  // void submitTime() async {
  //print("sending work week");
  // for (var i = 0; i < dateObjects.length; i++) {
  //   dateObjects[i]
  //       .setDate(dateObjects[i].getDate().toUtc().toIso8601String());
  //   dateObjects[i].setStartTime(
  //       dateObjects[i].getStartTime().toUtc().toIso8601String());
  //   dateObjects[i]
  //       .setEndTime(dateObjects[i].getEndTime().toUtc().toIso8601String());
  // }

  // final jsonDates = jsonEncode(dateObjects);

  // await ApiService()
  //     .submitTime(jsonDates, http.Client(), pref.getString('token'));
  // await pref.setString('week_key', '');

  // print("sending work day");
  // String jsonDates = jsonEncode(dateObjects);
  // await ApiService()
  //     .submitTime(jsonDates, http.Client(), prefs.getString('token'));
  // print('hi');
  // print(jsonDates);
  // }

  //Map for months
  var month = {
    '1': 'January',
    '2': 'February',
    '3': 'March',
    '4': 'April',
    '5': 'May',
    '6': 'June',
    '7': 'July',
    '8': 'August',
    '9': 'September',
    '10': 'October',
    '11': 'November',
    '12': 'December',
  };

  @override
  void initState() {
    super.initState();
    getThemeColor(httpClient);
    getTimesheet();
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  void getThemeColor(http.Client httpClient) async {
    prefs = await SharedPreferences.getInstance();
    final jsonString1 = await ApiService()
        .getAccountInfo(http.Client(), prefs.getString('token'));
    if (jsonString1.isNotEmpty) {
      final jsonData1 = jsonDecode(jsonString1);
      themeColor = jsonData1['ThemeColor'];
    }
    if (themeColor == 0) {
      headerColor = MyColors.buttonColorsGradiant1;
      gradientColor1 = MyColors.buttonColorsGradiant1;
      gradientColor2 = MyColors.buttonColorsGradiant2;
      animationColor = MyColors.buttonColorsGradiant1;
    } else if (themeColor == 1) {
      headerColor = MyColors.customPink;
      gradientColor1 = MyColors.buttonColorsGradient3;
      gradientColor2 = MyColors.buttonColorsGradient4;
      animationColor = MyColors.featurePageMainColor1;
    } else if (themeColor == 2) {
      headerColor = MyColors.customGreen;
      gradientColor1 = MyColors.buttonColorsGradient5;
      gradientColor2 = MyColors.buttonColorsGradient6;
      animationColor = MyColors.featurePageMainColor2;
    } else if (themeColor == 3) {
      headerColor = MyColors.customOrange;
      gradientColor1 = MyColors.buttonColorsGradient7;
      gradientColor2 = MyColors.buttonColorsGradient8;
      animationColor = MyColors.featurePageMainColor3;
    }
    setState(() {});
  }

  refresh() {
    setState(() {});
  }

  addPeriod() {
    period.add(AddPeriod(notifyParent: refresh));
  }

  DateTime previousSunday() {
    final DateTime now = DateTime.now();
    final int daysToSunday = now.weekday % 7 == 0 ? 0 : now.weekday;
    return now.subtract(Duration(days: daysToSunday));
  }

  DateTime upcomingSaturday() {
    final DateTime now = DateTime.now();
    final int daysToSaturday = DateTime.saturday - now.weekday;
    return now.add(Duration(days: daysToSaturday));
  }

  List<DateTime> generateActiveDates() {
    final DateTime start = previousSunday();
    final DateTime end = DateTime
        .now(); // Change this line to use the current day as the end date
    final List<DateTime> activeDates = [];
    for (int i = 0; i <= end.difference(start).inDays; i++) {
      activeDates.add(start.add(Duration(days: i)));
    }
    return activeDates;
  }

  Future<List> getTimesheet() async {
    setState(() {});
    pref = await SharedPreferences.getInstance();
    // print('currentDate in getTimesheet');
    // print(currentDate);
    // final adjustedDate = currentDate.add(const Duration(days: 1));
    // print('getting prefs');
    // print(prefs.getString('week_key'));

    // print('got prefs');
    final timeSheetArray = await ApiService()
        .getTimeSheetData(http.Client(), currentDate, pref.getString('token'));
    // if (pref.getString('week_key') == null ||
    //     pref.getString('week_key') == '') {
    // print(timeSheetArray[1]);
    // print('current date');
    // print(currentDate);
    // print('blah')
    dateObjects = [];
    for (var i = 0; i < timeSheetArray.length; i++) {
      for (var j = 0; j < timeSheetArray[i].length; j++) {
        // print('hello');
        // print(timeSheetArray[i][j]['date']);
        final date = timeSheetArray[i][j]['Date'];
        //print(timeSheetArray[i][j]['startTime']);
        final startTime = timeSheetArray[i][j]['StartTime'];
        final endTime = timeSheetArray[i][j]['EndTime'];
        final dateObject = TimeSheetWeekData(date, startTime, endTime);
        // print(dateObject.getStartTime());
        dateObjects.add(dateObject);
      }
    }
    // print('in function');

    // for (var i = 0; i < dateObjects.length; i++) {
    //   print(
    //       'Date: ${dateObjects[i].date},
    //Start Time: ${dateObjects[i].startTime},
    //End Time: ${dateObjects[i].endTime}');
    // }
    //   final String encodedDateObjects = encode(dateObjects);
    //   await pref.setString('week_key', encodedDateObjects);

    //   print('encoded');
    //   print(encodedDateObjects);
    //   print('encoded done');
    // } else {
    //   final String? decodedDateObjects = pref.getString('week_key');
    //   dateObjects = decode(decodedDateObjects!);
    // }

    if (mounted) {
      setState(() {});
    }

    return dateObjects;
  }

  @override
  Widget build(BuildContext context) {
    // DateTime testDate = nextWeek();
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          key: const Key('submitTimesheetAppBar'),
          title: const Text('Submit Timesheet', key: Key('title')),
          backgroundColor: headerColor,
          titleTextStyle: const TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.w500,
            fontSize: 20,
            fontFamily: 'Montserrat',
          ),
        ),
        resizeToAvoidBottomInset: false,
        backgroundColor: Colors.white,
        body: SingleChildScrollView(
          scrollDirection: Axis.vertical,
          child: SizedBox(
            child: Column(
              children: <Widget>[
                const SizedBox(
                  height: 20,
                ),
                Row(children: [
                  IconButton(
                    key: const Key('backButton'),
                    icon: const Icon(Icons.arrow_back),
                    onPressed: () {
                      Navigator.pop(context);
                    },
                  ),
                  Text(
                    "${month[currentMonth.toString()]!} $currentDay",
                    key: const Key('monthDay'),
                    textAlign: TextAlign.left,
                    style: const TextStyle(
                        fontFamily: 'Montserrat',
                        fontSize: 24,
                        color: Color(0xff262626),
                        fontWeight: FontWeight.w400),
                  ),
                ]),
                SizedBox(
                  key: const Key('dateSlider'),
                  height: 100,
                  width: 400,
                  child: DatePicker(
                    previousSunday(),
                    controller: _controller,
                    initialSelectedDate: DateTime.now(),
                    //initialSelectedDate: testDate,
                    selectionColor: gradientColor1,
                    selectedTextColor: Colors.white,
                    activeDates: generateActiveDates(),
                    onDateChange: (date) {
                      // print('currentDate in widget');
                      // print(currentDate);
                      setState(() {
                        currentDate = date;
                        // print('hi');
                        // print(currentDate);
                        currentMonth = currentDate.month;
                        currentDay = currentDate.day;
                        currentYear = currentDate.year;
                        getTimesheet();
                        if (period.isNotEmpty) {
                          period.clear();
                        }
                        editTimeInterval = false;
                      });
                    },
                  ),
                ),
                TimeIntervalWidget(
                  key: const Key("timeIntervalWidget"),
                  timeInterval: dateObjects,
                ),
                ListView.builder(
                    itemCount: period.length,
                    scrollDirection: Axis.vertical,
                    shrinkWrap: true,
                    padding: const EdgeInsets.all(5),
                    itemBuilder: (_, index) => period[index]),
                TextButton(
                    key: const Key("addNewPeriod"),
                    style: TextButton.styleFrom(
                      foregroundColor: gradientColor1,
                      disabledForegroundColor: Colors.grey.withOpacity(0.38),
                    ),
                    onPressed: () {
                      setState(() {
                        addPeriod();
                      });
                    },
                    child: const Text('Add Time Period')),
                Padding(
                  padding: const EdgeInsets.only(top: 10),
                  child: ElevatedButton(
                    key: const Key("submitTimesheet"),
                    onPressed: () {
                      final FocusScopeNode currentFocus =
                          FocusScope.of(context);

                      if (!currentFocus.hasPrimaryFocus) {
                        currentFocus.unfocus();
                      }
                      submitTime();
                      Future.delayed(const Duration(milliseconds: 500), () {
                        showDialog(
                            barrierDismissible: false,
                            context: context,
                            builder: (BuildContext context) {
                              return Stack(
                                children: [
                                  const AlertDialog(),
                                  Center(
                                    child: SizedBox(
                                      height: 100,
                                      width: 100,
                                      child: AnimatedCheck(
                                        animationColor: animationColor,
                                      ),
                                    ),
                                  ),
                                ],
                              );
                            });
                      });
                      Future.delayed(const Duration(seconds: 2), () {
                        Navigator.pop(context);
                        Navigator.pop(context);
                      });
                    },
                    style: ElevatedButton.styleFrom(
                        padding: EdgeInsets.zero,
                        shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(15))),
                    child: Ink(
                      decoration: BoxDecoration(
                          gradient: LinearGradient(
                              colors: [gradientColor1, gradientColor2]),
                          borderRadius: BorderRadius.circular(15)),
                      child: Container(
                        width: 200,
                        height: 50,
                        alignment: Alignment.center,
                        child: const Text(
                          'Submit Timesheet',
                          style: TextStyle(
                              fontSize: 18,
                              fontFamily: 'Montserrat',
                              fontWeight: FontWeight.w700),
                        ),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class TimeIntervalWidget extends StatefulWidget {
  static List? timeInterval;
  // ignore: avoid_unused_constructor_parameters
  const TimeIntervalWidget({Key? key, required timeInterval}) : super(key: key);

  @override
  State<TimeIntervalWidget> createState() => _TimeIntervalWidget();
}

class _TimeIntervalWidget extends State<TimeIntervalWidget> {
  List selectedTime = dateObjects;

  @override
  void initState() {
    super.initState();
    _initializeTimeInterval(TimeIntervalWidget.timeInterval);
  }

  // void _editTimeInterval() {
  //   setState(() {
  //     if (editTimeInterval) {
  //       editTimeInterval = false;
  //     } else {
  //       editTimeInterval = true;
  //     }
  //   });
  // }

  int editIndex = -1; // Add this line

  void _editTimeInterval(int index) {
    setState(() {
      if (editTimeInterval && editIndex == index) {
        editTimeInterval = false;
        editIndex = -1;
      } else {
        editTimeInterval = true;
        editIndex = index;
      }
    });
  }

  void addTimeIntervals(
      List<String> timeIntervals, DateTime startTime, DateTime endTime) {
    final interval = startTime.hour.toString() +
        ":" +
        startTime.minute.toString().padLeft(2, '0') +
        " - " +
        endTime.hour.toString() +
        ":" +
        endTime.minute.toString().padLeft(2, '0');
    timeIntervals.add(interval);
  }

  void _initializeTimeInterval(List? dataObjects) {
    if (dataObjects != null) {
      for (int i = 0; i < dataObjects.length; i++) {
        // print(dataObjects[i].date);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final List<String> timeIntervals = [];
    final List<DateTime> startTimes = [];
    final List<DateTime> endTimes = [];
    currentWorkDay = [];
    for (int i = 0; i < dateObjects.length; i++) {
      if (dateObjects[i].getDate().day == currentDate.day) {
        currentWorkDay.add(dateObjects[i]);
        startTime = dateObjects[i].getStartTime().toLocal();
        endTime = dateObjects[i].getEndTime().toLocal();
        startTimes.add(startTime);
        endTimes.add(endTime);
        workDate = dateObjects[i].getDate().toLocal();
        final interval = startTime.hour.toString() +
            ":" +
            startTime.minute.toString().padLeft(2, '0') +
            " - " +
            endTime.hour.toString() +
            ":" +
            endTime.minute.toString().padLeft(2, '0');
        timeIntervals.add(interval);
      }
    }

    // if (!editTimeInterval) {
    return Column(
      children: [
        ListView.builder(
            key: const Key("currentHours"),
            shrinkWrap: true,
            scrollDirection: Axis.vertical,
            itemCount: timeIntervals.length,
            padding: const EdgeInsets.all(10),
            itemBuilder: (BuildContext ctxt, index) {
              if (!editTimeInterval || index != editIndex) {
                return Stack(
                  children: <Widget>[
                    Padding(
                      padding: const EdgeInsets.all(20),
                      child: Container(
                        key: const Key("hoursContainer"),
                        alignment: Alignment.centerLeft,
                        width: 370,
                        height: 70,
                        color: const Color.fromRGBO(232, 232, 232, 1),
                        child: Padding(
                          padding: const EdgeInsets.only(left: 20),
                          child: Text(
                            key: const Key("loggedHours"),
                            timeIntervals[index],
                            style: const TextStyle(
                              color: Colors.black,
                              fontSize: 20.0,
                              fontFamily: 'Montserrat',
                            ),
                          ),
                        ),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(
                          left: 250, top: 35, right: 35, bottom: 35),
                      child: IconButton(
                        key: const Key("edit"),
                        icon: const Image(
                          image: AssetImage("assets/images/icons/edit.png"),
                          fit: BoxFit.scaleDown,
                          alignment: Alignment.centerRight,
                          width: 50,
                          height: 50,
                        ),
                        onPressed: () {
                          _editTimeInterval(index);
                          // print(timeIntervals[index]);
                        },
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(
                          left: 300, top: 30, right: 30, bottom: 30),
                      child: IconButton(
                        key: const Key("delete"),
                        icon: const Image(
                          image: AssetImage("assets/images/icons/delete.png"),
                          fit: BoxFit.scaleDown,
                          alignment: Alignment.centerRight,
                          width: 50,
                          height: 50,
                        ),
                        onPressed: () {
                          setState(() {
                            for (int i = 0; i < currentWorkDay.length; i++) {
                              if (i == index) {
                                dateObjects.remove(currentWorkDay[i]);
                              }
                            }
                            submitTime();
                            // final String encodedDateObjects =
                            //     encode(dateObjects);
                            // pref.setString('week_key', encodedDateObjects);
                          });
                        },
                      ),
                    ),
                  ],
                );
                // }),
                // ],
                // );
              } else {
                // return ListView.builder(
                //     key: const Key("editHours"),
                //     shrinkWrap: true,
                //     itemCount: timeIntervals.length,
                //     scrollDirection: Axis.vertical,
                //     padding: const EdgeInsets.all(10),
                //     itemBuilder: (BuildContext ctxt, index) {
                return Stack(
                  children: <Widget>[
                    Padding(
                      padding: const EdgeInsets.all(20),
                      child: TimeIntervalPicker(
                          key: const Key("timePicker"),
                          endLimit: endTimes[index],
                          startLimit: startTimes[index],
                          onChanged: (DateTime? newStartTime,
                              DateTime? newEndTime, bool isAllDay) {
                            if (newStartTime == null || newEndTime == null) {
                              "Null Values";
                            } else {
                              //print(currentDate.toString());
                              //print('Second print');
                              final editStartTime = DateTime(
                                  currentDate.year,
                                  currentDate.month,
                                  currentDate.day,
                                  newStartTime.hour,
                                  newStartTime.minute);
                              final editEndTime = DateTime(
                                  currentDate.year,
                                  currentDate.month,
                                  currentDate.day,
                                  newEndTime.hour,
                                  newEndTime.minute);

                              final currentDateUtc =
                                  currentDate.toUtc().toIso8601String();
                              final editStartTimeUTC =
                                  editStartTime.toUtc().toIso8601String();
                              final editEndTimeUTC =
                                  editEndTime.toUtc().toIso8601String();
                              final newDateObject = TimeSheetWeekData(
                                  currentDateUtc.toString(),
                                  editStartTimeUTC,
                                  editEndTimeUTC);
                              setState(() {
                                int workDayIndex = 0;
                                for (int i = 0;
                                    i < currentWorkDay.length;
                                    i++) {
                                  workDayIndex =
                                      dateObjects.indexOf(currentWorkDay[i]);
                                  if (i == index) {
                                    dateObjects.remove(currentWorkDay[i]);
                                  }
                                }

                                if (workDayIndex != 0) {
                                  dateObjects.insert(
                                      workDayIndex, newDateObject);
                                  // print(encode(dateObjects));

                                  // final String encodedDateObjects =
                                  //     encode(dateObjects);
                                  // pref.setString(
                                  //     'week_key', encodedDateObjects);
                                } else {
                                  dateObjects.insert(
                                      workDayIndex, newDateObject);
                                  //print(encode(dateObjects));

                                  // final String encodedDateObjects =
                                  //     encode(dateObjects);
                                  // pref.setString(
                                  //     'week_key', encodedDateObjects);
                                }
                              });
                            }
                          }),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 55, left: 10),
                      child: TextButton(
                          key: const Key("save"),
                          style: TextButton.styleFrom(
                            foregroundColor: gradientColor1,
                            disabledForegroundColor:
                                Colors.grey.withOpacity(0.38),
                          ),
                          onPressed: () {
                            setState(() {
                              encode(dateObjects);
                              submitTime();
                              // print(index);
                              // print(dateObjects);
                              _editTimeInterval(index);
                            });
                          },
                          child: const Text('Save')),
                    ),
                  ],
                );
              }
            }),
      ],
    );
  }
}

class TimeSheetWeekData {
  String date;
  String startTime;
  String endTime;

  TimeSheetWeekData(this.date, this.startTime, this.endTime);
  Map toJson() => {'Date': date, 'StartTime': startTime, 'EndTime': endTime};
  DateTime getDate() {
    return DateTime.parse(date);
  }

  DateTime getStartTime() {
    return DateTime.parse(startTime);
  }

  DateTime getEndTime() {
    return DateTime.parse(endTime);
  }

  void setDate(String date) {
    this.date = date;
  }

  void setStartTime(String startTime) {
    this.startTime = startTime;
  }

  void setEndTime(String endTime) {
    this.endTime = endTime;
  }
}

class AddPeriod extends StatefulWidget {
  final Function() notifyParent;
  const AddPeriod({Key? key, required this.notifyParent}) : super(key: key);

  @override
  State<AddPeriod> createState() => _AddPeriodState();
}

class _AddPeriodState extends State<AddPeriod> {
  @override
  Widget build(BuildContext context) {
    currentWorkDay = [];
    bool startTimeSelected = false;
    bool endTimeSelected = false;

    for (int i = 0; i < dateObjects.length; i++) {
      if (dateObjects[i].getDate().day == currentDate.day) {
        currentWorkDay.add(dateObjects[i]);
        startTime = dateObjects[i].getStartTime();
        endTime = dateObjects[i].getEndTime();
        workDate = dateObjects[i].getDate();
      }
    }

    return SingleChildScrollView(
      scrollDirection: Axis.vertical,
      child: Column(
        children: [
          SizedBox(
            child: ListView.builder(
                key: const Key("addHours"),
                shrinkWrap: true,
                scrollDirection: Axis.vertical,
                itemCount: period.length,
                padding: const EdgeInsets.all(10),
                itemBuilder: (BuildContext ctxt, int index) {
                  return Stack(
                    children: <Widget>[
                      Padding(
                        padding: const EdgeInsets.all(20),
                        child: StatefulBuilder(
                            builder: (BuildContext context, setState) {
                          return TimeIntervalPicker(
                              key: const Key("addPeriod"),
                              endLimit: null,
                              startLimit: null,
                              onChanged: (DateTime? nStartTime,
                                  DateTime? nEndTime, bool isAllDay) {
                                if (nStartTime == null || nEndTime == null) {
                                  "Null Values";
                                } else {
                                  if (!startTimeSelected) {
                                    startTimeSelected = true;
                                  } else if (!endTimeSelected) {
                                    endTimeSelected = true;
                                  }
                                  if (startTimeSelected && endTimeSelected) {
                                    final addStartTime = DateTime(
                                        currentDate.year,
                                        currentDate.month,
                                        currentDate.day,
                                        nStartTime.hour,
                                        nStartTime.minute);
                                    final addEndTime = DateTime(
                                        currentDate.year,
                                        currentDate.month,
                                        currentDate.day,
                                        nEndTime.hour,
                                        nEndTime.minute);

                                    // print(workDate);
                                    // print('blah');
                                    final addedDateObject = TimeSheetWeekData(
                                        currentDate.toString(),
                                        addStartTime.toString(),
                                        addEndTime.toString());
                                    // int nworkDayIndex = 0;
                                    setState(() {
                                      dateObjects.add(addedDateObject);

                                      // final String encodedDateObjects =
                                      //     encode(dateObjects);
                                      // pref.setString(
                                      //     'week_key', encodedDateObjects);
                                      // print('else');
                                      // }

                                      // print(addedDateObject.getEndTime());
                                    });
                                    startTimeSelected = false;
                                    endTimeSelected = false;
                                  }
                                }
                              });
                        }),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 55, left: 10),
                        child: TextButton(
                            key: const Key("add"),
                            style: TextButton.styleFrom(
                              foregroundColor: gradientColor1,
                              disabledForegroundColor:
                                  Colors.grey.withOpacity(0.38),
                            ),
                            onPressed: () {
                              setState(() {
                                encode(dateObjects);
                                submitTime();
                                removePeriod();
                                widget.notifyParent();
                              });
                            },
                            child: const Text('Add Period')),
                      ),
                    ],
                  );
                }),
          ),
        ],
      ),
    );
  }
}
