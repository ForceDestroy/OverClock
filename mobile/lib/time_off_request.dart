// ignore_for_file: lines_longer_than_80_chars, duplicate_ignore
import 'package:flutter/material.dart';
import 'package:mobile/utils/api_service.dart';
import 'themes/colors.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:date_picker_timeline/date_picker_timeline.dart';
import 'package:mobile/themes/check_mark_animation.dart';
import 'dart:async';

//Create the Time Off Request Page
class TimeOffRequest extends StatefulWidget {
  final DateTime anotherDate;
  static http.Client? httpClient;
  const TimeOffRequest({Key? key, required this.anotherDate}) : super(key: key);

  @override
  State<TimeOffRequest> createState() => _TimeOffRequestState();
}

class _TimeOffRequestState extends State<TimeOffRequest> {
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
  //Variables
  final double coverHeight = 250;
  int themeColor = 0;
  late SharedPreferences prefs;
  DateTime selectedDate = DateTime.now();
  var selectedMonth = DateTime.now().month;
  var selectedYear = DateTime.now().year;
  http.Client httpClient = http.Client();
  int count = 0;
  var numItems = 0;
  var displayedSchedule = [];
  var timeInterval = {'startTime': " ", 'endTime': " ", 'position': " "};
  var sendRequestInfo = {
    'date': DateTime.now().toIso8601String(),
    'startTime': " ",
    'endTime': " ",
    'body': " ",
    'type': " "
  };
  var requestType = "Nothing";
  bool requestInitiated = false;
  var position = " ";
  Color headerColor = Colors.white;
  Color gradientColor1 = Colors.white;
  Color gradientColor2 = Colors.white;
  Color animationColor = Colors.white;

