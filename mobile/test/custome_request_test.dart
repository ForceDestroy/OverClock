// ignore_for_file: lines_longer_than_80_chars
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/custom_request.dart';
import 'api_service_test.mocks.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/utils/api_service.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:mockito/mockito.dart';
import 'package:mobile/utils/constants.dart';
import 'dart:convert';

void main() {
  testWidgets('Custom Request', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      CustomRequest.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: CustomRequest()));
      //look for page title
      expect(find.byKey(const ValueKey("title")), findsOneWidget);
      //look for semicircle
      expect(find.byKey(const ValueKey("appBar")), findsOneWidget);

      //More widget tests
      expect(find.byType(Scaffold), findsNWidgets(1));
      expect(find.byType(SingleChildScrollView), findsNWidgets(1));
      expect(find.byType(Column), findsNWidgets(2));
      expect(find.byType(Container), findsNWidgets(1));
      expect(find.byType(Row), findsNWidgets(1));
      expect(find.byType(Text), findsNWidgets(4));

      //look for back button
      expect(find.byKey(const ValueKey("backButton")), findsOneWidget);

      //find information text
      expect(find.byKey(const ValueKey("custom")), findsOneWidget);

      //find request text field
      expect(find.byKey(const ValueKey("requestPad")), findsOneWidget);
      //find request text field
      expect(find.byKey(const ValueKey("requestBox")), findsOneWidget);
      //find request text field
      expect(find.byKey(const ValueKey("request")), findsOneWidget);

      await tester.pump(const Duration(milliseconds: 100));
      final requestTxtField = find.byKey(const ValueKey("request"));
      await tester.ensureVisible(requestTxtField);
      await tester.tap(find.byKey(const ValueKey("request")));
      await tester.pumpAndSettle();

      //find  submit button first padding
      expect(find.byKey(const ValueKey("customRequestPad")), findsOneWidget);

      //tap the submit button
      await tester.tap(find.byKey(const ValueKey("customRequestButton")));
      await tester.pump(const Duration(milliseconds: 100));
      final requestButton = find.byKey(const ValueKey("customRequestButton"));
      await tester.ensureVisible(requestButton);
      await tester.tap(find.byKey(const ValueKey("customRequestButton")));
      await tester.pumpAndSettle();
    });
  });
  testWidgets('Test Back Button', (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Load screen
      await tester.pumpWidget(const MaterialApp(home: CustomRequest()));

      //tap the log hours button
      await tester.tap(find.byKey(const ValueKey("backButton")));
    });
  });
  testWidgets('Theme is Blue', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      CustomRequest.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: CustomRequest()));
    });
  });
  testWidgets('Theme is Pink', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      CustomRequest.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: CustomRequest()));
    });
  });
  testWidgets('Theme is Green', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      CustomRequest.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: CustomRequest()));
    });
  });
  testWidgets('Theme is Orange', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      CustomRequest.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: CustomRequest()));
    });
  });
}
