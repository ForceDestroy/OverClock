import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/nav_bar.dart';
import 'package:mobile/utils/api_service.dart';
import 'package:mobile/utils/constants.dart';
import 'package:mockito/mockito.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'api_service_test.mocks.dart';
import 'package:http/http.dart' as http;

void main() {
  testWidgets('BottomNavigationBar content test', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      NavBar.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await prefs.setInt('theme', 0);
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

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      await tester.pumpWidget(const MaterialApp(home: NavBar(navIndex: 0)));
      expect(find.byType(Scaffold), findsNWidgets(2));
      expect(find.byType(Container), findsNWidgets(15));
      expect(find.byType(Column), findsNWidgets(8));

      expect(find.byKey(const ValueKey("bottom")), findsOneWidget);
      expect(find.byKey(const ValueKey("home")), findsOneWidget);
      await tester.tap(find.byKey(const ValueKey("home")));
    });
  });
  testWidgets('Test My Job Page', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      NavBar.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await prefs.setInt('theme', 0);
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

      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody,
              encoding: null))
          .thenAnswer((_) async => http.Response(encryption, 200));

      await tester.pumpWidget(const MaterialApp(home: NavBar(navIndex: 2)));
      expect(find.byType(Scaffold), findsNWidgets(2));
      expect(find.byType(Container), findsNWidgets(15));
      expect(find.byType(Column), findsNWidgets(8));

      expect(find.byKey(const ValueKey("bottom")), findsOneWidget);
      expect(find.byKey(const ValueKey("job")), findsOneWidget);
      await tester.tap(find.byKey(const ValueKey("job")));
    });
  });
  testWidgets('Theme is Blue', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      NavBar.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await prefs.setInt('theme', 0);
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

      await tester.pumpWidget(const MaterialApp(
          home: NavBar(
        navIndex: 0,
      )));
    });
  });
  testWidgets('Theme is Pink', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      NavBar.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await prefs.setInt('theme', 1);
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

      await tester.pumpWidget(const MaterialApp(
          home: NavBar(
        navIndex: 0,
      )));
    });
  });
  testWidgets('Theme is Green', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      NavBar.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);

      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await prefs.setInt('theme', 2);
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

      await tester.pumpWidget(const MaterialApp(
          home: NavBar(
        navIndex: 0,
      )));
    });
  });
  testWidgets('Theme is Orange', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      NavBar.httpClient = client;
      final String baseUrl = await readJson();
      await loadAsset();
      final body = jsonEncode({'sessionToken': "token123"});
      final encryptedBody = encrypt(body);
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', 'token123');
      await prefs.setInt('theme', 3);
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

      await tester.pumpWidget(const MaterialApp(
          home: NavBar(
        navIndex: 0,
      )));
    });
  });
}
