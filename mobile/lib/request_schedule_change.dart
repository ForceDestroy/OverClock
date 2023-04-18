import 'package:flutter/material.dart';
import 'package:mobile/profile_page.dart';
import 'package:mobile/utils/api_service.dart';
import 'themes/colors.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:date_picker_timeline/date_picker_timeline.dart';
import 'package:mobile/themes/check_mark_animation.dart';
import 'dart:async';

//Create the Log Hours Page
class ScheduleChange extends StatefulWidget {
  const ScheduleChange({Key? key}) : super(key: key);
  static http.Client? httpClient;

  @override
  State<ScheduleChange> createState() => _ScheduleChangeState();
}

class _ScheduleChangeState extends State<ScheduleChange> {
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

  int count = 0;

  DateTime currentDate = DateTime.now();
  var currentMonth = DateTime.now().month;
  var currentDay = DateTime.now().day;
  var currentYear = DateTime.now().year;
  TextEditingController requestController = TextEditingController();
  var numItems = 0;
  var displayedSchedule = [];
  var timeInterval = {'StartTime': " ", 'EndTime': " ", 'Position': " "};
  http.Client httpClient = http.Client();
  int requestID = 0;
  var fromId = " ";
  var position = " ";
  String request = '';
  String type = 'Schedule';

  int phoneNumber = 12345;
  DateTime startTime = DateTime.now();
  DateTime endTime = DateTime.now();
  var sendRequestInfo = {
    'FromId': " ",
    'Date': DateTime.now().toIso8601String(),
    'StartTime': " ",
    'EndTime': " ",
    'Body': " ",
    'Type': " "
  };
  Color headerColor = Colors.white;
  Color gradientColor1 = Colors.white;
  Color gradientColor2 = Colors.white;
  Color animationColor = Colors.white;