  //Initialize method that runs before rendering the screen
  @override
  void initState() {
    super.initState();
    setClient(TimeOffRequest.httpClient);
    getThemeColor(httpClient);
    setSelectedDate();
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  //Function to set selecteddate
  void setSelectedDate() async {
    selectedDate = widget.anotherDate;
    await getSchedule(httpClient);
  }

//Function that fetches the schedule for a selected date
  Future<bool> getSchedule(http.Client client) async {
    prefs = await SharedPreferences.getInstance();

    final jsonString =
        await ApiService().getAllSchedule(client, prefs.getString('token'));
    var jsonData = [];
    if (jsonString.isNotEmpty) {
      jsonData = jsonDecode(jsonString);
    }

    final jsonString1 = await ApiService()
        .getAccountInfo(http.Client(), prefs.getString('token'));
    if (jsonString1.isNotEmpty) {
      final jsonData1 = jsonDecode(jsonString1);
      position = jsonData1['Position'];
    }

    displayedSchedule.clear();
    numItems = 0;
    count = 0;
    // ignore: unused_local_variable
    for (var e in jsonData) {
      timeInterval['position'] = position;
      if (selectedDate == DateTime.parse(jsonData[count]['Date'])) {
        var temp1 = jsonData[count]['StartTime'].toString().split('T');
        timeInterval['startTime'] = temp1[1];
        if (temp1[1].contains('-')) {
          temp1 = temp1[1].split('-');
          timeInterval['startTime'] = temp1[0];
        }
        temp1 = timeInterval['startTime']!.split(':');
        timeInterval['startTime'] = "${temp1[0]}:${temp1[1]}";
        var temp2 = jsonData[count]['EndTime'].toString().split('T');
        timeInterval['endTime'] = temp2[1];
        if (temp2[1].contains('-')) {
          temp2 = temp2[1].split('-');
          timeInterval['endTime'] = temp2[0];
        }
        temp2 = timeInterval['endTime']!.split(':');
        timeInterval['endTime'] = "${temp2[0]}:${temp2[1]}";
        sendRequestInfo['startTime'] =
            DateTime.parse(jsonData[count]['StartTime']).toIso8601String();
        sendRequestInfo['endTime'] =
            DateTime.parse(jsonData[count]['EndTime']).toIso8601String();
        displayedSchedule.add(timeInterval);
        numItems++;
      }
      count++;
    }
    setState(() {});
    return true;
  }

  //Function to request time off
  void sendRequest() async {
    final body = jsonEncode(sendRequestInfo);
    await ApiService()
        .timeOffRequest(body, http.Client(), prefs.getString('token'));
  }

//Function that fetches the theme color
  void getThemeColor(http.Client client) async {
    prefs = await SharedPreferences.getInstance();
    final jsonString =
        await ApiService().getAccountInfo(client, prefs.getString('token'));
    if (jsonString.isNotEmpty) {
      final jsonData = jsonDecode(jsonString);
      themeColor = jsonData['ThemeColor'];
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

  //What is rendered on the screen
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          key: const Key('appBar'),
          title: const Text('Request Time Off', key: Key('title')),
          backgroundColor: headerColor,
          titleTextStyle: const TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.w500,
            fontSize: 20,
            fontFamily: 'Montserrat',
          ),
        ),
        body: Column(
          children: <Widget>[
            SingleChildScrollView(
              scrollDirection: Axis.vertical,
              child: SizedBox(
                child: Column(
                  children: <Widget>[
                    const SizedBox(
                      height: 20,
                    ),
                    SingleChildScrollView(
                      scrollDirection: Axis.horizontal,
                      padding: const EdgeInsets.only(right: 220),
                      child: Row(
                        children: [
                          IconButton(
                            key: const Key('backButton'),
                            icon: const Icon(Icons.arrow_back),
                            onPressed: () {
                              Navigator.pop(context);
                            },
                          ),
                          Text(
                            "${month[selectedMonth.toString()]!} $selectedYear",
                            key: const Key('monthYear'),
                            textAlign: TextAlign.left,
                            style: const TextStyle(
                                fontFamily: 'Montserrat',
                                fontSize: 24,
                                color: Color(0xff262626),
                                fontWeight: FontWeight.w400),
                          )
                        ],
                      ),
                    ),
                    // ignore: duplicate_ignore
                    SizedBox(
                      key: const Key('slidingCalendar'),
                      height: 100,
                      width: 400,
                      // ignore: lines_longer_than_80_chars
                      child: DatePicker(
                        DateTime.now(),
                        //DateTime.parse("2022-11-28T00:00:00"),
                        initialSelectedDate: selectedDate,
                        //initialSelectedDate: DateTime.parse("2022-11-28T00:00:00"),
                        selectionColor: gradientColor1,
                        selectedTextColor: Colors.white,
                        onDateChange: (date) {
                          // New date selected
                          setState(() {
                            selectedDate = date;
                            sendRequestInfo['startTime'] =
                                selectedDate.toIso8601String();
                            sendRequestInfo['endTime'] =
                                selectedDate.toIso8601String();
                            getSchedule(httpClient);
                            selectedMonth = selectedDate.month;
                            selectedYear = selectedDate.year;
                          });
                        },
                      ),
                    ),
                  ],
                ),
              ),
            ),
            SizedBox(
              child: ListView.builder(
                  scrollDirection: Axis.vertical,
                  shrinkWrap: true,
                  padding: const EdgeInsets.all(5),
                  itemCount: numItems,
                  itemBuilder: (BuildContext ctxt, int index) {
                    return Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Card(
                        elevation: 1,
                        shape: RoundedRectangleBorder(
                          side: BorderSide(
                            color: gradientColor1,
                          ),
                          borderRadius:
                              const BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          width: 300,
                          height: 80,
                          child: Padding(
                              padding: const EdgeInsets.all(16.0),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: <Widget>[
                                  Text(
                                    displayedSchedule[numItems - 1]['position'],
                                    style: const TextStyle(
                                        fontWeight: FontWeight.bold),
                                  ),
                                  Text(
                                      // ignore: lines_longer_than_80_chars, prefer_interpolation_to_compose_strings
                                      "${"\n" + displayedSchedule[numItems - 1]['startTime']} - " +
                                          displayedSchedule[numItems - 1]
                                              ['endTime']),
                                ],
                              )),
                        ),
                      ),
                    );
                  }),
            ),
            Column(
              children: <Widget>[
                RadioListTile(
                  title: const Text('Sick'),
                  key: const Key('sick'),
                  value: "Sick",
                  groupValue: requestType,
                  onChanged: (value) {
                    setState(() {
                      requestType = "Sick";
                      requestInitiated = true;
                      sendRequestInfo['type'] = requestType;
                    });
                  },
                ),
                RadioListTile(
                  title: const Text('Vacation'),
                  key: const Key('vacation'),
                  value: "Vacation",
                  groupValue: requestType,
                  onChanged: (value) {
                    setState(() {
                      requestType = "Vacation";
                      requestInitiated = true;
                      sendRequestInfo['type'] = requestType;
                    });
                  },
                ),
                RadioListTile(
                  title: const Text('Personal'),
                  key: const Key('personal'),
                  value: "Personal",
                  groupValue: requestType,
                  onChanged: (value) {
                    setState(() {
                      requestType = "Personal";
                      requestInitiated = true;
                      sendRequestInfo['type'] = requestType;
                    });
                  },
                ),
              ],
            ),
            Expanded(
              child: Align(
                alignment: Alignment.bottomCenter,
                child: Padding(
                  key: const Key("timeOffRequestPadding"),
                  padding: const EdgeInsets.all(48.0),
                  child: ElevatedButton(
                    key: const Key("timeOffRequestButton"),
                    onPressed: () {
                      if (requestInitiated) {
                        sendRequest();
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
                      }
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
                          'Send Request',
                          style: TextStyle(
                              fontSize: 18,
                              fontFamily: 'Montserrat',
                              fontWeight: FontWeight.w700),
                        ),
                      ),
                    ),
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
