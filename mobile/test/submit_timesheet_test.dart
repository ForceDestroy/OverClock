// ignore_for_file: lines_longer_than_80_chars
import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/submit_timesheet.dart';
import 'api_service_test.mocks.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/utils/api_service.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mockito/mockito.dart';
import 'package:mobile/utils/constants.dart';

void main() {
  testWidgets('Get TimeSheet', (WidgetTester tester) async {
    await tester.runAsync(() async {
      // Build our app and trigger a frame.
      final client = MockClient();
      TimeSheet.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');

      final body1 = jsonEncode({'sessionToken': "token123"});
      final encryptedBody1 = encrypt(body1);
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody1,
              encoding: null))
          .thenAnswer((_) async => http.Response(
              '"zvaDV0NdO+2o8DdoFL/kaaVbWNZQp1+N2v82BvAdjz0VRMqwQjRE/tyoSx2bA42Q6zLgZgEdSajgRNePt9DaXRjr9ae97T5Cc88H929ZlUJ6CBhZoiOFeMYaeL/rQkPsdd6TPuaTDDYHu5irNoxGVKjBSM3I+2O8mhYAaLKf2bejX8xXuOh5ARcpx7qMhyi33TCgZLKyH8w/7FBUwBQvI2HuVECHdwDtOEOIkJeut6Zcigl8xnP56V4OMBEgzmFbJ4gVzUEIc206vNIxdZF9CMa1ItRDapVga9LH/THPVYuCt+QiEITZ6+gAzRdidra1UUBzHR1E1IE5jbHqDPS0HA=="',
              200));

      await loadAsset();
      final DateTime date = DateTime.now();
      final DateTime date2 = date.toLocal().add(const Duration(hours: 5));
      final String date2String = date2.toString();
      const r = 'r2ViU/m7kaKP7Z90fCv6K/0uIlF9HG0WBA+l+WLATQo=';
      final body2 =
          jsonEncode({'sessionToken': "token123", 'date': date2String});
      final encryptedBody2 = encrypt(body2);
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getTimeSheetEndpoint}'),
          body: encryptedBody2,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(r, 200));

      when(client.put(Uri.parse('$baseUrl${BaseAPI.submitTimeEndpoint}'),
          body:
              '{"data":"mLuL9JOSCbPseMJuKC4lYdorGWU8Ri/M4SxsLB/h6YH96IRX98faHn0LblUoR84JTB4LimrTyMl/VKplrLgTmVD6GHZmNt8QdZZCNJhvSb+42z8XYKEAtrNLjO97GRdsiNysL5VzU1WJTPJdkGyhtW9HG3nPiHcr4WEv0MxQiieToEudf5HcglepEj449Jw2xMe/ZlUo6hpl7nINQvKVtw=="}',
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer(
          (_) async => http.Response('submitted timesheet succesfully', 200));

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: TimeSheet()));

//look for semicircle
      expect(
          find.byKey(const ValueKey("submitTimesheetAppBar")), findsOneWidget);

      //look for page title
      expect(find.byKey(const ValueKey("title")), findsOneWidget);

      //look for back button
      expect(find.byKey(const ValueKey("backButton")), findsOneWidget);

      //look for month and year
      expect(find.byKey(const ValueKey("monthDay")), findsOneWidget);
      //More widget tests
      expect(find.byType(Scaffold), findsNWidgets(1));
      expect(find.byType(Row), findsNWidgets(1));
      expect(find.byType(Column), findsNWidgets(9));
      expect(find.byType(Padding), findsNWidgets(27));
      expect(find.byType(Container), findsNWidgets(9));
      expect(find.byType(ListView), findsNWidgets(3));
      expect(find.byType(SizedBox), findsNWidgets(5));

      //look for submit timesheet button
      expect(find.byKey(const ValueKey("submitTimesheet")), findsOneWidget);

      //look for sliding calendar
      await tester.tap(find.byKey(const ValueKey("dateSlider")));

      await tester.pumpWidget(const MaterialApp(
          home: TimeIntervalWidget(timeInterval: [
        {
          "Date": "2023-01-02T05:00:00.000",
          "StartTime": "2023-01-02T13:00:00.000",
          "EndTime": "2023-01-02T17:00:00.000"
        }
      ])));

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: TimeSheet()));

      //  look for add time period button
      await tester.tap(find.byKey(const ValueKey("addNewPeriod")));
      void mockFunction() {}
      await tester
          .pumpWidget(MaterialApp(home: AddPeriod(notifyParent: mockFunction)));
      // look for add time period button
      expect(find.byKey(const ValueKey("addHours")), findsOneWidget);

      // look for time interval picker
      await tester.tap(find.byKey(const ValueKey("addPeriod")));

      await tester.tap(find.byKey(const ValueKey("add")));

      await tester.pumpWidget(const MaterialApp(home: TimeSheet()));

      await tester.tap(find.byKey(const ValueKey("backButton")));
    });
  });
  testWidgets('Theme is Blue', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeSheet.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: TimeSheet()));
    });
  });
  testWidgets('Theme is Pink', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeSheet.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: TimeSheet()));
    });
  });
  testWidgets('Theme is Green', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeSheet.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: TimeSheet()));
    });
  });
  testWidgets('Theme is Orange', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      TimeSheet.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: TimeSheet()));
    });
  });
}