  ProfileInfo profileInfo = ProfileInfo(' ', ' ', ' ', ' ', ' ', ' ', 0, 0);
  //Initialize method that runs before rendering the screen
  @override
  void initState() {
    super.initState();
    setClient(ScheduleChange.httpClient);
    getThemeColor(httpClient);
    getSchedule(httpClient);
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  void submitRequest() async {
    prefs = await SharedPreferences.getInstance();
    // print('sending request');
    request = requestController.text;
    sendRequestInfo['Type'] = type;
    sendRequestInfo['Body'] = request;
    // print(sendRequestInfo);

    final body = jsonEncode(sendRequestInfo);

    await ApiService()
        .scheduleChange(body, http.Client(), prefs.getString('token'));
  }

  void getSchedule(http.Client client) async {
    prefs = await SharedPreferences.getInstance();
    final jsonString =
        await ApiService().getAllSchedule(client, prefs.getString('token'));
    var jsonData = [];

    if (jsonString.isNotEmpty) {
      jsonData = jsonDecode(jsonString);
    }
    // print(jsonData);
    displayedSchedule.clear();
    numItems = 0;
    count = 0;
    // ignore: unused_local_variable
    for (var e in jsonData) {
      timeInterval['Position'] = position;
      if (currentDate.day == DateTime.parse(jsonData[count]['Date']).day &&
          currentDate.month == DateTime.parse(jsonData[count]['Date']).month &&
          currentDate.year == DateTime.parse(jsonData[count]['Date']).year) {
        var temp1 = jsonData[count]['StartTime'].toString().split('T');
        timeInterval['StartTime'] = temp1[1];
        if (temp1[1].contains('-')) {
          temp1 = temp1[1].split('-');
          timeInterval['StartTime'] = temp1[0];
        }
        var temp2 = jsonData[count]['EndTime'].toString().split('T');
        timeInterval['EndTime'] = temp2[1];
        if (temp2[1].contains('-')) {
          temp2 = temp2[1].split('-');
          timeInterval['EndTime'] = temp2[0];
        }
        displayedSchedule.add(timeInterval);
        sendRequestInfo['StartTime'] =
            DateTime.parse(jsonData[count]['StartTime']).toIso8601String();
        sendRequestInfo['EndTime'] =
            DateTime.parse(jsonData[count]['EndTime']).toIso8601String();
        numItems++;
      }
      count++;
    }
    setState(() {});
  }

  //Function that fetches the theme color
  void getThemeColor(http.Client client) async {
    prefs = await SharedPreferences.getInstance();
    final jsonString1 =
        await ApiService().getAccountInfo(client, prefs.getString('token'));
    if (jsonString1.isNotEmpty) {
      final jsonData1 = jsonDecode(jsonString1);
      themeColor = jsonData1['ThemeColor'];
      if (jsonData1['Position'] != null) {
        position = jsonData1['Position'];
      }
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
          title: const Text(
            'Request Schedule Change',
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
        resizeToAvoidBottomInset: false,
        backgroundColor: Colors.white,
        body: Column(
          children: [
            SingleChildScrollView(
              scrollDirection: Axis.vertical,
              child: SizedBox(
                child: Column(children: <Widget>[
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
                          "${month[currentMonth.toString()]!} $currentYear",
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
                  ),
                  SizedBox(
                    key: const Key('dateSlider'),
                    height: 100,
                    width: 400,
                    child: DatePicker(
                      DateTime.now(),

                      initialSelectedDate: currentDate,
                      // DateTime.parse("2022-11-28T00:00:00"),
                      // initialSelectedDate: DateTime.parse("2022-11-28T00:00:00"),

                      selectionColor: gradientColor1,
                      selectedTextColor: Colors.white,
                      onDateChange: (date) {
                        // New date selected
                        setState(() {
                          currentDate = date;
                          getSchedule(httpClient);
                          sendRequestInfo['StartTime'] =
                              currentDate.toIso8601String();
                          sendRequestInfo['EndTime'] =
                              currentDate.toIso8601String();
                          currentMonth = currentDate.month;
                          currentDay = currentDate.day;
                          currentYear = currentDate.year;

                          // print('blah');
                          // print(currentDate);
                        });
                      },
                    ),
                  ),
                  Column(
                    children: [
                      ListView.builder(
                          key: const Key("currentHours"),
                          shrinkWrap: true,
                          itemCount: numItems,
                          padding: const EdgeInsets.all(10),
                          itemBuilder: (BuildContext ctxt, index) {
                            // print(index);
                            // print('this is index');

                            return Padding(
                              padding: const EdgeInsets.all(16.0),
                              child: Card(
                                elevation: 1,
                                shape: RoundedRectangleBorder(
                                  side: BorderSide(
                                    color: gradientColor1,
                                  ),
                                  borderRadius: const BorderRadius.all(
                                      Radius.circular(12)),
                                ),
                                child: SizedBox(
                                  width: 300,
                                  height: 80,
                                  child: Center(
                                      child: Text(
                                          (displayedSchedule[numItems - 1]
                                                  ['Position'] +
                                              "\n\n" +
                                              displayedSchedule[numItems - 1]
                                                  ['StartTime'] +
                                              " - " +
                                              displayedSchedule[numItems - 1]
                                                  ['EndTime']))),
                                ),
                              ),
                            );
                          }),
                      const Text(
                        key: Key('reason'),
                        'Enter why you would need a schedule change:',
                        style: TextStyle(
                            fontFamily: 'Montserrat',
                            fontSize: 12,
                            color: MyColors.black,
                            fontWeight: FontWeight.bold),
                      ),
                      Padding(
                        key: const Key('txtFieldPadding'),
                        padding: const EdgeInsets.only(top: 10),
                        child: SizedBox(
                          key: const Key('txtFieldBox'),
                          width: 330.0,
                          child: TextField(
                            key: const Key('reasonTxtField'),
                            controller: requestController,
                            obscureText: false,
                            maxLines: 1,
                            decoration: InputDecoration(
                              contentPadding: const EdgeInsets.symmetric(
                                  vertical: 35.0, horizontal: 20),
                              fillColor: MyColors.fillColorInputFields,
                              filled: true,
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(10.0),
                                borderSide: const BorderSide(
                                  width: 1,
                                  color: MyColors.inputFieldsOutline,
                                ),
                              ),
                              labelText: 'Reason',
                            ),
                          ),
                        ),
                      ),
                    ],
                  )
                ]),
              ),
            ),
            Expanded(
              child: Align(
                alignment: Alignment.bottomCenter,
                child: Padding(
                  key: const Key("scheduleChangePad"),
                  padding: const EdgeInsets.all(48.0),
                  child: ElevatedButton(
                    key: const Key("scheduleChange"),
                    onPressed: () {
                      final FocusScopeNode currentFocus =
                          FocusScope.of(context);

                      if (!currentFocus.hasPrimaryFocus) {
                        currentFocus.unfocus();
                      }
                      submitRequest();
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
                          'Submit Request',
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
