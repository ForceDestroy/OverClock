// ignore_for_file: lines_longer_than_80_chars

import 'package:http/http.dart' as http;
import 'package:mockito/annotations.dart';
import 'package:mobile/utils/api_service.dart';
import 'package:test/test.dart';
import 'package:mockito/mockito.dart';
import 'package:flutter/material.dart';
import 'package:mobile/utils/constants.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:convert';
@GenerateMocks([http.Client])
import 'api_service_test.mocks.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  SharedPreferences.setMockInitialValues({});
  test(
      'Login api_service is called with empty email and password, '
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'email': "", 'password': ""});
    final encryptedBody = encrypt(body);

    when(client.post(Uri.parse('$baseUrl${BaseAPI.validateLoginEndpoint}'),
            headers: {
              "Accept": "application/json",
              "content-type": "application/json"
            },
            body: encryptedBody,
            encoding: null))
        .thenAnswer((_) async => http.Response('Not Found', 400));

    final int response = await api.login("", "", client);
    expect(response, 400);
  });
  test(
      'Login api_service is called with valid email and password,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body =
        jsonEncode({'email': "success@gmail.com", 'password': "password"});
    final encryptedBody = encrypt(body);
    final Map<String, dynamic> data = {
      'Id': 0,
      'Name': "Name",
      'AccessLevel': 0,
      'Token': "token123"
    };
    final String dataString = jsonEncode(data);
    final dynamic encryption = encrypt(dataString, true);

    when(client.post(Uri.parse('$baseUrl${BaseAPI.validateLoginEndpoint}'),
            headers: {
              "Accept": "application/json",
              "content-type": "application/json"
            },
            body: encryptedBody,
            encoding: null))
        .thenAnswer((_) async => http.Response(encryption, 200));

    final response = await api.login("success@gmail.com", "password", client);
    expect(response, 200);
  });

  test(
      'Logout api_service is called with empty token'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'token': ""});
    final encryptedBody = encrypt(body);

    when(client.post(Uri.parse('$baseUrl${BaseAPI.logoutEndpoint}'),
            headers: {
              "Accept": "application/json",
              "content-type": "application/json"
            },
            body: encryptedBody,
            encoding: null))
        .thenAnswer((_) async => http.Response('Not Found', 400));

    final int response = await api.logout(client, "");
    expect(response, 400);
  });

  test(
      'Logout api_service is called with valid token'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'token': "token123"});
    final encryptedBody = encrypt(body);

    when(client.post(Uri.parse('$baseUrl${BaseAPI.logoutEndpoint}'),
            headers: {
              "Accept": "application/json",
              "content-type": "application/json"
            },
            body: encryptedBody,
            encoding: null))
        .thenAnswer((_) async => http.Response('Logout Successful', 200));

    final int response = await api.logout(client, "token123");
    expect(response, 200);
  });

  test(
      'getAccountInfo api_service is called with valid token,'
      ' returns a strings with user info', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': "token123"});
    final encryptedBody = encrypt(body);
    final Map<String, dynamic> data = {
      'Id': 0,
      'Name': "Name",
      'AccessLevel': 0,
      'Token': "token123"
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

    final String response = await api.getAccountInfo(client, "token123");
    expect(
        response, '{"Id":0,"Name":"Name","AccessLevel":0,"Token":"token123"}');
  });

  test(
      'getAccountInfo api_service is called with empty token,'
      ' returns an empty string', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': ""});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}'),
            headers: {
              "Accept": "application/json",
              "content-type": "application/json"
            },
            body: encryptedBody,
            encoding: null))
        .thenAnswer((_) async => http.Response('Not Found', 404));
    final String response = await api.getAccountInfo(client, "");
    expect(response, '');
  });

  test(
      'updateAccountInfo api_service is called with valid token,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    const token = "token123";
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
    final body = jsonEncode({'sessionToken': token, 'newUser': userInfo});
    final encryptedBody = encrypt(body);
    when(client.put(Uri.parse('$baseUrl${BaseAPI.updateAccountInfoEndpoint}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer(
        (_) async => http.Response('User info updated succesfully', 200));
    final int response =
        await api.updateAccountInfo(jsonEncode(userInfo), client, "token123");
    expect(response, 200);
  });

  test(
      'updateAccountInfo api_service is called with empty token,'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    const token = "";
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
    final body = jsonEncode({'sessionToken': token, 'newUser': userInfo});
    final encryptedBody = encrypt(body);
    when(client.put(Uri.parse('$baseUrl${BaseAPI.updateAccountInfoEndpoint}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not Found', 404));

    final int response =
        await api.updateAccountInfo(jsonEncode(userInfo), client, "");
    expect(response, 400);
  });

  test(
      'updateTheme api_service is called with valid token,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': "token123", 'themeColor': 0});
    final encryptedBody = encrypt(body);
    when(client.put(Uri.parse('$baseUrl${BaseAPI.updateThemeEndpoint}'),
            headers: {
              "Accept": "application/json",
              "content-type": "application/json"
            },
            body: encryptedBody,
            encoding: null))
        .thenAnswer(
            (_) async => http.Response('Theme updated successfully', 200));

    final int response = await api.updateTheme(0, client, "token123");
    expect(response, 200);
  });

  test(
      'updateTheme api_service is called with empty token,'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': "", 'themeColor': 0});
    final encryptedBody = encrypt(body);
    when(client.put(Uri.parse('$baseUrl${BaseAPI.updateThemeEndpoint}'),
            headers: {
              "Accept": "application/json",
              "content-type": "application/json"
            },
            body: encryptedBody,
            encoding: null))
        .thenAnswer((_) async => http.Response('Not found', 404));

    final int response = await api.updateTheme(0, client, "");
    expect(response, 400);
  });
  test(
      'getAllSchedule api_service is called with valid token'
      ' and a schedule is assigned,'
      ' returns a string with assigned schedule', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': "token123"});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response(
        '"cTQjTyqHRq8fcFlcifSg4239I8f/iVyvttfUIjILFiPHvFJJ7EzVPh+SilopcZIy"',
        200));

    final String response = await api.getAllSchedule(client, "token123");
    expect(response, '{"startTime":"2022-11-21T09:00:00"}');
  });

  test(
      'getAllSchedule api_service is called with invalid or empty token ,'
      ' returns an empty string ', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': ""});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not Found', 404));

    final String response = await api.getAllSchedule(client, "");
    expect(response, '');
  });

  test(
      'getAllSchedule api_service is called with valid'
      ' token and no schedule is assigned ,'
      ' returns an empty string ', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': "token123"});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Bad Request', 400));

    final String response = await api.getAllSchedule(client, "token123");
    expect(response, '');
  });

  test(
      'logHours api_service is called with empty token,'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    when(client.put(Uri.parse('$baseUrl${BaseAPI.logHoursEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYYMkiikN8J9EQLMetFWRhaqPdgwz+aQTgseqGK7QaDt1mkn2/xGuTbmebf+P1jKligGQy/adtnlpBeBiYjkT/5eXhTEUiVyYDOF24F+MpidsuooH0Q/3Ni17HqpcTyRC7AvSDGALt0b/68a4bTW9zkbxHq42/if611TJ1MzTE+lPb/OyxE37xgy4EyavyK8Q2w=="}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not Found', 404));

    final int response = await api.logHours(
        '[{"Date":"2023-03-07T05:00:00.000Z","StartTime":"2023-03-07T12:00:00.000Z","EndTime":"2023-03-07T16:00:00.000Z"}]',
        client,
        "");
    expect(response, 400);
  });

  test(
      'logHours api_service is called with valid token,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    const token = "";
    final userInfo = {
      "Date": "2023-03-12T23:20:34.980158Z",
      "StartTime": "2023-03-12T11:00:00.000Z",
      "EndTime": "2023-03-12T20:00:00.000Z"
    };
    final body = jsonEncode({'sessionToken': "", 'submission': userInfo});
    final encryptedBody = encrypt(body);
    when(client.put(Uri.parse('$baseUrl${BaseAPI.logHoursEndpoint}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer(
        (_) async => http.Response('Logged hours succesfully', 200));

    final int response =
        await api.logHours(jsonEncode(userInfo), client, token);
    expect(response, 200);
  });

  test(
      'getHours api_service is called with valid token,'
      ' returns a strings with logged hours', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    const token = 'token123';
    final DateTime date1 = DateTime.now();
    final DateTime date2 = date1.toLocal().add(const Duration(hours: 5));
    final String date2String = date2.toString();
    final body = jsonEncode({'sessionToken': "token123", 'day': date2String});
    final encryptedBody = encrypt(body);
    const expected =
        '{"date":"2022-11-22T00:00:00","startTime":"2022-11-27T06:00:00","endTime":"2022-11-27T18:00:00"}';
    const r =
        '"k+iAYPIBF4q7kiRbtwnLtQwRYalco4TwK1QE+wZK864TgiEaHqGHLGEOVjF+3vf1yWfAs5C+miUpVnePFF25+vlH4b0zMIhquVPB19dn2eexRNzn+Do/cWaVmoVxy/CHGKMk9GbnoG5QTbJPmIGisw=="';
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response(r, 200));

    final String response = await api.getHours(client, date1, token);
    expect(response, expected);
  });

  test(
      'getHours api_service is called with empty token,'
      ' returns an empty string', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final DateTime date1 = DateTime.now();
    final DateTime date2 = date1.toLocal().add(const Duration(hours: 5));
    final String date2String = date2.toString();
    final body = jsonEncode({'sessionToken': "", 'day': date2String});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not Found', 404));

    final String response = await api.getHours(client, date1, "");
    expect(response, '');
  });

  test(
      'timeOffRequest api_service is called with valid token,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();

    when(client.post(Uri.parse('$baseUrl${BaseAPI.timeOffRequestEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYebcYdj5JP1SCdV2zCwk2SvQh0yGVEYiT+ZUP5IAIeV1wlcvdjqvCscv1WsH8uOPl40NoTBsDwZgDIMmdFKFcxV43Ib5n8zF4AGHL4PpEzsUWi9lXZh1FEZly1KIZRAmKtMh/ll+zKP8d4riz7hALDYe3rrVTi6EOAifWHL/pixOXp+6bG0xw/jUcAE/s5dVeMYtzloY9O35LrMBORynVq0wlej59gXzrjHyRUnStmgB"}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer(
        (_) async => http.Response('Sent time off request succesfully', 200));
    final int response = await api.timeOffRequest(
        '{"date":"2023-03-12T21:35:29.823114","startTime":"2023-03-14T00:00:00.000","endTime":"2023-03-14T00:00:00.000","body":" ","type":"Sick"}',
        client,
        "token123");
    expect(response, 200);
  });

  test(
      'timeOffRequest api_service is called with empty token,'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();

    when(client.post(Uri.parse('$baseUrl${BaseAPI.timeOffRequestEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYYw7riE3jyaufxmCBhIoBMd8y+C15r5gGavHGz9wpYLtU01B81ALixRkobev3V8vcsraaQjnN2K5OeVM0+duw6T5Q0liPQzuNueE4U6y6RMybORyizQY3Wxo/M0GpnjQ1XJEW4u2LKft7lMTlhUeojEw8iM6jWwA/0OyA8eFQjXGtA3rh1uNFWzkP6wXXtsOl12+ZOZ/lP3oYe8iTLBeteU="}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not found', 404));

    final int response = await api.timeOffRequest(
        '{"date":"2023-03-12T21:35:29.823114","startTime":"2023-03-14T00:00:00.000","endTime":"2023-03-14T00:00:00.000","body":" ","type":"Sick"}',
        client,
        "");
    expect(response, 400);
  });

  test(
      'customRequest api_service is called with valid token,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    when(client.post(Uri.parse('$baseUrl${BaseAPI.customRequestEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYebcYdj5JP1SCdV2zCwk2StSdvrDDQ0XC5xHNuIHY/YGERtHxwFKzz8hjTDqpmGZmmmBWqPCRoDatTzLNjBa+2R/MhYJ9/oEAfoJ/wZ77JxNbzIZTehvR3j3y5cpPmxSTXsR0BcB0+EyRL0Ds+PzJLno0bmFXhGM0NrIgvtWJryBhMYn4p4VXJxQHzpa96F5SVsqpojoCkdnT2Lc1hkXIL+XV2oODJ8NOSx2cEu7SYMu0MCcqq7mEw6l6//0Quh2cS0x2nsyGNwIr0ttBbM+LUVpJoiPyq1ZetbFUHSJ3C9S"}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer(
        (_) async => http.Response('Sent custom request succesfully', 200));
    final int response = await api.customRequest(
        '{"id":"0","fromId":"ZZZ","toId":"","body":"test","date":"2023-03-12T21:43:55.953965","startTime":"2023-03-12T21:43:55.953964","endTime":"2023-03-12T21:43:55.953966","status":"Pending"}',
        client,
        "token123");
    expect(response, 200);
  });

  test(
      'customRequest api_service is called with empty token,'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();

    when(client.post(Uri.parse('$baseUrl${BaseAPI.customRequestEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYYw7riE3jyaufxmCBhIoBMcECseb2c8emr4BCH0FfiBDfwdZMhyH9gCaNwLeEOFSGWW2XZq84gydLxNdswjsWgmIN1aqSDkuJahdzUblv98gjwOGJ/lOix570mDSYGk9B95tG0DN2bkthqs6xfXnnoy/JfM/RTtkwIM63MsXV7qQ665JudP6B3Oe8tVTAMPq011BulqbTUj06rH1z4Er1y9C7c770NvD2Dxwhu1fQZrrVO61QSckHrR82LI3I7pY8ZOAeJrSD5PBha21q8dJ0vY="}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not found', 404));

    final int response = await api.customRequest(
        '{"id":"0","fromId":"ZZZ","toId":"","body":"test","date":"2023-03-12T21:43:55.953965","startTime":"2023-03-12T21:43:55.953964","endTime":"2023-03-12T21:43:55.953966","status":"Pending"}',
        client,
        "");
    expect(response, 400);
  });
  test(
      'scheduleChangeRequest api_service is called with valid token,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    when(client.post(Uri.parse('$baseUrl${BaseAPI.scheduleChangeEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYebcYdj5JP1SCdV2zCwk2Sv+4QtApjad5AWWMYBc64lfLpHJ9A3RGZU8qBCVkTnmqbOtQAd6jdd+EU5LTHNeFxfTs1+tf/hwA5pt+N9CVJSKGgjXG5uBcLNclhFhQ90+uS7RwqDm+wK3AYomrhiDYJzuUmS5rUqBHElVmVLnxanKg5uo2A7QQOJaGV+uayHnhg=="}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async =>
        http.Response('Sent schedule change request succesfully', 200));
    final int response = await api.scheduleChange(
        '{"FromId":" ","Date":"2023-03-12T21:49:16.742331","StartTime":" ","EndTime":" ","Body":"test","Type":"Schedule"}',
        client,
        "token123");
    expect(response, 200);
  });

  test(
      'scheduleChangeRequest api_service is called with empty token,'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    when(client.post(
      Uri.parse('$baseUrl${BaseAPI.scheduleChangeEndpoint}'),
      headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      },
      body:
          '{"Data":"mLuL9JOSCbPseMJuKC4lYYw7riE3jyaufxmCBhIoBMcvzeyo0NiXsV4Lrezhi9aXjBiMqI2CMjinaZnRRHEr30oPI51zYy7oiCxU5spyGOEfOk6iu5eZc/CqAaNxhdLUCV5rmfXbEBoQdhJ0HpjKdkz0BvqNrrS2doG/98HebWGnOxbf+NWPSb7whymiDapY2h8kLiH253SpCZSoMj1aog=="}',
    )).thenAnswer((_) async => http.Response('Not found', 404));

    final int response = await api.scheduleChange(
        '{"FromId":" ","Date":"2023-03-12T21:49:16.742331","StartTime":" ","EndTime":" ","Body":"test","Type":"Schedule"}',
        client,
        "");
    expect(response, 400);
  });
  test(
      'getPayslips api_service is called with empty token, '
      'returns error message and appropriate status code', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': ""});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getPayslips}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not Found', 404));

    final dynamic response = await api.getPayslips(client, "");
    expect(response[0], 400);
    expect(response[1], 'getPayslips failed: response.statusCode != 200');
  });
  test(
      'getPayslips api_service is called with valid token, '
      'returns success message and appropriate status code', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final body = jsonEncode({'sessionToken': "token123"});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getPayslips}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response(
        '"YdERJG/uP9+1Ot0IxHVwarTuuBXlIfUsT9Z8xmXvn7lBIIrPEaKBqHpnkdqbOMD5y+urNOv9Nto1OT6dO4uT/HURFwMQGX006UjKrbjrEaU="',
        200));

    final dynamic response = await api.getPayslips(client, "token123");
    expect(response[0], 200);
    expect(response[1],
        '[{"name":"Dushdush truong","address":"0 Beach Avenue","salary":9.0}]');
  });
  test(
      'getTimesheet api_service is called with valid token,'
      ' returns a strings with timesheet', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final DateTime date = DateTime.now();
    final DateTime date2 = date.toLocal().add(const Duration(hours: 5));
    final String date2String = date2.toString();
    const r = 'r2ViU/m7kaKP7Z90fCv6K/0uIlF9HG0WBA+l+WLATQo=';
    final body = jsonEncode({'sessionToken': "token123", 'date': date2String});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getTimeSheetEndpoint}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response(r, 200));

    final List response = await api.getTimeSheetData(client, date2, "token123");
    expect(response, []);
  });

  test(
      'getTimeSheet api_service is called with empty token,'
      ' returns an empty string', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    final DateTime date = DateTime.now();
    final DateTime date2 = date.toLocal().add(const Duration(hours: 5));
    final String date2String = date2.toString();
    final body = jsonEncode({'sessionToken': "", 'date': date2String});
    final encryptedBody = encrypt(body);
    when(client.post(Uri.parse('$baseUrl${BaseAPI.getTimeSheetEndpoint}'),
        body: encryptedBody,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not Found', 404));

    final List response = await api.getTimeSheetData(client, date2, "");
    expect(response, []);
  });

  test(
      'submitTime api_service is called with valid token,'
      ' returns a 200 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    when(client.put(Uri.parse('$baseUrl${BaseAPI.submitTimeEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYdorGWU8Ri/M4SxsLB/h6YH96IRX98faHn0LblUoR84JTB4LimrTyMl/VKplrLgTmVD6GHZmNt8QdZZCNJhvSb+42z8XYKEAtrNLjO97GRdsiNysL5VzU1WJTPJdkGyhtW9HG3nPiHcr4WEv0MxQiieToEudf5HcglepEj449Jw2xMe/ZlUo6hpl7nINQvKVtw=="}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer(
        (_) async => http.Response('submitted timesheet succesfully', 200));

    final int response = await api.submitTime(
        '[{"Date":"2023-03-12T05:00:00.000Z","StartTime":"2023-03-12T11:00:00.000Z","EndTime":"2023-03-12T20:00:00.000Z"}]',
        client,
        "token123");
    expect(response, 200);
  });

  test(
      'submitTime api_service is called with empty token,'
      ' returns a 400 status', () async {
    final String baseUrl = await readJson();
    final ApiService api = ApiService();
    await loadAsset();
    final client = MockClient();
    when(client.put(Uri.parse('$baseUrl${BaseAPI.submitTimeEndpoint}'),
        body:
            '{"Data":"mLuL9JOSCbPseMJuKC4lYYMkiikN8J9EQLMetFWRhaqPdgwz+aQTgseqGK7QaDt1lhdm3xfJ2YxyPhizsR0UUzj7c8boJ/hdzHYSzXijtutfYyg8TZoq/+nOKG/8RX0HIL6xmmiL2ujJvOC1mDzNHJlrAzmKmfgJBs1iolHpsvKbRMiVuWYvIdHqD5c6r3RwUKpb4Kgz0VQtDWJ3huwjSA=="}',
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        })).thenAnswer((_) async => http.Response('Not found', 404));

    final int response = await api.submitTime(
        '[{"Date":"2023-03-12T05:00:00.000Z","StartTime":"2023-03-12T11:00:00.000Z","EndTime":"2023-03-12T20:00:00.000Z"}]',
        client,
        "");
    expect(response, 400);
  });
}
