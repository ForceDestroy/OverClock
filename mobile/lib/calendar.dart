// Copyright 2019 Aleksander Woźniak
// SPDX-License-Identifier: Apache-2.0

import 'package:flutter/material.dart';
import 'package:mobile/utils/api_service.dart';
import 'package:table_calendar/table_calendar.dart';
import 'package:http/http.dart' as http;
import '../utils/calendar_utils.dart';
import 'themes/my_clipper.dart';
import 'themes/colors.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';

class Calendar extends StatefulWidget {
  static DateTime? focusedDay;
  static http.Client? httpClient;
  const Calendar({Key? key}) : super(key: key);

  @override
  // ignore: library_private_types_in_public_api
  _Calendar createState() => _Calendar();
}

class _Calendar extends State<Calendar> {
  late final ValueNotifier<List<Event>> _selectedEvents;
  CalendarFormat _calendarFormat = CalendarFormat.month;
  RangeSelectionMode _rangeSelectionMode = RangeSelectionMode
      .toggledOff; // Can be toggled on/off by longpressing a date
  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;
  DateTime? _rangeStart;
  DateTime? _rangeEnd;
  late SharedPreferences prefs;
  var themeColor = 0;
  http.Client httpClient = http.Client();
  Color headerColor = Colors.white;
  Color selectionColor = Colors.white;

  void getFocusedDay(DateTime? day) {
    if (day != null) {
      _focusedDay = day;
      kToday = day;
      kFirstDay = DateTime(kToday.year, kToday.month - 3, kToday.day);
      kLastDay = DateTime(kToday.year, kToday.month + 3, kToday.day);
    }
  }

  @override
  void initState() {
    super.initState();
    setClient(Calendar.httpClient);
    getProfileInfo(httpClient);
    getFocusedDay(Calendar.focusedDay);
    _selectedDay = _focusedDay;
    _selectedEvents = ValueNotifier(_getEventsForDay(_selectedDay!));
  }

  @override
  void dispose() {
    _selectedEvents.dispose();
    super.dispose();
  }

  void setClient(http.Client? client) {
    if (client != null) {
      httpClient = client;
    }
  }

  List<Event> _getEventsForDay(DateTime day) {
    // Implementation example
    return kEvents[day] ?? [];
  }

  List<Event> _getEventsForRange(DateTime start, DateTime end) {
    // Implementation example
    final days = daysInRange(start, end);

    return [
      for (final d in days) ..._getEventsForDay(d),
    ];
  }

  void _onDaySelected(DateTime selectedDay, DateTime focusedDay) {
    if (!isSameDay(_selectedDay, selectedDay)) {
      setState(() {
        _selectedDay = selectedDay;
        _focusedDay = focusedDay;
        _rangeStart = null; // Important to clean those
        _rangeEnd = null;
        _rangeSelectionMode = RangeSelectionMode.toggledOff;
      });

      _selectedEvents.value = _getEventsForDay(selectedDay);
    }
  }

  void _onRangeSelected(DateTime? start, DateTime? end, DateTime focusedDay) {
    setState(() {
      _selectedDay = null;
      _focusedDay = focusedDay;
      _rangeStart = start;
      _rangeEnd = end;
      _rangeSelectionMode = RangeSelectionMode.toggledOn;
    });

    // `start` or `end` could be null
    if (start != null && end != null) {
      _selectedEvents.value = _getEventsForRange(start, end);
    } else if (start != null) {
      _selectedEvents.value = _getEventsForDay(start);
    } else if (end != null) {
      _selectedEvents.value = _getEventsForDay(end);
    }
  }

