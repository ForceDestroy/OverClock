import 'package:mobile/utils/payslip_utils.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/payslip_page.dart';
import 'package:mobile/payslip_detail_page.dart';
import 'package:flutter/material.dart';

void main() {
  testWidgets(
      'when payslip page is called with a non empty payslip list, '
      'returns the appropriate page', (WidgetTester tester) async {
    final Payslip payslip = Payslip(
        "name",
        "address",
        1000,
        DateTime.parse("2022-12-04T00:00:00"),
        DateTime.parse("2022-12-10T00:00:00"),
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100);
    final payslipPage = PayslipPage(
      payslips: [payslip],
      theme: 0,
    );
    expect(payslipPage, isNotNull);
    expect(payslipPage, isInstanceOf<PayslipPage>());
    await tester.pumpWidget(MaterialApp(
      home: payslipPage,
    ));
    expect(find.byType(ListView), findsOneWidget);
    expect(find.text('Payslips'), findsOneWidget);
    expect(find.text('No payslips available'), findsNothing);
    expect(find.text('2022-12-04 to 2022-12-10'), findsOneWidget);
  });
  testWidgets(
      'when payslip page is called with a non empty payslip list, '
      'returns the appropriate page with pink theme',
      (WidgetTester tester) async {
    final Payslip payslip = Payslip(
        "name",
        "address",
        1000,
        DateTime.parse("2022-12-04T00:00:00"),
        DateTime.parse("2022-12-10T00:00:00"),
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100);
    final payslipPage = PayslipPage(
      payslips: [payslip],
      theme: 1,
    );
    expect(payslipPage, isNotNull);
    expect(payslipPage, isInstanceOf<PayslipPage>());
    await tester.pumpWidget(MaterialApp(
      home: payslipPage,
    ));
    expect(find.byType(ListView), findsOneWidget);
    expect(find.text('Payslips'), findsOneWidget);
    expect(find.text('No payslips available'), findsNothing);
    expect(find.text('2022-12-04 to 2022-12-10'), findsOneWidget);
  });
  testWidgets(
      'when payslip page is called with a non empty payslip list, '
      'returns the appropriate page with green theme',
      (WidgetTester tester) async {
    final Payslip payslip = Payslip(
        "name",
        "address",
        1000,
        DateTime.parse("2022-12-04T00:00:00"),
        DateTime.parse("2022-12-10T00:00:00"),
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100);
    final payslipPage = PayslipPage(
      payslips: [payslip],
      theme: 2,
    );
    expect(payslipPage, isNotNull);
    expect(payslipPage, isInstanceOf<PayslipPage>());
    await tester.pumpWidget(MaterialApp(
      home: payslipPage,
    ));
    expect(find.byType(ListView), findsOneWidget);
    expect(find.text('Payslips'), findsOneWidget);
    expect(find.text('No payslips available'), findsNothing);
    expect(find.text('2022-12-04 to 2022-12-10'), findsOneWidget);
  });
  testWidgets(
      'when payslip page is called with a non empty payslip list, '
      'returns the appropriate page with orange theme',
      (WidgetTester tester) async {
    final Payslip payslip = Payslip(
        "name",
        "address",
        1000,
        DateTime.parse("2022-12-04T00:00:00"),
        DateTime.parse("2022-12-10T00:00:00"),
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100);
    final payslipPage = PayslipPage(
      payslips: [payslip],
      theme: 3,
    );
    expect(payslipPage, isNotNull);
    expect(payslipPage, isInstanceOf<PayslipPage>());
    await tester.pumpWidget(MaterialApp(
      home: payslipPage,
    ));
    expect(find.byType(ListView), findsOneWidget);
    expect(find.text('Payslips'), findsOneWidget);
    expect(find.text('No payslips available'), findsNothing);
    expect(find.text('2022-12-04 to 2022-12-10'), findsOneWidget);
  });
  testWidgets(
      'when payslip page is called with an empty payslip list, '
      'returns the appropriate page', (WidgetTester tester) async {
    const payslipPage = PayslipPage(
      payslips: [],
      theme: 0,
    );
    expect(payslipPage, isNotNull);
    expect(payslipPage, isInstanceOf<PayslipPage>());
    await tester.pumpWidget(const MaterialApp(
      home: payslipPage,
    ));
    expect(find.byType(ListView), findsNothing);
    expect(find.text('Payslips'), findsOneWidget);
    expect(find.text('No payslips available'), findsOneWidget);
  });
//test payslip_detail_page.dart here
  testWidgets(
      'when payslip detail page is called with a payslip , '
      'returns the appropriate page', (WidgetTester tester) async {
    final Payslip payslip = Payslip(
        "name",
        "address",
        1000,
        DateTime.parse("2022-12-04T00:00:00"),
        DateTime.parse("2022-12-10T00:00:00"),
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100);
    final payslipDetailPage = DetailPage(
      payslip: payslip,
    );
    expect(payslipDetailPage, isNotNull);
    expect(payslipDetailPage, isInstanceOf<DetailPage>());
    await tester.pumpWidget(MaterialApp(
      home: payslipDetailPage,
    ));
    expect(find.text('2022-12-04 to 2022-12-10'), findsOneWidget);
    expect(find.byType(ListView), findsOneWidget);
    expect(find.byType(Card), findsNWidgets(3));
    expect(find.text(' Name'), findsOneWidget);
    expect(find.text(' Address'), findsOneWidget);
    expect(find.text(' Start Date'), findsOneWidget);
    expect(find.text(' End Date'), findsOneWidget);
    expect(find.text(' Salary'), findsOneWidget);
    expect(find.text(' Hours worked'), findsOneWidget);

    expect(find.byType(FloatingActionButton), findsOneWidget);
  });
}
