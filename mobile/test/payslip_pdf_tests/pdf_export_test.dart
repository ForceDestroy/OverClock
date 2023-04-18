import 'package:mobile/pdfexport/pdf/pdfexport.dart';
import 'package:mobile/pdfexport/pdfpreview.dart';
import 'package:mobile/utils/payslip_utils.dart';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/printing_packages/printing.dart';

void main() {
  testWidgets(
      'when makePdf is called with a valid payslip, '
      'returns a non null pdf', (WidgetTester tester) async {
    final Payslip payslip = Payslip(
        "name",
        "address",
        1000,
        DateTime.now(),
        DateTime.now(),
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
    final pdf = await makePdf(payslip);
    expect(pdf, isNotNull);
  });
  testWidgets(
      'when pdfPreview is called with a valid payslip, '
      'returns the appropriate page', (WidgetTester tester) async {
    final Payslip payslip = Payslip(
        "name",
        "address",
        1000,
        DateTime.now(),
        DateTime.now(),
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
    await tester.pumpWidget(MaterialApp(
      home: PdfPreviewPage(payslip: payslip),
    ));
    expect(find.byType(PdfPreviewPage), findsOneWidget);
    expect(find.text('PDF Preview'), findsOneWidget);
    expect(find.byType(PdfPreview), findsOneWidget);
  });
}
