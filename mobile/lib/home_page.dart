import 'package:flutter/material.dart';
import 'themes/my_clipper.dart';
import 'themes/colors.dart';
import 'package:http/http.dart' as http;
import 'profile_page.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'utils/api_service.dart';
import 'dart:convert';
import 'utils/calendar_utils.dart';
import 'package:intl/intl.dart';

class HomePage extends StatefulWidget {
  static http.Client? httpClient;
  static DateTime? focusedDay;

  const HomePage({Key? key}) : super(key: key);

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  //Variables
  http.Client httpClient = http.Client();
  final double profileRadius = 60;
  late SharedPreferences prefs;
  ProfileInfo profileInfo = ProfileInfo(' ', ' ', ' ', ' ', ' ', ' ', 0, 0);
  String firstName = " ";
  String lastName = " ";
  late List<Event> upcomingShifts = <Event>[];
  int themeColor = 0;
  Color headerColor = Colors.white;

  @override
  void initState() {
    createUpcomingShifts(HomePage.focusedDay ?? DateTime.now());
    setClient(HomePage.httpClient);
    getProfileInfo(httpClient);
    super.initState();
  }

  void createUpcomingShifts(DateTime currentDay) {
    if (kEvents.isEmpty) {
      upcomingShifts = <Event>[];
      return;
    }
    final now = currentDay;
    upcomingShifts = getEventsForRange(now, now.add(const Duration(days: 7)));
  }

  List<Event> getEventsForRange(DateTime start, DateTime end) {
    // Implementation example
    final days = daysInRange(start, end);

    return [
      for (final d in days) ...getEventsForDay(d),
    ];
  }

