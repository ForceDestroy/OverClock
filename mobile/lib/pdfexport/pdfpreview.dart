import 'package:flutter/material.dart';
import 'package:mobile/utils/payslip_utils.dart';
import 'package:mobile/printing_packages/printing.dart';
import 'pdf/pdfexport.dart';

class PdfPreviewPage extends StatelessWidget {
  final Payslip payslip;
  const PdfPreviewPage({Key? key, required this.payslip}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('PDF Preview'),
      ),
      body: PdfPreview(
        build: (context) => makePdf(payslip),
      ),
    );
  }
}
