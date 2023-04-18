// ignore_for_file: lines_longer_than_80_chars
import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/log_hours.dart';
import 'package:mobile/log_hours_submission.dart';
import 'package:time_interval_picker/time_interval_picker.dart';
import 'api_service_test.mocks.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/utils/api_service.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mockito/mockito.dart';
import 'package:mobile/utils/constants.dart';

void main() {
  testWidgets('Log Hours Primary Page', (WidgetTester tester) async {
    await tester.runAsync(() async {
      // Build our app and trigger a frame.
      final client = MockClient();
      LogHours.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await loadAsset();

      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 0,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      //Get Account Info
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      final loggedInfo = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T11:00:00',
          'Function': 'Work',
          'Status': 'Submitted'
        }
      ];
      final temp = DateTime.parse('2023-03-31T00:00:00')
          .toLocal()
          .add(const Duration(hours: 5));
      final loggedHoursBody =
          jsonEncode({'sessionToken': "token123", 'day': temp.toString()});
      final encryptedLoggedHoursBody = encrypt(loggedHoursBody);
      final String loggedHoursString = jsonEncode(loggedInfo);
      final dynamic encryptedLoggedHours = encrypt(loggedHoursString, true);

      //Get Logged Hours
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedLoggedHoursBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryptedLoggedHours, 200));
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body:
                  '{"Data":"mLuL9JOSCbPseMJuKC4lYTP6KOiLlWO28nNoWGi16Rb+FA0qmF9TNICWDDI2SAcp/YOVU3GMvF8WoZ3Mk1SHJA=="}',
              encoding: null))
          .thenAnswer((_) async => http.Response(encryptedLoggedHours, 200));
      //Load screen
      await tester.pumpWidget(MaterialApp(
          home: LogHours(anotherDate: DateTime.parse('2023-03-31T00:00:00'))));

      //look for semicircle
      expect(find.byKey(const ValueKey("appBar")), findsOneWidget);

      //look for page title
      expect(find.byKey(const ValueKey("title")), findsOneWidget);

      //look for back button
      expect(find.byKey(const ValueKey("backButton")), findsOneWidget);

      //look for month and year
      expect(find.byKey(const ValueKey("monthYear")), findsOneWidget);

      //look for sliding calendar
      expect(find.byKey(const ValueKey("slidingCalendar")), findsOneWidget);

      //look for log hours button
      expect(find.byKey(const ValueKey("logHoursButton")), findsOneWidget);

      //More widget tests
      expect(find.byType(Padding), findsNWidgets(26));
      expect(find.byType(CircleAvatar), findsNWidgets(1));
      expect(find.byType(Align), findsNWidgets(4));
      expect(find.byType(ListView), findsNWidgets(2));
      expect(find.byType(SizedBox), findsNWidgets(6));

      //tap the sliding calendar
      await tester.tap(find.byKey(const ValueKey("slidingCalendar")));

      //tap the log hours button
      await tester.tap(find.byKey(const ValueKey("logHoursIconButton")));
    });
  });
  testWidgets('Test back button on primary page', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHours.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      //Stubing the http requests
      await loadAsset();

      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 0,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      //Get Account Info
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      final loggedInfo = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T11:00:00',
          'Function': 'Work',
          'Status': 'Submitted'
        }
      ];
      final temp = DateTime.parse('2023-03-31T00:00:00')
          .toLocal()
          .add(const Duration(hours: 5));
      final loggedHoursBody =
          jsonEncode({'sessionToken': "token123", 'day': temp.toString()});
      final encryptedLoggedHoursBody = encrypt(loggedHoursBody);
      final String loggedHoursString = jsonEncode(loggedInfo);
      final dynamic encryptedLoggedHours = encrypt(loggedHoursString, true);

      //Get Logged Hours
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedLoggedHoursBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryptedLoggedHours, 200));

      //Load screen
      await tester.pumpWidget(MaterialApp(
          home: LogHours(anotherDate: DateTime.parse('2023-03-31T00:00:00'))));

      //tap the log hours button
      await tester.tap(find.byKey(const ValueKey("backButton")));
    });
  });
  testWidgets('Log Hours Secondary Page', (WidgetTester tester) async {
    await tester.runAsync(() async {
      // Build our app and trigger a frame.
      final client = MockClient();
      LogHours.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      const String token = 'token123';
      //Stubing the http requests
      await loadAsset();
      final userInfo = {
        "Date": "2023-03-12T23:20:34.980158Z",
        "StartTime": "2023-03-12T11:00:00.000Z",
        "EndTime": "2023-03-12T20:00:00.000Z"
      };
      final body = jsonEncode({'sessionToken': token, 'submission': userInfo});
      final encryptedBody = encrypt(body);
      when(client.put(Uri.parse('$baseUrl${BaseAPI.logHoursEndpoint}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer(
          (_) async => http.Response('Logged hours succesfully', 200));

      //Load screen
      await tester.pumpWidget(MaterialApp(
          home: LogHoursSubmission(
        rawDate: DateTime.now(),
      )));

      //look for back button
      expect(find.byKey(const ValueKey("backButton")), findsOneWidget);

      //look for time interval picker
      expect(find.byKey(const ValueKey("timePicker")), findsOneWidget);

      //look for add time period button
      expect(find.byKey(const ValueKey("timePeriod")), findsOneWidget);

      //look for log hours button
      expect(find.byKey(const ValueKey("logHoursButton")), findsOneWidget);

      await tester.pump(const Duration(milliseconds: 100));
      await tester.tap(find.byType(TimeIntervalPicker));

      //tap the add time period button
      await tester.tap(find.byKey(const ValueKey("timePeriod")));

      //tap the log hours button
      await tester.tap(find.byKey(const ValueKey("logHoursButton")));
    });
  });
  testWidgets('Test Back Button on Secondary Page',
      (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Load screen
      await tester.pumpWidget(MaterialApp(
          home: LogHoursSubmission(
        rawDate: DateTime.now(),
      )));

      //tap the log hours button
      await tester.tap(find.byKey(const ValueKey("backButton")));
    });
  });
  testWidgets('Tap Time Interval Pciker', (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Load screen
      await tester.pumpWidget(MaterialApp(
          home: LogHoursSubmission(
        rawDate: DateTime.now(),
      )));
      await tester.tapAt(const Offset(100.06, 188));
      await tester.tapAt(const Offset(100.06, 188));
      await tester.tapAt(const Offset(300.06, 288));
      await tester.tapAt(const Offset(300.06, 288));
    });
  });
  testWidgets('Theme is Blue - Primary', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHours.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 0,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      final loggedInfo = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T11:00:00',
          'Function': 'Work',
          'Status': 'Submitted'
        }
      ];
      final temp = DateTime.parse('2023-03-31T00:00:00')
          .toLocal()
          .add(const Duration(hours: 5));
      final loggedHoursBody =
          jsonEncode({'sessionToken': "token123", 'day': temp.toString()});
      final encryptedLoggedHoursBody = encrypt(loggedHoursBody);
      final String loggedHoursString = jsonEncode(loggedInfo);
      final dynamic encryptedLoggedHours = encrypt(loggedHoursString, true);

      //Get Logged Hours
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedLoggedHoursBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryptedLoggedHours, 200));

      await tester.pumpWidget(MaterialApp(
          home: LogHours(anotherDate: DateTime.parse('2023-03-31T00:00:00'))));
    });
  });
  testWidgets('Theme is Pink - Primary', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHours.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 1,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      final loggedInfo = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T11:00:00',
          'Function': 'Work',
          'Status': 'Submitted'
        }
      ];
      final temp = DateTime.parse('2023-03-31T00:00:00')
          .toLocal()
          .add(const Duration(hours: 5));
      final loggedHoursBody =
          jsonEncode({'sessionToken': "token123", 'day': temp.toString()});
      final encryptedLoggedHoursBody = encrypt(loggedHoursBody);
      final String loggedHoursString = jsonEncode(loggedInfo);
      final dynamic encryptedLoggedHours = encrypt(loggedHoursString, true);

      //Get Logged Hours
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedLoggedHoursBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryptedLoggedHours, 200));

      await tester.pumpWidget(MaterialApp(
          home: LogHours(anotherDate: DateTime.parse('2023-03-31T00:00:00'))));
    });
  });
  testWidgets('Theme is Green - Primary', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHours.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 2,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      final loggedInfo = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T11:00:00',
          'Function': 'Work',
          'Status': 'Submitted'
        }
      ];
      final temp = DateTime.parse('2023-03-31T00:00:00')
          .toLocal()
          .add(const Duration(hours: 5));
      final loggedHoursBody =
          jsonEncode({'sessionToken': "token123", 'day': temp.toString()});
      final encryptedLoggedHoursBody = encrypt(loggedHoursBody);
      final String loggedHoursString = jsonEncode(loggedInfo);
      final dynamic encryptedLoggedHours = encrypt(loggedHoursString, true);

      //Get Logged Hours
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedLoggedHoursBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryptedLoggedHours, 200));

      await tester.pumpWidget(MaterialApp(
          home: LogHours(anotherDate: DateTime.parse('2023-03-31T00:00:00'))));
    });
  });
  testWidgets('Theme is Orange - Primary', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHours.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 3,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      final loggedInfo = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T11:00:00',
          'Function': 'Work',
          'Status': 'Submitted'
        }
      ];
      final temp = DateTime.parse('2023-03-31T00:00:00')
          .toLocal()
          .add(const Duration(hours: 5));
      final loggedHoursBody =
          jsonEncode({'sessionToken': "token123", 'day': temp.toString()});
      final encryptedLoggedHoursBody = encrypt(loggedHoursBody);
      final String loggedHoursString = jsonEncode(loggedInfo);
      final dynamic encryptedLoggedHours = encrypt(loggedHoursString, true);

      //Get Logged Hours
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedLoggedHoursBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryptedLoggedHours, 200));

      await tester.pumpWidget(MaterialApp(
          home: LogHours(anotherDate: DateTime.parse('2023-03-31T00:00:00'))));
    });
  });
  testWidgets('Theme is Blue - Secondary', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHoursSubmission.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 0,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      await tester.pumpWidget(
          MaterialApp(home: LogHoursSubmission(rawDate: DateTime.now())));
    });
  });
  testWidgets('Theme is Pink - Secondary', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHoursSubmission.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 1,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      await tester.pumpWidget(
          MaterialApp(home: LogHoursSubmission(rawDate: DateTime.now())));
    });
  });
  testWidgets('Theme is Green - Submission', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHoursSubmission.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 2,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      await tester.pumpWidget(
          MaterialApp(home: LogHoursSubmission(rawDate: DateTime.now())));
    });
  });
  testWidgets('Theme is Orange - Submission', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      LogHoursSubmission.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      final Map<String, dynamic> data = {
        'Id': 'ZZZ',
        'Name': 'Test',
        'Email': 'testEmail@gmail.com',
        'Password': 'pw',
        'Address': '0 street',
        'Birthday': '2002-12-17T00:00:00',
        'PhoneNumber': 0,
        'ThemeColor': 3,
        'VacationDays': 0,
        'SickDays': 0,
        'Position': null
      };
      final String dataString = jsonEncode(data);
      final dynamic encryption = encrypt(dataString, true);

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      await tester.pumpWidget(
          MaterialApp(home: LogHoursSubmission(rawDate: DateTime.now())));
    });
  });
}
