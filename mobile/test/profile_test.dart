// This is a basic Flutter widget test.
//
// To perform an interaction with a widget in your test, use the WidgetTester
// utility in the flutter_test package. For example, you can send tap and scroll
// gestures. You can also use WidgetTester to find child widgets in the widget
// tree, read text, and verify that the values of widget properties are correct.

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/profile_page.dart';
import 'api_service_test.mocks.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/utils/api_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'package:mockito/mockito.dart';
import 'package:mobile/utils/constants.dart';

void main() {
  testWidgets('Profile Page Test - General', (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Define and initialize variables
      final client = MockClient();
      ProfilePage.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      const String token = 'token123';
      await prefs.setString('token', token);

      //Load Key
      await loadAsset();

      //getAccountInfo API call
      final body1 = jsonEncode({'sessionToken': token});
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

      //UpdateAccountInfo API call
      final userInfo = {
        "Id": "AAA_000002",
        "Name": "DushDush Manick",
        "Email": "test2@gmail.com",
        "Password": "ZG2m+Et5dIE/twSPbWuKoA==",
        "Address": "2 Beach Avenue",
        "Birthday": "0001-01-01T00:00:00",
        "PhoneNumber": 5146874987,
        "ThemeColor": 0
      };
      final body6 = jsonEncode({'sessionToken': token, 'newUser': userInfo});
      final encryptedBody6 = encrypt(body6);
      when(client.put(Uri.parse('$baseUrl${BaseAPI.updateAccountInfoEndpoint}'),
          body: encryptedBody6,
          headers: {
            "Accept": "application/json",
            "content-type": "application/json"
          })).thenAnswer(
          (_) async => http.Response('User info updated succesfully', 200));

      //Logout API call
      final body7 = jsonEncode({'token': token});
      final encryptedBody7 = encrypt(body7);
      when(client.post(Uri.parse('$baseUrl${BaseAPI.logoutEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody7,
              encoding: null))
          .thenAnswer((_) async => http.Response('Logout Successful', 200));

      await tester.pumpWidget(const MaterialApp(home: ProfilePage()));

      //Verify that there is a clipPath for the header
      expect(find.byType(ClipPath), findsOneWidget);

      //Verify that profile pic exists
      expect(find.byType(CircleAvatar), findsNWidgets(2));

      // Verify that 6 textformfields are present
      expect(find.byType(TextFormField), findsNWidgets(6));

      //Verify that Icons exist for the textformfields
      expect(find.byType(Image), findsNWidgets(6));

      //Verify that refresh icon exists
      expect(find.byType(Icon), findsNWidgets(2));

      //Verify that change theme text is there
      expect(find.text('Change Theme'), findsOneWidget);

      await tester.pump();

      //Verify Edit/Save
      expect(find.byKey(const ValueKey("editProfile")), findsOneWidget);
      await tester.ensureVisible(find.byKey(const ValueKey("editProfile")));
      await tester.pumpAndSettle();
      await tester.tap(find.byKey(const ValueKey("editProfile")));
      await tester.pump();
      final nameField = find.byKey(const Key('name'));
      await tester.enterText(nameField, 'Dushdush tremblay');
      await tester.testTextInput.receiveAction(TextInputAction.done);
      await tester.pump();
      await tester.ensureVisible(find.byKey(const ValueKey("save")));
      await tester.pumpAndSettle();
      expect(find.byKey(const ValueKey("save")), findsOneWidget);
      //await tester.tap(find.byKey(const ValueKey("save")));
      await tester.pump();

      //Verify Logout
      expect(find.byKey(const ValueKey("logOut")), findsOneWidget);
      await tester.ensureVisible(find.byKey(const ValueKey("logOut")));
      await tester.pumpAndSettle();
      await tester.tap(find.byKey(const ValueKey("logOut")));
      await tester.pump();
    });
  });
  testWidgets('Profile Page Test - Update to Blue Theme',
      (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Define and initialize variables
      final client = MockClient();
      ProfilePage.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      const String token = 'token123';
      await prefs.setString('token', token);

      //Load Key
      await loadAsset();

      //getAccountInfo API call
      final body1 = jsonEncode({'sessionToken': token});
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

      //Update to Blue Theme API call
      final body2 = jsonEncode({'sessionToken': "token123", 'themeColor': 0});
      final encryptedBody2 = encrypt(body2);
      when(client.put(Uri.parse('$baseUrl${BaseAPI.updateThemeEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody2,
              encoding: null))
          .thenAnswer(
              (_) async => http.Response('Theme updated successfully', 200));

      await tester.pumpWidget(const MaterialApp(home: ProfilePage()));

      //Change theme to Blue
      final blueTheme = find.byKey(const ValueKey("blueTheme"));
      await tester.pump();
      await tester.ensureVisible(blueTheme);
      await tester.pumpAndSettle();
      await tester.tap(blueTheme);
    });
  });
  testWidgets('Profile Page Test - Update to Pink Theme',
      (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Define and initialize variables
      final client = MockClient();
      ProfilePage.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      const String token = 'token123';
      await prefs.setString('token', token);

      //Load Key
      await loadAsset();

      //getAccountInfo API call
      final body1 = jsonEncode({'sessionToken': token});
      final encryptedBody1 = encrypt(body1);
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody1,
              encoding: null))
          .thenAnswer((_) async => http.Response(
              '"zvaDV0NdO+2o8DdoFL/kaaVbWNZQp1+N2v82BvAdjz0VRMqwQjRE/tyoSx2bA42Q6zLgZgEdSajgRNePt9DaXRjr9ae97T5Cc88H929ZlUJ6CBhZoiOFeMYaeL/rQkPsdd6TPuaTDDYHu5irNoxGVKjBSM3I+2O8mhYAaLKf2bejX8xXuOh5ARcpx7qMhyi33TCgZLKyH8w/7FBUwBQvI2HuVECHdwDtOEOIkJeut6Zcigl8xnP56V4OMBEgzmFbuH6/jaeq7Gmrsb7YnxknPYSAPpDGnHjdFSQ8eEqZXkJ/9qmb9r1zGxsUpVvOpSJ6GoflJDfAeUrUtVlUJ5iuiA=="',
              200));

      //Update to Pink Theme API call
      final body2 = jsonEncode({'sessionToken': "token123", 'themeColor': 1});
      final encryptedBody2 = encrypt(body2);
      when(client.put(Uri.parse('$baseUrl${BaseAPI.updateThemeEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody2,
              encoding: null))
          .thenAnswer(
              (_) async => http.Response('Theme updated successfully', 200));

      await tester.pumpWidget(const MaterialApp(home: ProfilePage()));

      //Change theme to Pink
      final pinkTheme = find.byKey(const ValueKey("pinkTheme"));
      await tester.pump();
      await tester.ensureVisible(pinkTheme);
      await tester.pumpAndSettle();
      await tester.tap(pinkTheme);
    });
  });
  testWidgets('Profile Page Test - Update to Green Theme',
      (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Define and initialize variables
      final client = MockClient();
      ProfilePage.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      const String token = 'token123';
      await prefs.setString('token', token);

      //Load Key
      await loadAsset();

      //getAccountInfo API call
      final body1 = jsonEncode({'sessionToken': token});
      final encryptedBody1 = encrypt(body1);
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody1,
              encoding: null))
          .thenAnswer((_) async => http.Response(
              '"zvaDV0NdO+2o8DdoFL/kaaVbWNZQp1+N2v82BvAdjz0VRMqwQjRE/tyoSx2bA42Q6zLgZgEdSajgRNePt9DaXRjr9ae97T5Cc88H929ZlUJ6CBhZoiOFeMYaeL/rQkPsdd6TPuaTDDYHu5irNoxGVKjBSM3I+2O8mhYAaLKf2bejX8xXuOh5ARcpx7qMhyi33TCgZLKyH8w/7FBUwBQvI2HuVECHdwDtOEOIkJeut6Zcigl8xnP56V4OMBEgzmFbAhbY9blq8rWU1niE6Ha4G7ViE12HR7BSghRCebml5Iw7n9hiRM3g31/RZOwciSrFjINzs1npKOnKaTYpYA8fYA=="',
              200));

      //Update to Green Theme API call
      final body2 = jsonEncode({'sessionToken': "token123", 'themeColor': 2});
      final encryptedBody2 = encrypt(body2);
      when(client.put(Uri.parse('$baseUrl${BaseAPI.updateThemeEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody2,
              encoding: null))
          .thenAnswer(
              (_) async => http.Response('Theme updated successfully', 200));

      await tester.pumpWidget(const MaterialApp(home: ProfilePage()));

      //Change theme to Green
      final greenTheme = find.byKey(const ValueKey("greenTheme"));
      await tester.pump();
      await tester.ensureVisible(greenTheme);
      await tester.pumpAndSettle();
      await tester.tap(greenTheme);
    });
  });
  testWidgets('Profile Page Test - Update to Orange Theme',
      (WidgetTester tester) async {
    await tester.runAsync(() async {
      //Define and initialize variables
      final client = MockClient();
      ProfilePage.httpClient = client;
      final String baseUrl = await readJson();
      SharedPreferences.setMockInitialValues({});
      final SharedPreferences prefs = await SharedPreferences.getInstance();
      const String token = 'token123';
      await prefs.setString('token', token);

      //Load Key
      await loadAsset();

      //getAccountInfo API call
      final body1 = jsonEncode({'sessionToken': token});
      final encryptedBody1 = encrypt(body1);
      when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody1,
              encoding: null))
          .thenAnswer((_) async => http.Response(
              '"zvaDV0NdO+2o8DdoFL/kaaVbWNZQp1+N2v82BvAdjz0VRMqwQjRE/tyoSx2bA42Q6zLgZgEdSajgRNePt9DaXRjr9ae97T5Cc88H929ZlUJ6CBhZoiOFeMYaeL/rQkPsdd6TPuaTDDYHu5irNoxGVKjBSM3I+2O8mhYAaLKf2bejX8xXuOh5ARcpx7qMhyi33TCgZLKyH8w/7FBUwBQvI2HuVECHdwDtOEOIkJeut6Zcigl8xnP56V4OMBEgzmFbfuMypg0mo74AZKL1sy3Y5ZNBYdU8J5gQV9ewDeYUebOwTbuc87DwRCvR+sWM07HHqz7UD2guFOEvSDMH21qdXA=="',
              200));

      //Update to Orange Theme API call
      final body2 = jsonEncode({'sessionToken': "token123", 'themeColor': 3});
      final encryptedBody2 = encrypt(body2);
      when(client.put(Uri.parse('$baseUrl${BaseAPI.updateThemeEndpoint}'),
              headers: {
                "Accept": "application/json",
                "content-type": "application/json"
              },
              body: encryptedBody2,
              encoding: null))
          .thenAnswer(
              (_) async => http.Response('Theme updated successfully', 200));

      await tester.pumpWidget(const MaterialApp(home: ProfilePage()));

      //Change theme to Orange
      final orangeTheme = find.byKey(const ValueKey("orangeTheme"));
      await tester.pump();
      await tester.ensureVisible(orangeTheme);
      await tester.pumpAndSettle();
      await tester.tap(orangeTheme);
    });
  });
}