  void getProfileInfo(http.Client client) async {
    setState(() {});
    prefs = await SharedPreferences.getInstance();

    await ApiService().getAccountInfo(client, prefs.getString('token'));

    themeColor = prefs.getInt('theme')!;

    if (themeColor == 0) {
      headerColor = MyColors.customBlue;
      selectionColor = MyColors.customBlue2;
    } else if (themeColor == 1) {
      headerColor = MyColors.customPink;
      selectionColor = MyColors.buttonColorsGradient3;
    } else if (themeColor == 2) {
      headerColor = MyColors.customGreen;
      selectionColor = MyColors.buttonColorsGradient5;
    } else if (themeColor == 3) {
      headerColor = MyColors.customOrange;
      selectionColor = MyColors.buttonColorsGradient7;
    }

    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: Stack(
      children: [
        ClipPath(
          clipper: HomePageSemicircle(),
          child: Opacity(
            opacity: 0.3,
            child: Container(
              color: headerColor,
            ),
          ),
        ),
        Column(
          children: [
            TableCalendar<Event>(
              firstDay: kFirstDay,
              lastDay: kLastDay,
              focusedDay: _focusedDay,
              selectedDayPredicate: (day) => isSameDay(_selectedDay, day),
              rangeStartDay: _rangeStart,
              rangeEndDay: _rangeEnd,
              calendarFormat: _calendarFormat,
              rangeSelectionMode: _rangeSelectionMode,
              eventLoader: _getEventsForDay,
              startingDayOfWeek: StartingDayOfWeek.monday,
              rowHeight: MediaQuery.of(context).size.height * 0.068,
              daysOfWeekHeight: MediaQuery.of(context).size.height * 0.068,
              daysOfWeekStyle: DaysOfWeekStyle(
                dowTextFormatter: (date, locale) =>
                    DateFormat.E(locale).format(date)[0],
                weekdayStyle: const TextStyle(fontSize: 17),
                weekendStyle: const TextStyle(fontSize: 17),
                decoration: const BoxDecoration(
                    border: Border(
                  bottom: BorderSide(color: Color.fromRGBO(0, 0, 0, 0.14)),
                )),
              ),
              headerStyle: HeaderStyle(
                headerMargin: EdgeInsets.only(
                    top: MediaQuery.of(context).size.height * 0.08,
                    bottom: MediaQuery.of(context).size.height * 0.09),
                leftChevronIcon: const Icon(
                    key: Key("leftChevron"), Icons.arrow_back_ios_rounded),
                rightChevronIcon: const Icon(
                    key: Key("rightChevron"), Icons.arrow_forward_ios_rounded),
                titleTextStyle: const TextStyle(
                    fontFamily: 'Montserrat',
                    fontSize: 24,
                    color: Color(0xff262626),
                    fontWeight: FontWeight.w500),
                formatButtonDecoration: BoxDecoration(
                  color: selectionColor,
                  borderRadius: BorderRadius.circular(20.0),
                ),
                formatButtonTextStyle: const TextStyle(color: Colors.white),
                formatButtonShowsNext: false,
              ),
              calendarStyle: CalendarStyle(
                outsideDaysVisible: false,
                markerSizeScale: 0.11,
                markersAnchor: 1.2,
                markerMargin: const EdgeInsets.only(right: 1, left: 1),
                markerDecoration: BoxDecoration(
                    color: const Color.fromARGB(255, 255, 255, 255),
                    shape: BoxShape.circle,
                    border: Border.all(width: 2.0, color: Colors.blueGrey)),
                rowDecoration: const BoxDecoration(
                  border: Border(
                    bottom: BorderSide(color: Color.fromARGB(34, 63, 10, 66)),
                  ),
                ),
                defaultTextStyle:
                    const TextStyle(fontSize: 17, fontWeight: FontWeight.w500),
                weekendTextStyle:
                    const TextStyle(fontSize: 17, fontWeight: FontWeight.w500),
              ),
              calendarBuilders: CalendarBuilders(
                singleMarkerBuilder: (context, date, event) {
                  final Color categoryColor = event.categoryColor;
                  return Container(
                    key: Key("markerKey${event.startDate}"),
                    decoration: BoxDecoration(
                        color: const Color.fromARGB(255, 255, 255, 255),
                        shape: BoxShape.circle,
                        border: Border.all(width: 2.0, color: categoryColor)),
                    width: 7.0,
                    height: 7.0,
                    margin: const EdgeInsets.symmetric(horizontal: 1.5),
                  );
                },
                selectedBuilder: (context, date, events) => Container(
                    margin: const EdgeInsets.symmetric(
                        vertical: 15.0, horizontal: 15),
                    alignment: Alignment.center,
                    decoration: BoxDecoration(
                        color: selectionColor,
                        borderRadius: BorderRadius.circular(10.0)),
                    child: Text(
                      date.day.toString(),
                      style: const TextStyle(color: Colors.white),
                    )),
                todayBuilder: (context, date, events) => Container(
                    margin: const EdgeInsets.symmetric(
                        vertical: 15.0, horizontal: 15),
                    alignment: Alignment.center,
                    decoration: BoxDecoration(
                        border: Border.all(width: 2.0, color: selectionColor),
                        borderRadius: BorderRadius.circular(10.0)),
                    child: Text(
                      date.day.toString(),
                      style: const TextStyle(color: Colors.black),
                    )),
              ),
              onDaySelected: _onDaySelected,
              onRangeSelected: _onRangeSelected,
              onFormatChanged: (format) {
                if (_calendarFormat != format) {
                  setState(() {
                    _calendarFormat = format;
                  });
                }
              },
              onPageChanged: (focusedDay) {
                _focusedDay = focusedDay;
              },
            ),
            Expanded(
              child: ValueListenableBuilder<List<Event>>(
                key: const Key("eventListKey"),
                valueListenable: _selectedEvents,
                builder: (context, value, _) {
                  return ListView.builder(
                    itemCount: value.length,
                    itemBuilder: (context, index) {
                      return Container(
                          key: const Key("eventListContainerKey"),
                          margin: const EdgeInsets.symmetric(
                            horizontal: 12.0,
                            vertical: 4.0,
                          ),
                          decoration: const BoxDecoration(
                              // border: Border.all(),
                              // borderRadius: BorderRadius.circular(12.0),
                              ),
                          child: Column(children: [
                            index == 0
                                ? Image.asset(
                                    'assets/images/icons/drag_icon.png')
                                : const SizedBox(),
                            ListTile(
                              leading: Container(
                                decoration: BoxDecoration(
                                    color: const Color.fromARGB(
                                        255, 255, 255, 255),
                                    shape: BoxShape.circle,
                                    border: Border.all(
                                        width: 3.0,
                                        color: value[index].categoryColor)),
                                width: 10.0,
                                height: 10.0,
                                margin: const EdgeInsets.only(top: 5),
                              ),
                              title: Text(value[index].getStringTime(),
                                  style: const TextStyle(
                                      fontSize: 15,
                                      fontWeight: FontWeight.w900,
                                      color: MyColors.calendarTimeColor)),
                              subtitle: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                mainAxisAlignment: MainAxisAlignment.start,
                                children: <Widget>[
                                  Text(
                                      'Break time allowed:'
                                      ' ${value[index].breakTime.toString()}'
                                      ' hour',
                                      style: const TextStyle(
                                          fontSize: 15,
                                          fontWeight: FontWeight.w400,
                                          fontFamily: 'Montserrat',
                                          color:
                                              MyColors.calendarPositionColor))
                                ],
                              ),
                            )
                          ]));
                    },
                  );
                },
              ),
            ),
          ],
        ),
      ],
    ));
  }
}
