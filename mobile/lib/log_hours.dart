import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:mobile/themes/colors.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mobile/utils/api_service.dart';
import 'nav_bar.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/log_hours_submission.dart';
import 'package:date_picker_timeline/date_picker_timeline.dart';

//Create the Log Hours Page
class LogHours extends StatefulWidget {
  final DateTime anotherDate;
  static http.Client? httpClient;
  const LogHours({Key? key, required this.anotherDate}) : super(key: key);

  @override
  State<LogHours> createState() => _LogHoursState();
}

class _LogHoursState extends State<LogHours> {
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
  var modifiedData = [];
  int count = 0;
  var timeInterval = {'startTime': " ", 'endTime': " "};
  bool canLog = true;
  http.Client httpClient = http.Client();
  Color headerColor = Colors.white;
  Color gradientColor1 = Colors.white;
  Color gradientColor2 = Colors.white;

  //Initialize method that runs before rendering the screen
  @override
  void initState() {
    super.initState();
    setClient(LogHours.httpClient);
    getThemeColor(httpClient);
    setSelectedDate();
    getLoggedHours(httpClient);
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  //Function to set selecteddate
  void setSelectedDate() {
    selectedDate = widget.anotherDate;
    getLoggedHours(httpClient);
  }

  //Function that fetches the theme colo
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

  //Function that fetches logged hours for a selected date
  void getLoggedHours(http.Client client) async {
    canLog = true;
    prefs = await SharedPreferences.getInstance();
    int anotherCount = 0;
    final jsonString = await ApiService()
        .getHours(client, selectedDate, prefs.getString('token'));

    var jsonData = [];

    if (jsonString.isNotEmpty) {
      jsonData = jsonDecode(jsonString);
      count = jsonData.length;
    }

    if (count > 0) {
      canLog = false;
    }
    modifiedData.clear();
    // ignore: unused_local_variable
    for (var e in jsonData) {
      var temp1 = jsonData[anotherCount]['StartTime'].toString().split('T');
      timeInterval['startTime'] = temp1[1];
      if (temp1[1].contains('-')) {
        temp1 = temp1[1].split('-');
        timeInterval['startTime'] = temp1[0];
      }
      temp1 = timeInterval['startTime']!.split(':');
      timeInterval['startTime'] = "${temp1[0]}:${temp1[1]}";
      var temp2 = jsonData[anotherCount]['EndTime'].toString().split('T');
      timeInterval['endTime'] = temp2[1];
      if (temp2[1].contains('-')) {
        temp2 = temp2[1].split('-');
        timeInterval['endTime'] = temp2[0];
      }
      temp2 = timeInterval['endTime']!.split(':');
      timeInterval['endTime'] = "${temp2[0]}:${temp2[1]}";
      modifiedData.add(Map.from(timeInterval));
      anotherCount++;
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
          title: const Text(
            'Log Hours',
            key: Key('title'),
          ),
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
                const SizedBox(
                  height: 20,
                ),
                Row(
                  children: [
                    IconButton(
                      key: const Key('backButton'),
                      icon: const Icon(Icons.arrow_back),
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => const NavBar(navIndex: 2)),
                        );
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
                    ),
                  ],
                ),
              ]),
            ),
            SizedBox(
              key: const Key('slidingCalendar'),
              height: 100,
              width: 400,
              child: DatePicker(
                DateTime.now().subtract(const Duration(days: 5)),
                initialSelectedDate: selectedDate,
                selectionColor: gradientColor1,
                selectedTextColor: Colors.white,
                activeDates: [
                  DateTime.now(),
                  DateTime.now().subtract(const Duration(days: 1)),
                  DateTime.now().subtract(const Duration(days: 2)),
                  DateTime.now().subtract(const Duration(days: 3)),
                  DateTime.now().subtract(const Duration(days: 4)),
                  DateTime.now().subtract(const Duration(days: 5)),
                ],
                onDateChange: (date) {
                  // New date selected
                  setState(() {
                    selectedDate = date;
                    getLoggedHours(httpClient);
                    selectedMonth = selectedDate.month;
                    selectedYear = selectedDate.year;
                  });
                },
              ),
            ),
            Expanded(
              child: ListView.builder(
                  itemCount: count,
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
                          child: Center(
                              child: Text(modifiedData[index]['startTime'] +
                                  " - " +
                                  modifiedData[index]['endTime'])),
                        ),
                      ),
                    );
                  }),
            ),
            Padding(
              key: const Key('logHoursButton'),
              padding: const EdgeInsets.all(18.0),
              child: Align(
                alignment: Alignment.centerRight,
                child: CircleAvatar(
                  radius: 30,
                  backgroundColor: gradientColor1, //<-- SEE HERE
                  child: IconButton(
                    icon: const Icon(
                      key: Key('logHoursIconButton'),
                      Icons.add,
                      color: Colors.white,
                    ),
                    onPressed: () {
                      if (canLog) {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) =>
                                  LogHoursSubmission(rawDate: selectedDate)),
                        );
                      }
                    },
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
