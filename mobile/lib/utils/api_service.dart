import 'package:http/http.dart' as http;
import 'constants.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:convert';
import 'dart:async' show Future;
import 'package:flutter/services.dart' show rootBundle;
import 'package:encrypt/encrypt.dart';

//Variable to store key for encryption/decryption
String aesKey = " ";

Future<String> readJson() async {
  final String response =
      await rootBundle.loadString('assets/network_resources.json');
  final data = await json.decode(response);
  return (data["url"]);
}

//Function that loads the key for encryption/decrytion
Future<String> loadAsset() async {
  aesKey = await rootBundle.loadString('assets/TLSKey.txt');
  return aesKey;
}

encrypt(String body, [bool testing = false]) {
  if (testing) {
    final key = Key.fromUtf8(aesKey);
    final iv = IV.fromUtf8('0000000000000000');
    final encrypter = Encrypter(AES(key, mode: AESMode.cbc));
    final encrypted = encrypter.encrypt(body, iv: iv);
    final encryptedBody = jsonEncode(encrypted.base64);
    return encryptedBody;
  }
  final key = Key.fromUtf8(aesKey);
  final iv = IV.fromUtf8('0000000000000000');
  final encrypter = Encrypter(AES(key, mode: AESMode.cbc));
  final encrypted = encrypter.encrypt(body, iv: iv);
  final encryptedBody = jsonEncode({'Data': encrypted.base64});
  //print(encryptedBody);
  return encryptedBody;
}

decrypt(String body) {
  final key = Key.fromUtf8(aesKey);
  final iv = IV.fromUtf8('0000000000000000');
  final encrypter = Encrypter(AES(key, mode: AESMode.cbc));
  final decrypted = encrypter.decrypt(Encrypted.fromBase64(body), iv: iv);
  final decodedData = json.decode(decrypted);
  return decodedData;
}

