import 'package:flutter/material.dart';
import 'package:mobile/my_job.dart';
import 'package:mobile/profile_page.dart';
import 'calendar.dart';
import 'package:mobile/themes/colors.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;
import 'utils/api_service.dart';
import 'utils/payslip_utils.dart';
import 'package:mobile/home_page.dart';

class NavBar extends StatefulWidget {
  static http.Client? httpClient;
  final int navIndex;
  const NavBar({super.key, required this.navIndex});
  @override
  State<NavBar> createState() => _NavBarState();
}

class _NavBarState extends State<NavBar> {
  @override
  void initState() {
    super.initState();
    setIndex();
    getProfileInfo(httpClient);
    WidgetsBinding.instance.addPostFrameCallback((_) {
      pageController.jumpToPage(_selectedIndex);
    });
  }

//Variables
  PageController pageController = PageController();
  late SharedPreferences prefs;
  late List<Payslip> payslips = [];
  int _selectedIndex = 0;
  http.Client httpClient = http.Client();
  int themeColor = 0;
  Color navBarColor = Colors.white;
  String homeIconPath = "assets/images/icons/home.png";
  String calendarIconPath = "assets/images/icons/calendar.png";
  String jobIconPath = "assets/images/icons/job.png";
  String profileIconPath = "assets/images/icons/profile.png";

  void onTapped(int index) async {
    setState(() {
      _selectedIndex = index;
    });
    if (index == 2) {
      prefs = await SharedPreferences.getInstance();

      final jsonString = await ApiService()
          .getPayslips(http.Client(), prefs.getString('token'));
      payslips = createPayslips(jsonString[1]);
    }
    pageController.jumpToPage(index);
  }

  void setIndex() {
    _selectedIndex = widget.navIndex;
  }

  void getProfileInfo(http.Client client) async {
    prefs = await SharedPreferences.getInstance();

    await ApiService().getAccountInfo(client, prefs.getString('token'));

    themeColor = prefs.getInt('theme')!;

    if (themeColor == 0) {
      navBarColor = MyColors.buttonColorsGradiant1;
      homeIconPath = "assets/images/icons/home.png";
      calendarIconPath = "assets/images/icons/calendar.png";
      jobIconPath = "assets/images/icons/job.png";
      profileIconPath = "assets/images/icons/profile.png";
    } else if (themeColor == 1) {
      navBarColor = MyColors.buttonColorsGradient3;
      homeIconPath = "assets/images/icons/pinkHome.png";
      calendarIconPath = "assets/images/icons/pinkCalendar.png";
      jobIconPath = "assets/images/icons/pinkJob.png";
      profileIconPath = "assets/images/icons/pinkProfile.png";
    } else if (themeColor == 2) {
      navBarColor = MyColors.buttonColorsGradient5;
      homeIconPath = "assets/images/icons/greenHome.png";
      calendarIconPath = "assets/images/icons/greenCalendar.png";
      jobIconPath = "assets/images/icons/greenJob.png";
      profileIconPath = "assets/images/icons/greenProfile.png";
    } else if (themeColor == 3) {
      navBarColor = MyColors.buttonColorsGradient7;
      homeIconPath = "assets/images/icons/orangeHome.png";
      calendarIconPath = "assets/images/icons/orangeCalendar.png";
      jobIconPath = "assets/images/icons/orangeJob.png";
      profileIconPath = "assets/images/icons/orangeProfile.png";
    }

    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: PageView(
        controller: pageController,
        children: const [
          HomePage(),
          Calendar(),
          MyJob(),
          ProfilePage(),
        ],
      ),
      bottomNavigationBar: Container(
        decoration: const BoxDecoration(
          boxShadow: <BoxShadow>[
            BoxShadow(
                color: Color.fromRGBO(255, 255, 255, 1),
                offset: Offset(0.0, 0.75))
          ],
        ),
        child: Column(mainAxisSize: MainAxisSize.min, children: [
          Row(
            children: List.generate(
                4,
                (index) => Expanded(
                      child: Transform(
                        transform: Matrix4.translationValues(0, 5, 0),
                        child: Divider(
                          height: 5,
                          endIndent: 4,
                          thickness: 10,
                          indent: 4,
                          color: _selectedIndex == index
                              ? navBarColor
                              : const Color.fromRGBO(0, 0, 0, 0),
                        ),
                      ),
                    )),
          ),
          BottomNavigationBar(
              key: const Key('bottom'),
              type: BottomNavigationBarType.fixed,
              elevation: 0.0,
              items: [
                BottomNavigationBarItem(
                  icon: const Image(
                    image: AssetImage("assets/images/icons/home_outline.png"),
                    fit: BoxFit.scaleDown,
                    alignment: Alignment.center,
                  ),
                  activeIcon: Image(
                    key: const Key('home'),
                    image: AssetImage(homeIconPath),
                    fit: BoxFit.scaleDown,
                    alignment: Alignment.center,
                  ),
                  label: 'Home',
                ),
                BottomNavigationBarItem(
                  icon: const Image(
                    image:
                        AssetImage("assets/images/icons/calendar_outline.png"),
                    fit: BoxFit.scaleDown,
                    alignment: Alignment.center,
                  ),
                  activeIcon: Image(
                    key: const Key('calendar'),
                    image: AssetImage(calendarIconPath),
                    fit: BoxFit.scaleDown,
                    alignment: Alignment.center,
                  ),
                  label: 'Calendar',
                ),
                BottomNavigationBarItem(
                  icon: const Image(
                    image: AssetImage("assets/images/icons/job_outline.png"),
                    fit: BoxFit.scaleDown,
                    alignment: Alignment.center,
                  ),
                  activeIcon: Image(
                    key: const Key('job'),
                    image: AssetImage(jobIconPath),
                    fit: BoxFit.scaleDown,
                    alignment: Alignment.center,
                  ),
                  label: 'My Job',
                ),
                BottomNavigationBarItem(
                    icon: const Image(
                      image:
                          AssetImage("assets/images/icons/profile_outline.png"),
                      fit: BoxFit.scaleDown,
                      alignment: Alignment.center,
                    ),
                    activeIcon: Image(
                      key: const Key('profile'),
                      image: AssetImage(profileIconPath),
                      fit: BoxFit.scaleDown,
                      alignment: Alignment.center,
                    ),
                    label: 'Profile'),
              ],
              selectedLabelStyle: const TextStyle(fontSize: 12),
              selectedItemColor: navBarColor,
              showUnselectedLabels: false,
              currentIndex: _selectedIndex,
              backgroundColor: const Color.fromRGBO(255, 255, 255, 1),
              onTap: onTapped),
        ]),
      ),
    );
  }
}
