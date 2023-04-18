import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:mobile/themes/colors.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mobile/utils/api_service.dart';
import 'package:http/http.dart' as http;
import 'profile_page.dart';
import 'package:mobile/themes/check_mark_animation.dart';
import 'dart:async';

class CustomRequest extends StatefulWidget {
  const CustomRequest({super.key});
  static http.Client? httpClient;

  @override
  State<CustomRequest> createState() => _CustomRequestState();
}

class _CustomRequestState extends State<CustomRequest> {
  int themeColor = 0;
  late SharedPreferences prefs;
  final double coverHeight = 250;
  TextEditingController requestController = TextEditingController();
  TextEditingController typeController = TextEditingController();
  String request = '';
  String firstName = " ";
  String lastName = " ";
  String name = " ";
  String id = " ";
  String birthday = " ";
  String email = " ";
  String password = " ";
  String address = " ";
  int phoneNumber = 12345;
  http.Client httpClient = http.Client();
  int requestID = 0;
  String managerID = '';
  String status = 'Pending';
  DateTime startTime = DateTime.now();
  DateTime date = DateTime.now();
  DateTime endTime = DateTime.now();
  Color headerColor = Colors.white;
  Color gradientColor1 = Colors.white;
  Color gradientColor2 = Colors.white;
  Color animationColor = Colors.white;

  ProfileInfo profileInfo = ProfileInfo(' ', ' ', ' ', ' ', ' ', ' ', 0, 0);
  @override
  void initState() {
    super.initState();
    setClient(CustomRequest.httpClient);
    getProfileInfo(httpClient);
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

      name = jsonData['Name'].toString();

      email = jsonData['Email'].toString();

      password = jsonData['Password'].toString();

      address = jsonData['Address'].toString();

      birthday = jsonData['Birthday'].toString();

      phoneNumber = jsonData['PhoneNumber'];
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
    // print('printing info');
    // print(id);
    // print('printed');
    setState(() {});
  }

  void submitRequest() async {
    prefs = await SharedPreferences.getInstance();
    // print('sending request');

    request = requestController.text;
    final body = jsonEncode({
      'id': requestID.toString(),
      'fromId': id,
      'toId': managerID,
      'body': request,
      'date': date.toIso8601String(),
      'startTime': startTime.toIso8601String(),
      'endTime': endTime.toIso8601String(),
      'status': status
    });
    // print(body);
    // print('hi');

    await ApiService()
        .customRequest(body, http.Client(), prefs.getString('token'));
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        home: Scaffold(
      appBar: AppBar(
        key: const Key("appBar"),
        title: const Text('Custom Request', key: Key('title')),
        backgroundColor: headerColor,
        titleTextStyle: const TextStyle(
          color: Colors.white,
          fontWeight: FontWeight.w500,
          fontSize: 20,
          fontFamily: 'Montserrat',
        ),
      ),
      body: Column(
        children: [
          SingleChildScrollView(
            child: Column(
              children: [
                const SizedBox(
                  height: 20,
                ),
                Row(
                  children: [
                    IconButton(
                      key: const Key("backButton"),
                      icon: const Icon(Icons.arrow_back),
                      onPressed: () {
                        Navigator.pop(context);
                      },
                    ),
                    const Text(
                      key: Key('custom'),
                      'Enter your custom request below :',
                      style: TextStyle(
                          fontFamily: 'Montserrat',
                          fontSize: 15,
                          color: MyColors.black,
                          fontWeight: FontWeight.bold),
                    )
                  ],
                ),
                Padding(
                  key: const Key("requestPad"),
                  padding: const EdgeInsets.only(top: 10),
                  child: SizedBox(
                    key: const Key("requestBox"),
                    width: 300.0,
                    child: TextField(
                      key: const Key("request"),
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
                        labelText: 'Request...',
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
          Expanded(
            child: Align(
              alignment: Alignment.bottomCenter,
              child: Padding(
                key: const Key("customRequestPad"),
                padding: const EdgeInsets.all(48.0),
                child: ElevatedButton(
                  key: const Key("customRequestButton"),
                  onPressed: () {
                    final FocusScopeNode currentFocus = FocusScope.of(context);

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
    ));
  }
}