class ApiService {
  // refactored
  Future<int> login(String email, password, http.Client client) async {
    loadAsset();
    final prefs = await SharedPreferences.getInstance();
    final String baseUrl = await readJson();
    final body = jsonEncode({'email': email, 'password': password});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.validateLoginEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      //print(response.statusCode);
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body.toString());
        final encodedData = decrypt(data);
        await prefs.setString('token', encodedData['Token']);
        //print('Login successfully');
        return 200;
      } else {
        //print('Login failed');
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

  Future<int> logout(http.Client client, String? token) async {
    loadAsset();
    //final prefs = await SharedPreferences.getInstance();
    final String baseUrl = await readJson();
    final body = jsonEncode({'token': token});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.logoutEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      //print(response.statusCode);
      if (response.statusCode == 200) {
        //print('Logout successfully');
        return 200;
      } else {
        //print('Logout failed');
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

  // refactored
  Future<String> getAccountInfo(http.Client client, String? token) async {
    final prefs = await SharedPreferences.getInstance();
    final String baseUrl = await readJson();
    final body = jsonEncode({'sessionToken': token});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.getAccountInfoEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body.toString());
        final encodedData = decrypt(data);
        if (encodedData['ThemeColor'] != null) {
          await prefs.setInt('theme', encodedData['ThemeColor']);
        }
        //print(prefs.getInt('theme'));
        //print(encodedData);
        // print('Get account info successfully');
        return jsonEncode(encodedData);
      } else {
        // print('Get account info failed');
        return "";
      }
    } on Exception {
      return "";
    }
  }

  //refactored
  Future<String> getAllSchedule(http.Client client, String? token) async {
    final String baseUrl = await readJson();
    final body = jsonEncode({'sessionToken': token});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.getAssignedSchedule}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body.toString());
        final encodedData = decrypt(data);
        //print(encodedData);
        return jsonEncode(encodedData);
      } else {
        // print('getAllSchedule failed');
        return "";
      }
    } on Exception {
      return "";
    }
  }

  //refactored
  Future<int> updateAccountInfo(
      String body, http.Client client, String? token) async {
    final tempData = jsonDecode(body);
    final String baseUrl = await readJson();
    body = jsonEncode({'sessionToken': token, 'newUser': tempData});
    // print(body);
    // print(jsonDecode(body));
    final encryptedBody = encrypt(body);
    try {
      // ignore: lines_longer_than_80_chars
      final url = Uri.parse('$baseUrl${BaseAPI.updateAccountInfoEndpoint}');
      final response = await client.put(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      //print(response);
      if (response.statusCode == 200 || response.statusCode == 204) {
        // print('Update account info successfully');
        return 200;
      } else {
        //print('Update account info failed');
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

//refactored
  Future<String> getHours(
      http.Client client, DateTime day, String? token) async {
    final String baseUrl = await readJson();
    final temp = day.toLocal().add(const Duration(hours: 5));
    final body = jsonEncode({'sessionToken': token, 'day': temp.toString()});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.getHoursEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body.toString());
        final decryptedData = decrypt(data);
        //print(decryptedData);
        //print('Get hours successfully');
        return jsonEncode(decryptedData);
      } else {
        // print('Get hours failed');
        return "";
      }
    } on Exception {
      return "error";
    }
  }

//refactored
  Future<int> logHours(String body, http.Client client, String? token) async {
    final String baseUrl = await readJson();
    final tempData = jsonDecode(body);
    body = jsonEncode({'sessionToken': token, 'submission': tempData});
    final encryptedBody = encrypt(body);
    try {
      // ignore: lines_longer_than_80_chars
      final url = Uri.parse('$baseUrl${BaseAPI.logHoursEndpoint}');
      final response = await client.put(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200) {
        //print('Logged hours successfully.');
        return 200;
      } else {
        // print('Failed to log hours');
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

//refactored
  Future<int> updateTheme(
      int themeColor, http.Client client, String? token) async {
    final String baseUrl = await readJson();
    final body = jsonEncode({'sessionToken': token, 'themeColor': themeColor});
    final encryptedBody = encrypt(body);
    try {
      // ignore: lines_longer_than_80_chars
      final url = Uri.parse('$baseUrl${BaseAPI.updateThemeEndpoint}');
      final response = await client.put(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200) {
        //print('theme successfully');
        return 200;
      } else {
        //print('Update theme info failed');
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

//refactored
  Future<int> timeOffRequest(
      String body, http.Client client, String? token) async {
    final String baseUrl = await readJson();
    final tempData = jsonDecode(body);
    body = jsonEncode({'sessionToken': token, 'requestInfo': tempData});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.timeOffRequestEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200) {
        //print('Requested time off successfully.');
        return 200;
      } else {
        //print('Failed to request time off');
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

  //refactored
  Future<int> customRequest(
      String body, http.Client client, String? token) async {
    final String baseUrl = await readJson();
    final tempData = jsonDecode(body);
    body = jsonEncode({'sessionToken': token, 'requestInfo': tempData});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.customRequestEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200 || response.statusCode == 204) {
        //print("sent successful");
        return 200;
      } else {
        //print("sent failed");
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

//Refactored
  Future<dynamic> getPayslips(http.Client client, String? token) async {
    final String baseUrl = await readJson();
    final body = jsonEncode({'sessionToken': token});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.getPayslips}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body.toString());
        final decryptedData = decrypt(data);
        //print('getPayslips successful');
        // print(data);
        return [200, jsonEncode(decryptedData)];
      } else {
        //print('getPayslips failed');
        return [400, "getPayslips failed: response.statusCode != 200"];
      }
    } on Exception {
      return [-1, "getPayslips failed: Exception caught"];
    }
  }

//Refactored
  Future<int> scheduleChange(
      String body, http.Client client, String? token) async {
    final String baseUrl = await readJson();
    final tempData = jsonDecode(body);
    body = jsonEncode({'sessionToken': token, 'requestInfo': tempData});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.scheduleChangeEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      if (response.statusCode == 200 || response.statusCode == 204) {
        //print("sent successful");
        return 200;
      } else {
        //print("sent failed");
        return 400;
      }
    } on Exception {
      return -1;
    }
  }

//Refactored
  Future<List> getTimeSheetData(
      http.Client client, DateTime date, String? token) async {
    final String baseUrl = await readJson();
    final body = jsonEncode({'sessionToken': token, 'date': date.toString()});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.getTimeSheetEndpoint}');
      final response = await client.post(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      //print(response.statusCode);
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body.toString());
        final decryptedData = decrypt(data);
        //print(data);
        //print(decryptedData);
        //print('Get Timesheet successfully');
        return decryptedData;
      } else {
        // print('Get Timesheet failed');
        // print(response);
        return List.empty();
      }
    } on Exception {
      return List.empty();
    }
  }

  //refactored
  Future<int> submitTime(String body, http.Client client, String? token) async {
    final String baseUrl = await readJson();
    // print("this is the body");
    // print(body);
    final tempData = jsonDecode(body);
    body = jsonEncode({'sessionToken': token, 'submission': tempData});
    final encryptedBody = encrypt(body);
    try {
      final url = Uri.parse('$baseUrl${BaseAPI.submitTimeEndpoint}');
      final response = await client.put(url, body: encryptedBody, headers: {
        "Accept": "application/json",
        "content-type": "application/json"
      });
      //print(response.statusCode);
      if (response.statusCode == 200) {
        // print('Submitted timesheet successfully');
        return 200;
      } else {
        // print("sent failed");
        return 400;
      }
    } on Exception {
      return -1;
    }
  }
}
