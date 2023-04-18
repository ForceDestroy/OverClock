// ignore_for_file: lines_longer_than_80_chars

import 'package:flutter/material.dart';
import 'package:mobile/log_hours.dart';
import 'package:mobile/utils/api_service.dart';
import 'themes/colors.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:time_interval_picker/time_interval_picker.dart';

//Create the Log Hours Page
class LogHoursSubmission extends StatefulWidget {
  final DateTime rawDate;
  static http.Client? httpClient;
  const LogHoursSubmission({Key? key, required this.rawDate}) : super(key: key);

  @override
  State<LogHoursSubmission> createState() => _LogHoursSubmissionState();
}

class _LogHoursSubmissionState extends State<LogHoursSubmission> {
  //What will get called before rendering the screen
  @override
  void initState() {
    super.initState();
    setFullDate();
    setClient(LogHoursSubmission.httpClient);
    getThemeColor(httpClient);
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  //Variables
  final double coverHeight = 250;
  int themeColor = 0;
  late SharedPreferences prefs;
  int count = 1;
  var fullDate = " ";
  var loggedHours = [];
  var timeInterval = {
    'Date': DateTime.now().toIso8601String(),
    'StartTime': DateTime.now().toIso8601String(),
    'EndTime': DateTime.now().toIso8601String()
  };
  Color headerColor = Colors.white;
  Color gradientColor1 = Colors.white;
  Color gradientColor2 = Colors.white;
  http.Client httpClient = http.Client();

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

  //Function to set full date
  void setFullDate() {
    timeInterval['Date'] = widget.rawDate.toIso8601String();
    fullDate =
        "${month[widget.rawDate.month.toString()]!} ${widget.rawDate.day}, ${widget.rawDate.year}";
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
    } else if (themeColor == 1) {
      headerColor = MyColors.customPink;
      gradientColor1 = MyColors.buttonColorsGradient3;
      gradientColor2 = MyColors.buttonColorsGradient4;
    } else if (themeColor == 2) {
      headerColor = MyColors.customGreen;
      gradientColor1 = MyColors.buttonColorsGradient5;
      gradientColor2 = MyColors.buttonColorsGradient6;
    } else if (themeColor == 3) {
      headerColor = MyColors.customOrange;
      gradientColor1 = MyColors.buttonColorsGradient7;
      gradientColor2 = MyColors.buttonColorsGradient8;
    }
    setState(() {});
  }

  //Function to log hours
  void submitLog() async {
    final body = jsonEncode(loggedHours);
    await ApiService().logHours(body, http.Client(), prefs.getString('token'));
  }

  //What is displayed on the screen
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          key: const Key('appBar'),
          title: const Text('Log Hours', key: Key('title')),
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
              child: Column(children: <Widget>[
                Row(
                  children: [
                    IconButton(
                      key: const Key("backButton"),
                      icon: const Icon(Icons.arrow_back),
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) =>
                                  LogHours(anotherDate: widget.rawDate)),
                        );
                      },
                    ),
                    Text(
                      fullDate,
                      textAlign: TextAlign.left,
                      style: const TextStyle(
                          fontFamily: 'Montserrat',
                          fontSize: 24,
                          color: Color(0xff262626),
                          fontWeight: FontWeight.w400),
                    ),
                  ],
                ),
              ]),
            ),
            Flexible(
                key: const Key("timePicker"),
                fit: FlexFit.loose,
                child: ListView.builder(
                    itemCount: count,
                    itemBuilder: (BuildContext ctxt, int index) {
                      return Padding(
                        padding: const EdgeInsets.all(16.0),
                        child: TimeIntervalPicker(
                          endLimit: null,
                          startLimit: null,
                          onChanged: (DateTime? startTime, DateTime? endTime,
                              bool isAllDay) {
                            if (startTime == null || endTime == null) {
                              "Null Values";
                            } else {
                              final DateTime selectedD =
                                  DateTime.parse(timeInterval['Date']!);
                              timeInterval['StartTime'] = DateTime(
                                      selectedD.year,
                                      selectedD.month,
                                      selectedD.day,
                                      startTime.hour,
                                      startTime.minute)
                                  .toUtc()
                                  .toIso8601String();
                              timeInterval['EndTime'] = DateTime(
                                      selectedD.year,
                                      selectedD.month,
                                      selectedD.day,
                                      endTime.hour,
                                      endTime.minute)
                                  .toUtc()
                                  .toIso8601String();
                              timeInterval['Date'] =
                                  selectedD.toUtc().toIso8601String();
                            }
                            if (loggedHours.length < count) {
                              final anotherTimeVariable = {
                                'Date': timeInterval['Date'],
                                'StartTime': timeInterval['StartTime'],
                                'EndTime': timeInterval['EndTime']
                              };
                              loggedHours.add(anotherTimeVariable);
                            } else {
                              loggedHours[index]['StartTime'] =
                                  timeInterval['StartTime'];
                              loggedHours[index]['EndTime'] =
                                  timeInterval['EndTime'];
                            }
                          },
                        ),
                      );
                    })),
            TextButton(
              key: const Key("timePeriod"),
              style: ButtonStyle(
                foregroundColor:
                    MaterialStateProperty.all<Color>(gradientColor1),
              ),
              onPressed: () {
                count++;
                setState(() {});
              },
              child: const Text('Add Time Period'),
            ),
            Padding(
              key: const Key("logHoursButton"),
              padding: const EdgeInsets.all(16.0),
              child: ElevatedButton(
                onPressed: () {
                  submitLog();
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: (context) =>
                            LogHours(anotherDate: widget.rawDate)),
                  );
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
                      'Log Hours',
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
    );
  }
}
