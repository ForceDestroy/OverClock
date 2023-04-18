import 'dart:collection';

import 'package:flutter/material.dart';
import 'package:mobile/themes/colors.dart';
import 'package:table_calendar/table_calendar.dart';
import 'dart:convert';
import "package:collection/collection.dart";

/// Example event class.
class Event {
  final DateTime date;
  final DateTime startDate;
  final DateTime endDate;
  final Color categoryColor;
  final String breakTime;

  const Event(this.date, this.startDate, this.endDate, this.categoryColor,
      this.breakTime);

  String getStringTime() => '${startDate.hour.toString()}:'
      '${startDate.minute.toString().padLeft(2, "0")}-'
      '${endDate.hour.toString()}:${endDate.minute.toString().padLeft(2, "0")}';
}

late Map<DateTime, List<Event>> eventDayMap;
// ignore: prefer_typing_uninitialized_variables
//late var kEvents;
var kEvents = <DateTime, List<Event>>{};

void createUserSchedule(String scheduleData) {
  if (scheduleData.isEmpty) {
    kEvents = <DateTime, List<Event>>{};
    return;
  }
  // print(scheduleData);
  final jsonScheduleData = jsonDecode(scheduleData);
  final scheduleList = <Event>[];

  for (final event in jsonScheduleData) {
    final DateTime date = DateTime.parse(event['Date']);
    final DateTime startTime = DateTime.parse(event['StartTime']);
    final DateTime endTime = DateTime.parse(event['EndTime']);
    const Color categoryColor = MyColors.customPink;
    final String breakTime = event['AllowedBreakTime'].toString();
    scheduleList.add(Event(date, startTime, endTime, categoryColor, breakTime));
  }

  //create map
  eventDayMap = groupBy(scheduleList, (Event e) => e.date);

  kEvents = LinkedHashMap<DateTime, List<Event>>(
    equals: isSameDay,
    hashCode: getHashCode,
  )..addAll(eventDayMap);
}

int getHashCode(DateTime key) {
  return key.day * 1000000 + key.month * 10000 + key.year;
}

/// Returns a list of [DateTime] objects from [first] to [last], inclusive.
List<DateTime> daysInRange(DateTime first, DateTime last) {
  final dayCount = last.difference(first).inDays + 1;
  return List.generate(
    dayCount,
    (index) => DateTime.utc(first.year, first.month, first.day + index),
  );
}

var kToday = DateTime.now();
var kFirstDay = DateTime(kToday.year, kToday.month - 3, kToday.day);
var kLastDay = DateTime(kToday.year, kToday.month + 3, kToday.day);
