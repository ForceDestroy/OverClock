import 'package:mobile/utils/calendar_utils.dart';
import 'package:mobile/themes/colors.dart';

import 'package:flutter_test/flutter_test.dart';

void main() {
  test('getStringTime() test', () {
    final Event eventTest = Event(
        DateTime(2022, 11, 21),
        DateTime(2022, 11, 21, 9, 30, 0, 0, 0),
        DateTime(2022, 11, 21, 12, 30, 0, 0, 0),
        MyColors.customGrey,
        "1");
    final String eventStringTime = eventTest.getStringTime();
    expect(eventStringTime, '9:30-12:30');
  });

  test(
      'createUserSchedule() test with empty string,'
      ' initialize the kEvents value and returns', () {
    const String scheduleData = '';
    createUserSchedule(scheduleData);
    expect(kEvents, {});
  });

  test(
      'createUserSchedule() test with'
      ' valid schedule string, '
      'fill the kEvents values', () {
    const String scheduleData = '[{"id":1,"Date":"2022-11-21T00:00:00",'
        '"StartTime":"2022-11-21T09:00:00"'
        ',"EndTime":"2022-11-21T17:00:00","userName":"Dushdush tremblay", '
        '"userId":"AAA_000002","AllowedBreakTime":1,"position":"goalie"}]';
    createUserSchedule(scheduleData);
    final Event expectedEvent = Event(
        DateTime.parse('2022-11-21T00:00:00'),
        DateTime.parse('2022-11-21T09:00:00'),
        DateTime.parse('2022-11-21T17:00:00'),
        MyColors.customPink,
        "1");
    final Map<DateTime, List<Event>> expectedEventsMap = {
      DateTime.parse('2022-11-21T00:00:00'): [expectedEvent]
    };

    expect(expectedEventsMap['2022-11-21T00:00:00'],
        kEvents['2022-11-21T00:00:00']);
  });

  test(
      'daysInRange() test with valid first day and last day,'
      ' Returns a list of [DateTime] objects from [first] to [last]', () {
    final daysInRangeValues =
        daysInRange(DateTime(2022, 11, 21), DateTime(2022, 11, 26));
    final List<DateTime> expectedDaysInRange = [
      DateTime.parse('2022-11-21 00:00:00.000Z'),
      DateTime.parse('2022-11-22 00:00:00.000Z'),
      DateTime.parse('2022-11-23 00:00:00.000Z'),
      DateTime.parse('2022-11-24 00:00:00.000Z'),
      DateTime.parse('2022-11-25 00:00:00.000Z'),
      DateTime.parse('2022-11-26 00:00:00.000Z')
    ];
    expect(expectedDaysInRange, daysInRangeValues);
  });
}
