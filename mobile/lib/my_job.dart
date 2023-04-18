import 'package:flutter/material.dart';
import 'package:mobile/payslip_page.dart';
import 'package:mobile/request_schedule_change.dart';
import 'package:mobile/time_off_request.dart';
import 'package:mobile/utils/api_service.dart';
import 'package:mobile/submit_timesheet.dart';
import 'custom_request.dart';
import 'log_hours.dart';
import 'themes/colors.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';
import 'utils/payslip_utils.dart';

//Create the Profile Page
class MyJob extends StatefulWidget {
  static http.Client? httpClient;
  const MyJob({Key? key}) : super(key: key);

  @override
  State<MyJob> createState() => _MyJobState();
}

class _MyJobState extends State<MyJob> {
  //Variables
  final double coverHeight = 250;
  final double profileRadius = 80;
  int themeColor = 0;
  late SharedPreferences prefs;
  late List<Payslip> payslips = [];
  http.Client httpClient = http.Client();
  Map<String, double> trainingData = {
    "Complete": 8,
    "Incomplete": 2,
  };
  final gradientList = <List<Color>>[
    [
      MyColors.buttonColorsGradiant2,
      MyColors.buttonColorsGradiant1,
    ],
    [
      const Color.fromRGBO(129, 182, 205, 1),
      const Color.fromRGBO(91, 253, 199, 1),
    ],
  ];

  Color headerColor = Colors.white;
  Color gradientColor1 = Colors.white;
  Color gradientColor2 = Colors.white;

