//Import Statements
import 'package:flutter/material.dart';
import 'package:mobile/utils/api_service.dart';
import 'login_page.dart';
import 'nav_bar.dart';
import 'themes/colors.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:shared_preferences/shared_preferences.dart';
import 'themes/my_clipper.dart';

//Create the Profile Page
class ProfilePage extends StatefulWidget {
  static http.Client? httpClient;
  const ProfilePage({Key? key}) : super(key: key);

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  //Variables
  ProfileInfo profileInfo = ProfileInfo(' ', ' ', ' ', ' ', ' ', ' ', 0, 0);
  final double coverHeight = 250;
  final double profileRadius = 80;
  bool blueSelected = false;
  bool pinkSelected = false;
  bool greenSelected = false;
  bool orangeSelected = false;
  String firstName = " ";
  String lastName = " ";
  String name = " ";
  String id = " ";
  String birthday = " ";
  String email = " ";
  String password = " ";
  String address = " ";
  int phoneNumber = 12345;
  int themeColor = 0;
  bool editProfile = false;
  TextEditingController nameController = TextEditingController();
  TextEditingController idController = TextEditingController();
  TextEditingController birthdayController = TextEditingController();
  TextEditingController emailController = TextEditingController();
  TextEditingController passwordController = TextEditingController();
  TextEditingController addressController = TextEditingController();
  TextEditingController phoneNumberController = TextEditingController();
  late SharedPreferences prefs;
  http.Client httpClient = http.Client();
  Color headerColor = Colors.white;
  Color logOutColor = Colors.white;
  Color gradientColor1 = Colors.white;
  Color gradientColor2 = Colors.white;

