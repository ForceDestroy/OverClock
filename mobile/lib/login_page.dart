import 'package:flutter/material.dart';
import 'nav_bar.dart';
import 'themes/my_clipper.dart';
import 'themes/colors.dart';
import 'utils/api_service.dart';
import 'utils/validation.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';
import '../utils/calendar_utils.dart';

class LoginDemo extends StatefulWidget {
  const LoginDemo({Key? key}) : super(key: key);

  @override
  State<LoginDemo> createState() => _LoginDemoState();
}

class _LoginDemoState extends State<LoginDemo> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  TextEditingController emailController = TextEditingController();
  TextEditingController passwordController = TextEditingController();
  final ApiService _apiClient = ApiService();
  late SharedPreferences prefs;

  Future<void> login(ApiService api) async {
    if (_formKey.currentState!.validate()) {
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        key: const Key("loadingSnackBar"),
        content: const Text('Processing Data'),
        backgroundColor: Colors.green.shade300,
      ));
      final messenger = ScaffoldMessenger.of(context);
      final dynamic res = await api.login(
          emailController.text, passwordController.text, http.Client());
      messenger.hideCurrentSnackBar();
      if (res == 200) {
        messenger.showSnackBar(SnackBar(
          content: const Text('Logging in'),
          backgroundColor: Colors.green.shade300,
        ));
        //Create user schedule
        prefs = await SharedPreferences.getInstance();
        await createSchedule(http.Client());
        await Future.delayed(const Duration(seconds: 1));
        if (!mounted) return;
        Navigator.push(
            context,
            MaterialPageRoute(
                builder: (context) => const NavBar(
                      navIndex: 0,
                    )));
        messenger.hideCurrentSnackBar();
      } else if (res == 400) {
        // ignore: lines_longer_than_80_chars
        messenger.showSnackBar(SnackBar(
          content: const Text(
              'The email and password entered does not correspond to a user.'),
          backgroundColor: Colors.red.shade300,
        ));
      } else {
        messenger.showSnackBar(SnackBar(
          content: const Text('Something went wrong!'),
          backgroundColor: Colors.red.shade300,
        ));
      }
    }
  }

  createSchedule(http.Client client) async {
    prefs = await SharedPreferences.getInstance();
    final jsonString =
        await ApiService().getAllSchedule(client, prefs.getString('token'));
    createUserSchedule(jsonString);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: Colors.white,
        body: Form(
          key: _formKey,
          child: Stack(children: [
            ClipPath(
                clipper: MyClipper(),
                child: Opacity(
                    opacity: 0.3,
                    child: Container(
                      color: MyColors.customBlue,
                    ))),
            Center(
                child: SingleChildScrollView(
              child: Column(
                children: <Widget>[
                  Padding(
                    padding: EdgeInsets.only(
                        top: MediaQuery.of(context).size.height * 0.199),
                    child: Center(
                      child: Column(
                        children: [
                          SizedBox(
                            width: MediaQuery.of(context).size.width * 0.68,
                            height: MediaQuery.of(context).size.height * 0.08,
                            /*decoration: BoxDecoration(
                        color: Colors.red,
                        borderRadius: BorderRadius.circular(50.0)),*/
                            child: const Text(
                              "Login",
                              style: TextStyle(
                                  fontFamily: 'Montserrat',
                                  fontSize: 40,
                                  color: Color(0xff262626),
                                  fontWeight: FontWeight.w400),
                              textAlign: TextAlign.center,
                            ),
                          ),
                          Container(
                              margin: EdgeInsets.only(
                                  bottom: MediaQuery.of(context).size.height *
                                      0.038),
                              width: MediaQuery.of(context).size.width * 0.64,
                              height: MediaQuery.of(context).size.height * 0.33,
                              /*decoration: BoxDecoration(
                        color: Colors.red,
                        borderRadius: BorderRadius.circular(50.0)),*/
                              child: Image.asset(
                                  'assets/images/Authentication.png'))
                        ],
                      ),
                    ),
                  ),
                  Padding(
                    padding: EdgeInsets.only(
                        left: MediaQuery.of(context).size.width * 0.077,
                        right: MediaQuery.of(context).size.width * 0.077,
                        bottom: MediaQuery.of(context).size.height * 0.024),
                    child: SizedBox(
                        height: MediaQuery.of(context).size.height * 0.057,
                        child: TextFormField(
                          key: const Key('email'),
                          controller: emailController,
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              value = "";
                            }
                            return Validation.validateEmail(value);
                          },
                          cursorHeight: 20,
                          style: const TextStyle(
                              fontSize: 16, height: 1, color: Colors.black),
                          decoration: InputDecoration(
                            enabledBorder: const OutlineInputBorder(
                              borderSide: BorderSide(
                                  width: 1, color: MyColors.inputFieldsOutline),
                            ),
                            focusedBorder: const OutlineInputBorder(
                              borderSide: BorderSide(
                                  width: 3,
                                  color: MyColors.focusedInputFieldsOutline),
                            ),
                            prefixIcon: const Image(
                              image: AssetImage(
                                  "assets/images/icons/login-email-icon.png"),
                              color: null,
                              fit: BoxFit.scaleDown,
                              alignment: Alignment.center,
                            ),
                            labelText:
                                emailController.text == "" ? 'Email' : "",
                            labelStyle: const TextStyle(
                                color: MyColors.inputFieldsLabels),
                            filled: true,
                            fillColor: MyColors.fillColorInputFields,
                          ),
                        )),
                  ),
                  Padding(
                    padding: EdgeInsets.symmetric(
                        horizontal: MediaQuery.of(context).size.width * 0.077),
                    child: SizedBox(
                      height: MediaQuery.of(context).size.height * 0.057,
                      child: TextFormField(
                        key: const Key('password'),
                        controller: passwordController,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            value = "";
                          }
                          return Validation.validatePassword(value);
                        },
                        obscureText: true,
                        cursorHeight: 20,
                        style: const TextStyle(
                            fontSize: 15, height: 1, color: Colors.black),
                        decoration: InputDecoration(
                          enabledBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                                width: 1, color: MyColors.inputFieldsOutline),
                          ),
                          focusedBorder: const OutlineInputBorder(
                            borderSide: BorderSide(
                                width: 3,
                                color: MyColors.focusedInputFieldsOutline),
                          ),
                          prefixIcon: const Padding(
                              padding: EdgeInsets.only(left: 12, right: 15),
                              child: Image(
                                image: AssetImage(
                                    "assets/images/icons/login-pw-icon.png"),
                                fit: BoxFit.scaleDown,
                                alignment: Alignment.center,
                              )),
                          labelText:
                              passwordController.text == "" ? 'Password' : "",
                          labelStyle: const TextStyle(
                              color: MyColors.inputFieldsLabels),
                          filled: true,
                          fillColor: MyColors.fillColorInputFields,
                        ),
                      ),
                    ),
                  ),
                  TextButton(
                    onPressed: () {
                      //TODO FORGOT PASSWORD SCREEN GOES HERE
                    },
                    child: const Text(
                      'Forgot Password',
                      style: TextStyle(
                          color: MyColors.actionTextColor, fontSize: 15),
                    ),
                  ),
                  Container(
                      height: MediaQuery.of(context).size.height * 0.06,
                      width: MediaQuery.of(context).size.width * 0.42,
                      margin: EdgeInsets.only(
                          top: MediaQuery.of(context).size.height * 0.01,
                          bottom: MediaQuery.of(context).size.height * 0.028),
                      child: ElevatedButton(
                        key: const Key('signIn'),
                        onPressed: () async {
                          final FocusScopeNode currentFocus =
                              FocusScope.of(context);
                          if (!currentFocus.hasPrimaryFocus) {
                            currentFocus.unfocus();
                          }
                          login(_apiClient);
                        },
                        style: ElevatedButton.styleFrom(
                            padding: EdgeInsets.zero,
                            shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(15))),
                        child: Ink(
                          decoration: BoxDecoration(
                              gradient: const LinearGradient(colors: [
                                MyColors.buttonColorsGradiant1,
                                MyColors.buttonColorsGradiant2
                              ]),
                              borderRadius: BorderRadius.circular(15)),
                          child: Container(
                            alignment: Alignment.center,
                            child: const Text(
                              'Log In',
                              style: TextStyle(
                                  fontSize: 18,
                                  fontFamily: 'Montserrat',
                                  fontWeight: FontWeight.w700),
                            ),
                          ),
                        ),
                      )),
                  SizedBox(
                    height: MediaQuery.of(context).size.height * 0.05,
                  ),
                ],
              ),
            ))
          ]),
        ));
  }
}
