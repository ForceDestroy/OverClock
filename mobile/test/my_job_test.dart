// ignore_for_file: lines_longer_than_80_chars, duplicate_ignore
import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/my_job.dart';
import 'api_service_test.mocks.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/utils/api_service.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mockito/mockito.dart';
import 'package:mobile/utils/constants.dart';

void main() {
  testWidgets('My Job Page', (WidgetTester tester) async {
    await tester.runAsync(() async {
      // Build our app and trigger a frame.
      final client = MockClient();
      MyJob.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      final body1 = jsonEncode({'sessionToken': "token123"});
      final encryptedBody1 = encrypt(body1);

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
              body: encryptedBody1,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getPayslips}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(
          '"YdERJG/uP9+1Ot0IxHVwarTuuBXlIfUsT9Z8xmXvn7lBIIrPEaKBqHpnkdqbOMD5y+urNOv9Nto1OT6dO4uT/HURFwMQGX006UjKrbjrEaU="',
          200));

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: MyJob()));

      //look for semicircle
      expect(find.byKey(const ValueKey("customShape")), findsOneWidget);

      //look for page title
      expect(find.byKey(const ValueKey("pageTitle")), findsOneWidget);

      //More widgets by type
      expect(find.byType(Padding), findsNWidgets(37));
      expect(find.byType(Row), findsNWidgets(5));
      expect(find.byType(Expanded), findsNWidgets(8));
      expect(find.byType(Card), findsNWidgets(7));
      expect(find.byType(SizedBox), findsNWidgets(7));
      expect(find.byType(Column), findsNWidgets(9));
      expect(find.byType(Center), findsNWidgets(6));
      expect(find.byType(Text), findsNWidgets(18));
      expect(find.byType(ElevatedButton), findsNWidgets(6));
      expect(find.byType(Container), findsNWidgets(15));
      expect(find.byType(Image), findsNWidgets(1));
    });
  });
  testWidgets('Tap Payslips Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      // Build our app and trigger a frame.
      final client = MockClient();
      MyJob.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');

      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      final body1 = jsonEncode({'sessionToken': "token123"});
      final encryptedBody1 = encrypt(body1);

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
              body: encryptedBody1,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getPayslips}'),
          body: encryptedBody,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer((_) async => http.Response(
          '"YdERJG/uP9+1Ot0IxHVwarTuuBXlIfUsT9Z8xmXvn7lBIIrPEaKBqHpnkdqbOMD5y+urNOv9Nto1OT6dO4uT/HURFwMQGX006UjKrbjrEaU="',
          200));

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: MyJob()));

      // tap the payslips button
      await tester.pump(const Duration(milliseconds: 100));
      final payslipsButton = find.byKey(const ValueKey("payslipsButton"));
      await tester.ensureVisible(payslipsButton);
      await tester.tap(find.byKey(const ValueKey("payslipsButton")));
      await tester.pumpAndSettle();
    });
  });
  testWidgets('Test Log Hours Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: MyJob()));

      // tap the log hours button button
      await tester.pump(const Duration(milliseconds: 100));
      final logHoursButton = find.byKey(const ValueKey("logHoursButton"));
      await tester.ensureVisible(logHoursButton);
      await tester.tap(find.byKey(const ValueKey("logHoursButton")));
      await tester.pumpAndSettle();
    });
  });
  testWidgets('Test scheduleChangeButton Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: MyJob()));

      // tap the schedule change button button
      await tester.pump(const Duration(milliseconds: 100));
      final scheduleChangeButton =
          find.byKey(const ValueKey("scheduleChangeButton"));
      await tester.ensureVisible(scheduleChangeButton);
      await tester.tap(find.byKey(const ValueKey("scheduleChangeButton")));
      await tester.pumpAndSettle();
    });
  });
  testWidgets('Test scheduleChangeButton Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: MyJob()));

      // tap the weeklyTimesheetButton button
      await tester.pump(const Duration(milliseconds: 100));
      final weeklyTimesheetButton =
          find.byKey(const ValueKey("weeklyTimesheetButton"));
      await tester.ensureVisible(weeklyTimesheetButton);
      await tester.tap(find.byKey(const ValueKey("weeklyTimesheetButton")));
      await tester.pumpAndSettle();
    });
  });
  testWidgets('Test customRequestsButton Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: MyJob()));

      // tap the customRequestsButton button
      await tester.pump(const Duration(milliseconds: 100));
      final customRequestsButton =
          find.byKey(const ValueKey("customRequestsButton"));
      await tester.ensureVisible(customRequestsButton);
      await tester.tap(find.byKey(const ValueKey("customRequestsButton")));
      await tester.pumpAndSettle();
    });
  });
  testWidgets('Test requestTimeOffButton Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      //Load screen
      await tester.pumpWidget(const MaterialApp(home: MyJob()));

      // tap the requestTimeOffButton button
      await tester.pump(const Duration(milliseconds: 100));
      final requestTimeOffButton =
          find.byKey(const ValueKey("requestTimeOffButton"));
      await tester.ensureVisible(requestTimeOffButton);
      await tester.tap(find.byKey(const ValueKey("requestTimeOffButton")));
      await tester.pumpAndSettle();
    });
  });

  testWidgets('Theme is Blue', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: MyJob()));
    });
  });
  testWidgets('Theme is Pink', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: MyJob()));
    });
  });
  testWidgets('Theme is Green', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: MyJob()));
    });
  });

  testWidgets('Theme is Orange', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      MyJob.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: MyJob()));
    });
  });
}
