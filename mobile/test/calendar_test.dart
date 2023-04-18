import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:mobile/calendar.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:flutter/material.dart';
import 'package:mobile/utils/api_service.dart';
import 'package:mobile/utils/constants.dart';
import 'package:mockito/mockito.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:table_calendar/src/widgets/calendar_header.dart';
import 'package:table_calendar/src/widgets/format_button.dart';
import 'package:mobile/utils/calendar_utils.dart';
import 'package:table_calendar/src/customization/header_style.dart';
import 'package:table_calendar/src/shared/utils.dart';
import 'package:intl/intl.dart' as intl;
import 'package:table_calendar/src/widgets/custom_icon_button.dart';

import 'api_service_test.mocks.dart';

final focusedMonth = DateTime.utc(2022, 11, 24);

Widget setupTestWidget({
  HeaderStyle headerStyle = const HeaderStyle(),
  VoidCallback? onLeftArrowTap,
  VoidCallback? onRightArrowTap,
  VoidCallback? onHeaderTap,
  VoidCallback? onHeaderLongPress,
  Function(CalendarFormat)? onFormatButtonTap,
  Map<CalendarFormat, String> availableCalendarFormats = const {
    CalendarFormat.month: 'Month',
    CalendarFormat.twoWeeks: '2 weeks',
    CalendarFormat.week: 'Week'
  },
}) {
  return Directionality(
    textDirection: TextDirection.ltr,
    child: Material(
      child: CalendarHeader(
        focusedMonth: focusedMonth,
        calendarFormat: CalendarFormat.month,
        headerStyle: headerStyle,
        onLeftChevronTap: () => onLeftArrowTap?.call(),
        onRightChevronTap: () => onRightArrowTap?.call(),
        onHeaderTap: () => onHeaderTap?.call(),
        onHeaderLongPress: () => onHeaderLongPress?.call(),
        onFormatButtonTap: (format) => onFormatButtonTap?.call(format),
        availableCalendarFormats: availableCalendarFormats,
      ),
    ),
  );
}

void main() {
  testWidgets(
    'Displays the correct focused month and year for given focusedMonth',
    (tester) async {
      await tester.pumpWidget(setupTestWidget());

      final headerText = intl.DateFormat.yMMMM().format(focusedMonth);

      expect(find.byType(CalendarHeader), findsOneWidget);
      expect(find.text(headerText), findsOneWidget);
    },
  );
  testWidgets(
    'Ensure arrows and FormatButton are visible and test the onTap callbacks',
    (tester) async {
      bool leftArrowTapped = false;
      bool rightArrowTapped = false;
      bool headerTapped = false;
      bool formatButtonTapped = false;

      await tester.pumpWidget(
        setupTestWidget(
          onLeftArrowTap: () => leftArrowTapped = true,
          onRightArrowTap: () => rightArrowTapped = true,
          onHeaderTap: () => headerTapped = true,
          onFormatButtonTap: (_) => formatButtonTapped = true,
        ),
      );

      final leftChevron = find.widgetWithIcon(
        CustomIconButton,
        Icons.chevron_left,
      );

      final rightChevron = find.widgetWithIcon(
        CustomIconButton,
        Icons.chevron_right,
      );

      final header = find.byType(CalendarHeader);
      final formatButton = find.byType(FormatButton);

      expect(leftChevron, findsOneWidget);
      expect(rightChevron, findsOneWidget);
      expect(header, findsOneWidget);
      expect(formatButton, findsOneWidget);

      expect(leftArrowTapped, false);
      expect(rightArrowTapped, false);
      expect(headerTapped, false);
      expect(formatButtonTapped, false);

      await tester.tap(leftChevron);
      await tester.pumpAndSettle();

      await tester.tap(rightChevron);
      await tester.pumpAndSettle();

      await tester.tap(header);
      await tester.pumpAndSettle();

      await tester.tap(formatButton);
      await tester.pumpAndSettle();

      expect(leftArrowTapped, true);
      expect(rightArrowTapped, true);
      expect(headerTapped, true);
      expect(formatButtonTapped, true);
    },
  );

  testWidgets('Ensure the calendar page is complete',
      (WidgetTester tester) async {
    const Calendar calendarPage = Calendar();
    const app = MediaQuery(
        data: MediaQueryData(), child: MaterialApp(home: calendarPage));

    createUserSchedule('');

    await tester.pumpWidget(app);

    final header = find.byType(CalendarHeader);
    final formatButton = find.byType(FormatButton);
    final eventList = find.byKey(const Key("eventListKey"));

    expect(header, findsOneWidget);
    expect(formatButton, findsOneWidget);
    expect(eventList, findsOneWidget);
  });

  testWidgets('Test the _getEventsForRange method',
      (WidgetTester tester) async {
    const String scheduleData = '[{"id":1,"Date":"2022-11-21T00:00:00",'
        '"StartTime":"2022-11-21T09:00:00","EndTime":"2022-11-21T17:00:00",'
        '"userName":"Dushdush tremblay", '
        '"userId":"AAA_000002",'
        '"AllowedBreakTime"'
        ':1,"position":"goalie"}]';
    createUserSchedule(scheduleData);
    Calendar.focusedDay = DateTime(2022, 11, 22);
    const Calendar calendarPage = Calendar();
    const app = MediaQuery(
        data: MediaQueryData(), child: MaterialApp(home: calendarPage));
    await tester.pumpWidget(app);
    final headerText = intl.DateFormat.yMMMM().format(focusedMonth);
    const formatButtonText = "Month";
    final leftChevron = find.byKey(const Key("leftChevron"));
    final rightChevron = find.byKey(const Key("rightChevron"));

    expect(leftChevron, findsOneWidget);
    expect(rightChevron, findsOneWidget);

    expect(find.text(headerText), findsOneWidget);
    expect(find.text(formatButtonText), findsOneWidget);
    expect(find.text("M"), findsAtLeastNWidgets(1));
    expect(find.text("T"), findsAtLeastNWidgets(2));
    expect(find.text("W"), findsAtLeastNWidgets(1));
    expect(find.text("F"), findsAtLeastNWidgets(1));
    expect(find.text("S"), findsAtLeastNWidgets(2));

    await tester.tap(find.text("23"));

    await tester.longPress(find.text("23"));
    await tester.tap(find.text("26"));
    await tester.longPress(find.text("21"));
  });
  testWidgets('Theme is Blue', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      Calendar.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: Calendar()));
    });
  });
  testWidgets('Theme is Pink', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      Calendar.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: Calendar()));
    });
  });
  testWidgets('Theme is Green', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      Calendar.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: Calendar()));
    });
  });

  testWidgets('Theme is Orange', (WidgetTester tester) async {
    await tester.runAsync(() async {
      final client = MockClient();
      Calendar.httpClient = client;
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

      await tester.pumpWidget(const MaterialApp(home: Calendar()));
    });
  });
}