  List<Event> getEventsForDay(DateTime day) {
    // Implementation example
    return kEvents[day] ?? [];
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  void getProfileInfo(http.Client client) async {
    setState(() {});
    prefs = await SharedPreferences.getInstance();

    final jsonString =
        await ApiService().getAccountInfo(client, prefs.getString('token'));

    if (jsonString.isNotEmpty) {
      final jsonData = jsonDecode(jsonString);

      themeColor = prefs.getInt('theme')!;

      if (themeColor == 0) {
        headerColor = MyColors.customBlue;
      } else if (themeColor == 1) {
        headerColor = MyColors.customPink;
      } else if (themeColor == 2) {
        headerColor = MyColors.customGreen;
      } else if (themeColor == 3) {
        headerColor = MyColors.customOrange;
      }

      profileInfo = ProfileInfo(
          jsonData['Id'],
          jsonData['Name'],
          jsonData['Email'],
          jsonData['Password'],
          jsonData['Address'],
          jsonData['Birthday'],
          jsonData['PhoneNumber'],
          jsonData['ThemeColor']);
      final arr = jsonData['Name'].toString().split(' ');
      firstName = arr[0];
      lastName = arr.length > 1 ? arr[1] : "";
    }

    if (mounted) {
      setState(() {});
    }
  }

  var positionText = RichText(
    key: const Key('positionText'),
    textAlign: TextAlign.center,
    text: const TextSpan(
      // Note: Styles for TextSpans must be explicitly defined.
      // Child text spans will inherit styles from parent
      style: TextStyle(
          fontFamily: 'Montserrat',
          fontSize: 16,
          color: MyColors.homePageContainerText,
          fontWeight: FontWeight.w500),
      children: <TextSpan>[
        TextSpan(text: 'Sales Associate at \n'),
        TextSpan(
            text: 'Mode Durgaa',
            style: TextStyle(color: MyColors.calendarPositionColor)),
      ],
    ),
  );

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          ClipPath(
            clipper: HomePageSemicircle(),
            child: Opacity(
              opacity: 0.3,
              child: Container(
                color: headerColor,
              ),
            ),
          ),
          Positioned(
              height: MediaQuery.of(context).size.height,
              top: MediaQuery.of(context).size.height * 0.06,
              left: 20,
              child: Column(
                children: [
                  SizedBox(
                    height: MediaQuery.of(context).size.height * 0.135,
                    child: Row(children: [
                      profilePicture(),
                      const SizedBox(
                        width: 10,
                      ),
                      SizedBox(
                        width: MediaQuery.of(context).size.width * 0.6,
                        child: Text(
                          "$firstName, $lastName",
                          style: const TextStyle(
                              fontFamily: 'Montserrat',
                              fontSize: 22,
                              color: Color(0xff262626),
                              fontWeight: FontWeight.w500),
                        ),
                      ),
                    ]),
                  ),
                  SizedBox(
                    height: MediaQuery.of(context).size.height * 0.08,
                  ),
                  Container(
                    height: MediaQuery.of(context).size.height * 0.10,
                    width: MediaQuery.of(context).size.width * 0.8,
                    decoration: BoxDecoration(
                      border: Border.all(
                        color: MyColors.homePageContainerOutline,
                      ),
                      borderRadius: BorderRadius.circular(15),
                      color: MyColors.homePageContainerBackground,
                    ),
                    child: Padding(
                        padding: const EdgeInsets.all(20),
                        child: Center(child: positionText)),
                  ),
                  SizedBox(
                    height: MediaQuery.of(context).size.height * 0.02,
                  ),
                  Container(
                    width: MediaQuery.of(context).size.width * 0.8,
                    height: MediaQuery.of(context).size.height * 0.35,
                    decoration: BoxDecoration(
                      border: Border.all(
                        color: MyColors.homePageContainerOutline,
                      ),
                      borderRadius: BorderRadius.circular(15),
                      color: MyColors.homePageContainerBackground,
                    ),
                    child: Padding(
                        padding: const EdgeInsets.all(15),
                        child: SingleChildScrollView(
                            child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                              const Text(
                                'Upcoming shifts',
                                style: TextStyle(
                                    fontFamily: 'Montserrat',
                                    fontSize: 16,
                                    color: MyColors.calendarPositionColor,
                                    fontWeight: FontWeight.w600),
                              ),
                              SizedBox(
                                height:
                                    MediaQuery.of(context).size.height * 0.29,
                                child: ListView.builder(
                                  scrollDirection: Axis.vertical,
                                  itemCount: upcomingShifts.length,
                                  itemBuilder: (context, index) {
                                    return Container(
                                        key: const Key(
                                            "upcomingShiftsContainerKey"),
                                        margin: const EdgeInsets.symmetric(
                                          horizontal: 4.0,
                                          vertical: 4.0,
                                        ),
                                        child: SizedBox(
                                            width: MediaQuery.of(context)
                                                    .size
                                                    .width *
                                                0.5,
                                            child: ListTile(
                                              leading: Container(
                                                decoration: BoxDecoration(
                                                    color: const Color.fromARGB(
                                                        255, 255, 255, 255),
                                                    shape: BoxShape.circle,
                                                    border: Border.all(
                                                        width: 3.0,
                                                        color: upcomingShifts[
                                                                index]
                                                            .categoryColor)),
                                                width: 10.0,
                                                height: 10.0,
                                                margin: const EdgeInsets.only(
                                                    top: 5),
                                              ),
                                              title: Text(
                                                  '${DateFormat('EEE d').format(upcomingShifts[index].startDate)}, ${upcomingShifts[index].getStringTime()}',
                                                  style: const TextStyle(
                                                      fontSize: 15,
                                                      fontWeight:
                                                          FontWeight.w900,
                                                      color: MyColors
                                                          .calendarTimeColor)),
                                              subtitle: Column(
                                                crossAxisAlignment:
                                                    CrossAxisAlignment.start,
                                                mainAxisAlignment:
                                                    MainAxisAlignment.start,
                                                children: <Widget>[
                                                  const Text("JobTitle",
                                                      style: TextStyle(
                                                          fontSize: 12,
                                                          fontWeight:
                                                              FontWeight.w600,
                                                          fontFamily:
                                                              'Montserrat',
                                                          color: MyColors
                                                              .calendarPositionColor)),
                                                  Text(
                                                      'Break time allowed:'
                                                      '${upcomingShifts[index].breakTime.toString()} hour',
                                                      style: const TextStyle(
                                                          fontSize: 12,
                                                          fontWeight:
                                                              FontWeight.w400,
                                                          fontFamily:
                                                              'Montserrat',
                                                          color: MyColors
                                                              .calendarPositionColor))
                                                ],
                                              ),
                                            )));
                                  },
                                ),
                              ),
                            ]))),
                  ),
                  SizedBox(
                    height: MediaQuery.of(context).size.height * 0.02,
                  ),
                  Container(
                    height: MediaQuery.of(context).size.height * 0.1,
                    width: MediaQuery.of(context).size.width * 0.8,
                    decoration: BoxDecoration(
                      border: Border.all(
                        color: MyColors.homePageContainerOutline,
                      ),
                      borderRadius: BorderRadius.circular(15),
                      color: MyColors.homePageContainerBackground,
                    ),
                    child: Padding(
                        padding: const EdgeInsets.all(15),
                        child: SingleChildScrollView(
                            child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                              const Text(
                                'Reminders',
                                style: TextStyle(
                                    fontFamily: 'Montserrat',
                                    fontSize: 16,
                                    color: MyColors.calendarPositionColor,
                                    fontWeight: FontWeight.w600),
                              ),
                              const SizedBox(
                                height: 5,
                              ),
                              Row(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: [
                                  Icon(
                                      key: const Key("ReminderIconKey"),
                                      color: headerColor,
                                      const IconData(
                                        0xf02a1,
                                        fontFamily: 'MaterialIcons',
                                      )),
                                  const SizedBox(
                                    width: 5,
                                  ),
                                  const Text(
                                    'Log your hours for the day ',
                                    style: TextStyle(
                                        fontFamily: 'Montserrat',
                                        fontSize: 15,
                                        color: MyColors.calendarPositionColor,
                                        fontWeight: FontWeight.w500),
                                  )
                                ],
                              )
                            ]))),
                  ),
                ],
              ))
        ],
      ),
    );
  }

  Widget profilePicture() => CircleAvatar(
        radius: profileRadius,
        backgroundColor: Colors.white,
        child: CircleAvatar(
          radius: profileRadius - 5,
          backgroundImage: const AssetImage('assets/images/Reema.jpg'),
        ),
      );
}