  @override
  void initState() {
    super.initState();
    setClient(MyJob.httpClient);
    getProfileInfo(httpClient);
    getPaySlipInfo();
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  void getPaySlipInfo() async {
    prefs = await SharedPreferences.getInstance();

    final jsonString =
        await ApiService().getPayslips(http.Client(), prefs.getString('token'));
    payslips = createPayslips(jsonString[1]);
  }

  void getProfileInfo(http.Client client) async {
    prefs = await SharedPreferences.getInstance();

    await ApiService().getAccountInfo(client, prefs.getString('token'));

    themeColor = prefs.getInt('theme')!;

    if (themeColor == 0) {
      headerColor = MyColors.customBlue;
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

  //What is rendered on the screen
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        body: SingleChildScrollView(
          scrollDirection: Axis.vertical,
          child: Column(
            children: <Widget>[
              Stack(
                clipBehavior: Clip.none,
                alignment: Alignment.center,
                children: [
                  ClipPath(
                    key: const Key('customShape'),
                    clipper: Semicircle(),
                    child: Opacity(
                      opacity: 0.3,
                      child: Container(
                        color: headerColor,
                        height: coverHeight,
                      ),
                    ),
                  ),
                  const Positioned(
                    top: 30,
                    child: Text(
                      "Mode Durgaa",
                      key: Key('pageTitle'),
                      style: TextStyle(
                          fontFamily: 'Montserrat',
                          fontSize: 40,
                          color: Color(0xff262626),
                          fontWeight: FontWeight.w400),
                    ),
                  ),
                  Positioned(
                    top: 55,
                    child: Container(
                        margin: EdgeInsets.only(
                            bottom: MediaQuery.of(context).size.height * 0.038),
                        width: MediaQuery.of(context).size.width * 0.30,
                        height: MediaQuery.of(context).size.height * 0.30,
                        child: Image.asset('assets/images/SareeImage.png')),
                  )
                ],
              ),
              const Padding(
                padding: EdgeInsets.only(left: 5.0, right: 5.0),
                child: Row(
                  children: [
                    Expanded(
                      child: Card(
                        elevation: 1,
                        shape: RoundedRectangleBorder(
                          side: BorderSide(
                            color: Colors.black,
                          ),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          height: 225,
                          child: Padding(
                            padding: EdgeInsets.all(16.0),
                            child: SingleChildScrollView(
                              child: Column(children: [
                                Text(
                                  "Company Information",
                                  style: TextStyle(
                                      fontFamily: 'Montserrat',
                                      fontSize: 18,
                                      color: Color(0xff262626),
                                      fontWeight: FontWeight.w400),
                                ),
                                Row(
                                  children: [
                                    Expanded(
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            "\nAddress",
                                            style: TextStyle(
                                                fontFamily: 'Montserrat',
                                                fontSize: 15,
                                                color: Color(0xff262626),
                                                fontWeight: FontWeight.w800),
                                          ),
                                          Text(
                                            "4787 boul. des Sources\nPierrefonds, QC\nH8Y 1S3",
                                            style: TextStyle(
                                                fontFamily: 'Montserrat',
                                                fontSize: 15,
                                                color: Color(0xff262626),
                                                fontWeight: FontWeight.w400),
                                          ),
                                          Text(
                                            "\nContact",
                                            style: TextStyle(
                                                fontFamily: 'Montserrat',
                                                fontSize: 15,
                                                color: Color(0xff262626),
                                                fontWeight: FontWeight.w800),
                                          ),
                                          Text(
                                            "(514) 685 0329\nmodedurgaa@gmail.com",
                                            style: TextStyle(
                                                fontFamily: 'Montserrat',
                                                fontSize: 15,
                                                color: Color(0xff262626),
                                                fontWeight: FontWeight.w400),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                              ]),
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(left: 5.0, right: 5.0),
                child: Row(
                  children: [
                    Expanded(
                      child: Card(
                        elevation: 1,
                        shape: const RoundedRectangleBorder(
                          side: BorderSide(
                            color: Colors.black,
                          ),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          height: 150,
                          child: Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Column(children: [
                              const Center(
                                child: Text(
                                  "Log your daily hours",
                                  style: TextStyle(
                                      fontFamily: 'Montserrat',
                                      fontSize: 18,
                                      color: Color(0xff262626),
                                      fontWeight: FontWeight.w400),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.all(10.0),
                                child: ElevatedButton(
                                  key: const Key('logHoursButton'),
                                  onPressed: () {
                                    Navigator.push(
                                      context,
                                      MaterialPageRoute(
                                          builder: (context) => LogHours(
                                              anotherDate: DateTime.now())),
                                    );
                                  },
                                  style: ElevatedButton.styleFrom(
                                      padding: EdgeInsets.zero,
                                      shape: RoundedRectangleBorder(
                                          borderRadius:
                                              BorderRadius.circular(15))),
                                  child: Ink(
                                    decoration: BoxDecoration(
                                        gradient: LinearGradient(colors: [
                                          gradientColor1,
                                          gradientColor2
                                        ]),
                                        borderRadius:
                                            BorderRadius.circular(15)),
                                    child: Container(
                                      width: 200,
                                      height: 50,
                                      alignment: Alignment.center,
                                      child: const Text(
                                        'Log Hours',
                                        style: TextStyle(
                                            fontSize: 12,
                                            fontFamily: 'Montserrat',
                                            fontWeight: FontWeight.w700),
                                      ),
                                    ),
                                  ),
                                ),
                              ),
                            ]),
                          ),
                        ),
                      ),
                    ),
                    Expanded(
                      child: Card(
                        elevation: 1,
                        shape: const RoundedRectangleBorder(
                          side: BorderSide(
                            color: Colors.black,
                          ),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          height: 150,
                          child: Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: SingleChildScrollView(
                              scrollDirection: Axis.vertical,
                              child: Column(children: [
                                const Center(
                                  child: Text(
                                    "Request a schedule change",
                                    style: TextStyle(
                                        fontFamily: 'Montserrat',
                                        fontSize: 18,
                                        color: Color(0xff262626),
                                        fontWeight: FontWeight.w400),
                                  ),
                                ),
                                Padding(
                                  padding: const EdgeInsets.all(10.0),
                                  child: ElevatedButton(
                                    key: const Key('scheduleChangeButton'),
                                    onPressed: () {
                                      Navigator.of(context).push(
                                          MaterialPageRoute(
                                              builder: (context) =>
                                                  const ScheduleChange()));
                                    },
                                    style: ElevatedButton.styleFrom(
                                        padding: EdgeInsets.zero,
                                        shape: RoundedRectangleBorder(
                                            borderRadius:
                                                BorderRadius.circular(15))),
                                    child: Ink(
                                      decoration: BoxDecoration(
                                          gradient: LinearGradient(colors: [
                                            gradientColor1,
                                            gradientColor2
                                          ]),
                                          borderRadius:
                                              BorderRadius.circular(15)),
                                      child: Container(
                                        width: 200,
                                        height: 50,
                                        alignment: Alignment.center,
                                        child: const Text(
                                          'Send Request',
                                          style: TextStyle(
                                              fontSize: 12,
                                              fontFamily: 'Montserrat',
                                              fontWeight: FontWeight.w700),
                                        ),
                                      ),
                                    ),
                                  ),
                                ),
                              ]),
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(left: 5.0, right: 5.0),
                child: Row(
                  children: [
                    Expanded(
                      child: Card(
                        elevation: 1,
                        shape: const RoundedRectangleBorder(
                          side: BorderSide(
                            color: Colors.black,
                          ),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          height: 150,
                          child: Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: SingleChildScrollView(
                              scrollDirection: Axis.vertical,
                              child: Column(children: [
                                const Center(
                                  child: Text(
                                    "Submit your weekly timesheet",
                                    style: TextStyle(
                                        fontFamily: 'Montserrat',
                                        fontSize: 18,
                                        color: Color(0xff262626),
                                        fontWeight: FontWeight.w400),
                                  ),
                                ),
                                Padding(
                                  padding: const EdgeInsets.all(10.0),
                                  child: ElevatedButton(
                                    key: const Key('weeklyTimesheetButton'),
                                    onPressed: () {
                                      Navigator.push(
                                        context,
                                        MaterialPageRoute(
                                            builder: (context) =>
                                                const TimeSheet()),
                                      );
                                    },
                                    style: ElevatedButton.styleFrom(
                                        padding: EdgeInsets.zero,
                                        shape: RoundedRectangleBorder(
                                            borderRadius:
                                                BorderRadius.circular(15))),
                                    child: Ink(
                                      decoration: BoxDecoration(
                                          gradient: LinearGradient(colors: [
                                            gradientColor1,
                                            gradientColor2
                                          ]),
                                          borderRadius:
                                              BorderRadius.circular(15)),
                                      child: Container(
                                        width: 200,
                                        height: 50,
                                        alignment: Alignment.center,
                                        child: const Text(
                                          'Submit Timesheet',
                                          style: TextStyle(
                                              fontSize: 12,
                                              fontFamily: 'Montserrat',
                                              fontWeight: FontWeight.w700),
                                        ),
                                      ),
                                    ),
                                  ),
                                ),
                              ]),
                            ),
                          ),
                        ),
                      ),
                    ),
                    Expanded(
                      child: Card(
                        elevation: 1,
                        shape: const RoundedRectangleBorder(
                          side: BorderSide(
                            // color: MyColors.buttonColorsGradiant1,
                            color: Colors.black,
                          ),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          height: 150,
                          child: Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Column(children: [
                              const Center(
                                child: Text(
                                  "Request time off from work",
                                  style: TextStyle(
                                      fontFamily: 'Montserrat',
                                      fontSize: 18,
                                      color: Color(0xff262626),
                                      fontWeight: FontWeight.w400),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.all(10.0),
                                child: ElevatedButton(
                                  key: const Key('requestTimeOffButton'),
                                  onPressed: () {
                                    Navigator.push(
                                      context,
                                      MaterialPageRoute(
                                          builder: (context) => TimeOffRequest(
                                              anotherDate: DateTime.now())),
                                    );
                                  },
                                  style: ElevatedButton.styleFrom(
                                      padding: EdgeInsets.zero,
                                      shape: RoundedRectangleBorder(
                                          borderRadius:
                                              BorderRadius.circular(15))),
                                  child: Ink(
                                    decoration: BoxDecoration(
                                        gradient: LinearGradient(colors: [
                                          gradientColor1,
                                          gradientColor2
                                        ]),
                                        borderRadius:
                                            BorderRadius.circular(15)),
                                    child: Container(
                                      width: 200,
                                      height: 50,
                                      alignment: Alignment.center,
                                      child: const Text(
                                        'Request Time Off',
                                        style: TextStyle(
                                            fontSize: 12,
                                            fontFamily: 'Montserrat',
                                            fontWeight: FontWeight.w700),
                                      ),
                                    ),
                                  ),
                                ),
                              ),
                            ]),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(left: 5.0, right: 5.0),
                child: Row(
                  children: [
                    Expanded(
                      child: Card(
                        elevation: 1,
                        shape: const RoundedRectangleBorder(
                          side: BorderSide(
                            color: Colors.black,
                          ),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          height: 150,
                          child: Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Column(children: [
                              const Center(
                                child: Text(
                                  "Access your payslips",
                                  style: TextStyle(
                                      fontFamily: 'Montserrat',
                                      fontSize: 18,
                                      color: Color(0xff262626),
                                      fontWeight: FontWeight.w400),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.all(10.0),
                                child: ElevatedButton(
                                  key: const Key('payslipsButton'),
                                  onPressed: () {
                                    Navigator.push(
                                      context,
                                      MaterialPageRoute(
                                          builder: (context) => PayslipPage(
                                                payslips: payslips,
                                                theme: themeColor,
                                              )),
                                    );
                                  },
                                  style: ElevatedButton.styleFrom(
                                      padding: EdgeInsets.zero,
                                      shape: RoundedRectangleBorder(
                                          borderRadius:
                                              BorderRadius.circular(15))),
                                  child: Ink(
                                    decoration: BoxDecoration(
                                        gradient: LinearGradient(colors: [
                                          gradientColor1,
                                          gradientColor2
                                        ]),
                                        borderRadius:
                                            BorderRadius.circular(15)),
                                    child: Container(
                                      width: 200,
                                      height: 50,
                                      alignment: Alignment.center,
                                      child: const Text(
                                        'Access Payslips',
                                        style: TextStyle(
                                            fontSize: 12,
                                            fontFamily: 'Montserrat',
                                            fontWeight: FontWeight.w700),
                                      ),
                                    ),
                                  ),
                                ),
                              ),
                            ]),
                          ),
                        ),
                      ),
                    ),
                    Expanded(
                      child: Card(
                        elevation: 1,
                        shape: const RoundedRectangleBorder(
                          side: BorderSide(),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                        child: SizedBox(
                          height: 150,
                          child: Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: Column(children: [
                              const Center(
                                child: Text(
                                  "Send custom requests",
                                  style: TextStyle(
                                      fontFamily: 'Montserrat',
                                      fontSize: 18,
                                      color: Color(0xff262626),
                                      fontWeight: FontWeight.w400),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.all(10.0),
                                child: ElevatedButton(
                                  key: const Key('customRequestsButton'),
                                  onPressed: () {
                                    Navigator.of(context).push(
                                        MaterialPageRoute(
                                            builder: (context) =>
                                                const CustomRequest()));
                                  },
                                  style: ElevatedButton.styleFrom(
                                      padding: EdgeInsets.zero,
                                      shape: RoundedRectangleBorder(
                                          borderRadius:
                                              BorderRadius.circular(15))),
                                  child: Ink(
                                    decoration: BoxDecoration(
                                        gradient: LinearGradient(colors: [
                                          gradientColor1,
                                          gradientColor2
                                        ]),
                                        borderRadius:
                                            BorderRadius.circular(15)),
                                    child: Container(
                                      width: 200,
                                      height: 50,
                                      alignment: Alignment.center,
                                      child: const Text(
                                        'Send Request',
                                        style: TextStyle(
                                            fontSize: 12,
                                            fontFamily: 'Montserrat',
                                            fontWeight: FontWeight.w700),
                                      ),
                                    ),
                                  ),
                                ),
                              ),
                            ]),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

//Class that creates custom shape for profile header
class Semicircle extends CustomClipper<Path> {
  @override
  Path getClip(Size size) {
    final double w = size.width;
    final double h = size.height;

    final path = Path();

    path.lineTo(0, h - 100);
    path.quadraticBezierTo(
      w * 0.5,
      h,
      w,
      h - 100,
    );
    path.lineTo(w, 0);
    path.close();

    return path;
  }

  @override
  bool shouldReclip(CustomClipper<Path> oldClipper) {
    return false;
  }
}
