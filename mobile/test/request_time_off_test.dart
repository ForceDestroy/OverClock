// ignore_for_file: lines_longer_than_80_chars, duplicate_ignore
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/time_off_request.dart';
import 'api_service_test.mocks.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/utils/api_service.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mockito/mockito.dart';
import 'package:mobile/utils/constants.dart';
import 'dart:convert';

void main() {
  testWidgets('Time off request', (WidgetTester tester) async {
    await tester.runAsync(() async {
      // Build our app and trigger a frame.
      final client = MockClient();
      TimeOffRequest.httpClient = client;
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

      final scheduleData = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T13:00:00',
          'EndTime': '2023-03-31T18:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        },
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T12:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        }
      ];

      final String scheduleDataString = jsonEncode(scheduleData);
      final dynamic scheduleEncryption = encrypt(scheduleDataString, true);

      //Get Assigned Schedule
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(scheduleEncryption, 200));

      //Load screen
      await tester.pumpWidget(
          MaterialApp(home: TimeOffRequest(anotherDate: DateTime.now())));

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

      //look for Sick Radio Button
      expect(find.byKey(const ValueKey("sick")), findsOneWidget);

      //look for Vacation Radio Button
      expect(find.byKey(const ValueKey("vacation")), findsOneWidget);

      //look for Personal Radio Button
      expect(find.byKey(const ValueKey("personal")), findsOneWidget);

      //look for log hours button
      expect(
          find.byKey(const ValueKey("timeOffRequestButton")), findsOneWidget);

      //More widget tests
      expect(find.byType(Padding), findsNWidgets(33));
      expect(find.byType(ListView), findsNWidgets(2));
      expect(find.byType(SizedBox), findsNWidgets(6));

      //tap the sliding calendar
      await tester.pump(const Duration(milliseconds: 100));
      await tester.tap(find.byKey(const ValueKey("slidingCalendar")));
      await tester.pumpAndSettle();

      //Test radio buttons
      await tester.pump(const Duration(milliseconds: 100));
      await tester.tap(find.byKey(const ValueKey("sick")));
      await tester.tap(find.byKey(const ValueKey("vacation")));
      await tester.tap(find.byKey(const ValueKey("personal")));
      await tester.pumpAndSettle();

      //tap the time request button
      await tester.pump(const Duration(milliseconds: 100));
      final requestButton = find.byKey(const ValueKey("timeOffRequestButton"));
      await tester.ensureVisible(requestButton);
      await tester.tap(find.byKey(const ValueKey("timeOffRequestButton")));
      await tester.pumpAndSettle();
    });
  });
  testWidgets('Test Back Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeOffRequest.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');

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

      final scheduleData = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T13:00:00',
          'EndTime': '2023-03-31T18:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        },
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T12:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        }
      ];

      final String scheduleDataString = jsonEncode(scheduleData);
      final dynamic scheduleEncryption = encrypt(scheduleDataString, true);

      //Get Assigned Schedule
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(scheduleEncryption, 200));

      //Load screen
      await tester.pumpWidget(MaterialApp(
          home: TimeOffRequest(
              anotherDate: DateTime.parse('2023-03-31T00:00:00'))));

      //tap the log hours button
      await tester.tap(find.byKey(const ValueKey("backButton")));
    });
  });
  testWidgets('Theme is Blue', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeOffRequest.httpClient = client;
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

      final scheduleData = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T13:00:00',
          'EndTime': '2023-03-31T18:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        },
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T12:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        }
      ];

      final String scheduleDataString = jsonEncode(scheduleData);
      final dynamic scheduleEncryption = encrypt(scheduleDataString, true);

      //Get Assigned Schedule
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(scheduleEncryption, 200));

      await tester.pumpWidget(MaterialApp(
          home: TimeOffRequest(
        anotherDate: DateTime.now(),
      )));
    });
  });
  testWidgets('Theme is Pink', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeOffRequest.httpClient = client;
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

      final scheduleData = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T13:00:00',
          'EndTime': '2023-03-31T18:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        },
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T12:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        }
      ];

      final String scheduleDataString = jsonEncode(scheduleData);
      final dynamic scheduleEncryption = encrypt(scheduleDataString, true);

      //Get Assigned Schedule
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(scheduleEncryption, 200));

      await tester.pumpWidget(MaterialApp(
          home: TimeOffRequest(
        anotherDate: DateTime.now(),
      )));
    });
  });
  testWidgets('Theme is Green', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeOffRequest.httpClient = client;
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

      final scheduleData = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T13:00:00',
          'EndTime': '2023-03-31T18:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        },
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T12:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        }
      ];

      final String scheduleDataString = jsonEncode(scheduleData);
      final dynamic scheduleEncryption = encrypt(scheduleDataString, true);

      //Get Assigned Schedule
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(scheduleEncryption, 200));

      await tester.pumpWidget(MaterialApp(
          home: TimeOffRequest(
        anotherDate: DateTime.now(),
      )));
    });
  });
  testWidgets('Theme is Orange', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeOffRequest.httpClient = client;
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

      final scheduleData = [
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T13:00:00',
          'EndTime': '2023-03-31T18:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        },
        {
          'Date': '2023-03-31T00:00:00',
          'StartTime': '2023-03-31T07:00:00',
          'EndTime': '2023-03-31T12:00:00',
          'UserName': 'Danny Tremblay',
          'UserId': 'AAA_000003',
          'AllowedBreakTime': 0.0
        }
      ];

      final String scheduleDataString = jsonEncode(scheduleData);
      final dynamic scheduleEncryption = encrypt(scheduleDataString, true);

      //Get Assigned Schedule
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(scheduleEncryption, 200));

      await tester.pumpWidget(MaterialApp(
          home: TimeOffRequest(
        anotherDate: DateTime.now(),
      )));
    });
  });
}