  @override
  void initState() {
    super.initState();
    setClient(ProfilePage.httpClient);
    getProfileInfo(httpClient);
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  //Function that fetches the data from the database to display on the screen
  void getProfileInfo(http.Client client) async {
    setState(() {});
    prefs = await SharedPreferences.getInstance();

    final jsonString =
        await ApiService().getAccountInfo(client, prefs.getString('token'));
    final jsonData = jsonDecode(jsonString);

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
    id = jsonData['Id'].toString();
    idController.text = id;
    name = jsonData['Name'].toString();
    nameController.text = name;
    email = jsonData['Email'].toString();
    emailController.text = email;
    password = jsonData['Password'].toString();
    passwordController.text = password;
    address = jsonData['Address'].toString();
    addressController.text = address;
    birthday = jsonData['Birthday'].toString();
    birthdayController.text = birthday;
    phoneNumber = jsonData['PhoneNumber'];
    phoneNumberController.text = phoneNumber.toString();
    themeColor = jsonData['ThemeColor'];
    if (themeColor == 0) {
      blueSelected = true;
      headerColor = MyColors.customBlue;
      logOutColor = MyColors.buttonColorsGradiant1;
      gradientColor1 = MyColors.buttonColorsGradiant1;
      gradientColor2 = MyColors.buttonColorsGradiant2;
    } else if (themeColor == 1) {
      pinkSelected = true;
      headerColor = MyColors.customPink;
      logOutColor = MyColors.buttonColorsGradient3;
      gradientColor1 = MyColors.buttonColorsGradient3;
      gradientColor2 = MyColors.buttonColorsGradient4;
    } else if (themeColor == 2) {
      greenSelected = true;
      headerColor = MyColors.customGreen;
      logOutColor = MyColors.buttonColorsGradient5;
      gradientColor1 = MyColors.buttonColorsGradient5;
      gradientColor2 = MyColors.buttonColorsGradient6;
    } else if (themeColor == 3) {
      orangeSelected = true;
      headerColor = MyColors.customOrange;
      logOutColor = MyColors.buttonColorsGradient7;
      gradientColor1 = MyColors.buttonColorsGradient7;
      gradientColor2 = MyColors.buttonColorsGradient8;
    }
    setState(() {});
  }

  //Function that stores information that has been updated and calls PUT method
  void saveInfo() {
    name = nameController.text;
    email = emailController.text;
    password = passwordController.text;
    address = addressController.text;
    //phoneNumber = int.parse(phoneNumberController.text);
    profileInfo = ProfileInfo(
        id, name, email, password, address, birthday, phoneNumber, themeColor);
    updateProfileInfo(httpClient);
  }

  //Function that updates changes after editing profile to the database
  void updateProfileInfo(http.Client client) async {
    final body = jsonEncode({
      'Id': profileInfo.id,
      'Name': profileInfo.name,
      'Email': profileInfo.email,
      'Password': profileInfo.password,
      'Address': profileInfo.address,
      'Birthday': profileInfo.birthday,
      'PhoneNumber': profileInfo.phoneNumber,
      'ThemeColor': profileInfo.themeColor
    });

    await ApiService()
        .updateAccountInfo(body, client, prefs.getString('token'));
  }

  //Function that updates the theme
  void updateTheme(http.Client client) async {
    await ApiService()
        .updateTheme(themeColor, client, prefs.getString('token'));
  }

  //Function that updates the theme
  void logout(http.Client client) async {
    await ApiService().logout(client, prefs.getString('token'));
  }

  //What is displayed on the screen
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        resizeToAvoidBottomInset: false,
        backgroundColor: Colors.white,
        body: SingleChildScrollView(
          child: Column(children: <Widget>[
            Stack(
              clipBehavior: Clip.none,
              alignment: Alignment.center,
              children: [
                ClipPath(
                  clipper: Semicircle(),
                  child: Opacity(
                    opacity: 0.3,
                    child: Container(
                      color: headerColor,
                      height: coverHeight,
                    ),
                  ),
                ),
                Positioned(
                  top: 50,
                  child: Text(
                    key: const Key("fullName"),
                    "$firstName, $lastName",
                    style: const TextStyle(
                        fontFamily: 'Montserrat',
                        fontSize: 24,
                        color: Color(0xff262626),
                        fontWeight: FontWeight.w400),
                  ),
                ),
                const Positioned(
                  top: 80,
                  child: Text(
                    key: Key("position"),
                    'Sales Associate',
                    style: TextStyle(
                        fontFamily: 'Montserrat',
                        fontSize: 16,
                        color: MyColors.customGrey,
                        fontWeight: FontWeight.w400),
                  ),
                ),
                Positioned(
                    top: coverHeight - profileRadius * 1.8,
                    child: profilePicture()),
              ],
            ),
            Padding(
              padding: const EdgeInsets.only(top: 15),
              child: Card(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: <Widget>[
                    TextButton.icon(
                      key: const Key('logOut'),
                      onPressed: () {
                        logout(httpClient);
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => const LoginDemo()),
                        );
                      },
                      style: TextButton.styleFrom(
                        foregroundColor: logOutColor,
                      ),
                      icon: Icon(
                        Icons.logout_rounded,
                        color: logOutColor,
                        size: 24.0,
                      ),
                      label: const Text('Log Out'),
                    ),
                    ListTile(
                      leading: Image.asset(
                        'assets/images/icons/name.png',
                        scale: 1.0,
                        height: 50,
                        width: 50,
                      ),
                      title: TextFormField(
                        key: const Key('name'),
                        decoration: const InputDecoration(
                          border: InputBorder.none,
                        ),
                        controller: nameController,
                        readOnly: !editProfile,
                        style: const TextStyle(
                          fontFamily: 'Montserrat',
                          color: MyColors.customGrey,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
            Card(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  ListTile(
                    leading: Image.asset(
                      'assets/images/icons/id.png',
                      scale: 1.0,
                      height: 50,
                      width: 50,
                    ),
                    title: TextFormField(
                      decoration: const InputDecoration(
                        border: InputBorder.none,
                      ),
                      controller: idController,
                      readOnly: true,
                      style: const TextStyle(
                        fontFamily: 'Montserrat',
                        color: MyColors.customGrey,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Card(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  ListTile(
                    leading: Image.asset(
                      'assets/images/icons/dob.png',
                      scale: 1.0,
                      height: 50,
                      width: 50,
                    ),
                    title: TextFormField(
                      decoration: const InputDecoration(
                        border: InputBorder.none,
                      ),
                      controller: birthdayController,
                      readOnly: true,
                      style: const TextStyle(
                        fontFamily: 'Montserrat',
                        color: MyColors.customGrey,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Card(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  ListTile(
                    leading: Image.asset(
                      'assets/images/icons/telephone.png',
                      scale: 1.0,
                      height: 50,
                      width: 50,
                    ),
                    title: TextFormField(
                      key: const Key('number'),
                      decoration: const InputDecoration(
                        border: InputBorder.none,
                      ),
                      controller: phoneNumberController,
                      readOnly: !editProfile,
                      style: const TextStyle(
                        fontFamily: 'Montserrat',
                        color: MyColors.customGrey,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Card(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  ListTile(
                    leading: Image.asset(
                      'assets/images/icons/Email.png',
                      scale: 1.0,
                      height: 50,
                      width: 50,
                    ),
                    title: TextFormField(
                      key: const Key('mail'),
                      decoration: const InputDecoration(
                        border: InputBorder.none,
                      ),
                      controller: emailController,
                      readOnly: !editProfile,
                      style: const TextStyle(
                        fontFamily: 'Montserrat',
                        color: MyColors.customGrey,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Card(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  ListTile(
                    leading: Image.asset(
                      'assets/images/icons/Password.png',
                      scale: 1.0,
                      height: 50,
                      width: 50,
                    ),
                    title: TextFormField(
                      key: const Key('password'),
                      decoration: const InputDecoration(
                        border: InputBorder.none,
                      ),
                      controller: passwordController,
                      obscureText: true,
                      readOnly: !editProfile,
                      style: const TextStyle(
                        fontFamily: 'Montserrat',
                        color: MyColors.customGrey,
                      ),
                    ),
                    trailing: const Icon(
                      Icons.autorenew,
                      color: MyColors.buttonColorsGradiant1,
                    ),
                  ),
                ],
              ),
            ),
            if (editProfile) ...[
              Padding(
                padding: const EdgeInsets.only(top: 20),
                child: ElevatedButton(
                  key: const Key("save"),
                  onPressed: () {
                    editProfile = false;
                    saveInfo();
                    setState(() {});
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
                        'Save',
                        style: TextStyle(
                            fontSize: 18,
                            fontFamily: 'Montserrat',
                            fontWeight: FontWeight.w700),
                      ),
                    ),
                  ),
                ),
              ),
            ] else ...[
              Padding(
                padding: const EdgeInsets.only(top: 20),
                child: ElevatedButton(
                  key: const Key("editProfile"),
                  onPressed: () {
                    editProfile = true;
                    setState(() {});
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
                        'Edit Profile',
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
            const Padding(
              padding: EdgeInsets.only(top: 20),
              child: Text(
                'Change Theme',
                style: TextStyle(
                  fontFamily: 'Montserrat',
                  color: MyColors.black,
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(top: 20),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Padding(
                    padding: const EdgeInsets.only(left: 3, right: 3),
                    child: ElevatedButton(
                        key: const Key("blueTheme"),
                        onPressed: () {
                          themeColor = 0;
                          blueSelected = true;
                          pinkSelected = false;
                          greenSelected = false;
                          orangeSelected = false;
                          updateTheme(httpClient);
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) =>
                                    const NavBar(navIndex: 3)),
                          );
                          setState(() {});
                        },
                        style: ElevatedButton.styleFrom(
                          fixedSize: const Size(60, 60),
                          shape: CircleBorder(
                            side: BorderSide(
                                color: blueSelected
                                    ? Colors.white
                                    : Colors.transparent,
                                width: 3),
                          ),
                          backgroundColor: MyColors.buttonColorsGradiant1,
                        ),
                        child: null),
                  ),
                  ElevatedButton(
                      key: const Key("pinkTheme"),
                      onPressed: () {
                        getProfileInfo(httpClient);
                        themeColor = 1;
                        blueSelected = false;
                        pinkSelected = true;
                        greenSelected = false;
                        orangeSelected = false;
                        updateTheme(httpClient);
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => const NavBar(navIndex: 3)),
                        );
                        setState(() {});
                      },
                      style: ElevatedButton.styleFrom(
                        fixedSize: const Size(60, 60),
                        shape: CircleBorder(
                          side: BorderSide(
                              color: pinkSelected
                                  ? Colors.white
                                  : Colors.transparent,
                              width: 3),
                        ),
                        backgroundColor: MyColors.buttonColorsGradient3,
                      ),
                      child: null),
                  Padding(
                    padding: const EdgeInsets.only(left: 3, right: 3),
                    child: ElevatedButton(
                        key: const Key("greenTheme"),
                        onPressed: () {
                          themeColor = 2;
                          blueSelected = false;
                          pinkSelected = false;
                          greenSelected = true;
                          orangeSelected = false;
                          updateTheme(httpClient);
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) =>
                                    const NavBar(navIndex: 3)),
                          );
                          setState(() {});
                        },
                        style: ElevatedButton.styleFrom(
                          fixedSize: const Size(60, 60),
                          shape: CircleBorder(
                            side: BorderSide(
                                color: greenSelected
                                    ? Colors.white
                                    : Colors.transparent,
                                width: 3),
                          ),
                          backgroundColor: MyColors.buttonColorsGradient5,
                        ),
                        child: null),
                  ),
                  ElevatedButton(
                      key: const Key("orangeTheme"),
                      onPressed: () {
                        themeColor = 3;
                        blueSelected = false;
                        pinkSelected = false;
                        greenSelected = false;
                        orangeSelected = true;
                        updateTheme(httpClient);
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => const NavBar(navIndex: 3)),
                        );
                        setState(() {});
                      },
                      style: ElevatedButton.styleFrom(
                        fixedSize: const Size(60, 60),
                        shape: CircleBorder(
                          side: BorderSide(
                              color: orangeSelected
                                  ? Colors.white
                                  : Colors.transparent,
                              width: 3),
                        ),
                        backgroundColor: MyColors.buttonColorsGradient7,
                      ),
                      child: null),
                ],
              ),
            ),
          ]),
        ),
      ),
    );
  }

  //Widget for Profile Picture
  Widget profilePicture() => CircleAvatar(
        radius: profileRadius,
        backgroundColor: Colors.white,
        child: CircleAvatar(
          radius: profileRadius - 5,
          backgroundImage: const AssetImage('assets/images/Reema.jpg'),
        ),
      );
}

//Create a class for the information
class ProfileInfo {
  final String name, email;
  final String id, password, address;
  final int phoneNumber, themeColor;
  final String birthday;

  ProfileInfo(this.id, this.name, this.email, this.password, this.address,
      this.birthday, this.phoneNumber, this.themeColor);
}
