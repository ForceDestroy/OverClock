import 'dart:typed_data';

import '../../utils/payslip_utils.dart';
import 'package:pdf/pdf.dart';
import 'package:pdf/widgets.dart';
import 'package:flutter/services.dart' show rootBundle;
import 'package:intl/intl.dart';

Future<Uint8List> makePdf(Payslip payslip) async {
  final pdf = Document();
  final imageLogo = MemoryImage(
      (await rootBundle.load('assets/images/mode_Durgaa_mock_logo.jpg'))
          .buffer
          .asUint8List());

  pdf.addPage(
    Page(
      build: (context) {
        return Column(
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Column(
                  children: [
                    Text(payslip.name),
                    Text(payslip.address),
                    Text("Time Period: "
                        '${DateFormat('MM/dd/yyyy').format(payslip.startDate)}-'
                        '${DateFormat('MM/dd/yyyy').format(payslip.endDate)}'),
                    Text("Salary: \$${payslip.salary.toStringAsFixed(2)}"),
                    Text('Hours Worked:'
                        '${payslip.hoursWorked.toStringAsFixed(2)}'),
                  ],
                  crossAxisAlignment: CrossAxisAlignment.start,
                ),
                SizedBox(
                  height: 150,
                  width: 150,
                  child: Image(imageLogo),
                )
              ],
            ),
            Container(height: 20),
            Table(
              border: TableBorder.all(color: PdfColors.black),
              children: [
                TableRow(
                  children: [
                    Expanded(
                      child: paddedText("Gross Amount"),
                      flex: 2,
                    ),
                    Expanded(
                      child: paddedText(
                          "\$${payslip.grossAmount.toStringAsFixed(2)}"),
                      flex: 1,
                    )
                  ],
                ),
//EI row
                TableRow(
                  children: [
                    Expanded(
                      child: paddedText("EI"),
                      flex: 2,
                    ),
                    Expanded(
                      child: paddedText("-\$${payslip.ei.toStringAsFixed(2)}"),
                      flex: 1,
                    )
                  ],
                ),
//FIT row
                TableRow(
                  children: [
                    Expanded(
                      child: paddedText("FIT"),
                      flex: 2,
                    ),
                    Expanded(
                      child: paddedText("-\$${payslip.fit.toStringAsFixed(2)}"),
                      flex: 1,
                    )
                  ],
                ),
//QCPIT row
                TableRow(
                  children: [
                    Expanded(
                      child: paddedText("QCPIT"),
                      flex: 2,
                    ),
                    Expanded(
                      child:
                          paddedText("-\$${payslip.qcpit.toStringAsFixed(2)}"),
                      flex: 1,
                    )
                  ],
                ),
//CPP row
                TableRow(
                  children: [
                    Expanded(
                      child: paddedText("QPIP"),
                      flex: 2,
                    ),
                    Expanded(
                      child:
                          paddedText("-\$${payslip.qpip.toStringAsFixed(2)}"),
                      flex: 1,
                    )
                  ],
                ),
                TableRow(
                  children: [
                    Expanded(
                      child: paddedText("QPP"),
                      flex: 2,
                    ),
                    Expanded(
                      child: paddedText("-\$${payslip.qpp.toStringAsFixed(2)}"),
                      flex: 1,
                    )
                  ],
                ),
                TableRow(
                  children: [
                    paddedText('Net Amount', align: TextAlign.right),
                    paddedText('\$${(payslip.netAmount.toStringAsFixed(2))}')
                  ],
                )
              ],
            ),
            Container(height: 50),
            Table(
              border: TableBorder.all(color: PdfColors.black),
              children: [
                TableRow(
                  children: [
                    paddedText('Hours Accumulated'),
                    paddedText(
                      payslip.hoursAccumulated.toStringAsFixed(2),
                    )
                  ],
                ),
                TableRow(
                  children: [
                    paddedText(
                      'Amount Accumulated',
                    ),
                    paddedText(
                      '\$${payslip.amountAccumulated.toStringAsFixed(2)}',
                    )
                  ],
                ),
//ei accumulated row
                TableRow(
                  children: [
                    paddedText(
                      'EI Accumulated',
                    ),
                    paddedText(
                      '\$${payslip.eiAccumulated.toStringAsFixed(2)}',
                    )
                  ],
                ),
//fit accumulated row
                TableRow(
                  children: [
                    paddedText(
                      'FIT Accumulated',
                    ),
                    paddedText(
                      '\$${payslip.fitAccumulated.toStringAsFixed(2)}',
                    )
                  ],
                ),
//qcpit accumulated row
                TableRow(
                  children: [
                    paddedText(
                      'QCPIT Accumulated',
                    ),
                    paddedText(
                      '\$${payslip.qcpitAccumulated.toStringAsFixed(2)}',
                    )
                  ],
                ),
//qpip accumulated row
                TableRow(
                  children: [
                    paddedText(
                      'QPIP Accumulated',
                    ),
                    paddedText(
                      '\$${payslip.qpipaAccumulated.toStringAsFixed(2)}',
                    )
                  ],
                ),
//qpp accumulated row
                TableRow(
                  children: [
                    paddedText(
                      'QPP Accumulated',
                    ),
                    paddedText(
                      '\$${payslip.qppAccumulated.toStringAsFixed(2)}',
                    )
                  ],
                ),
              ],
            ),
          ],
        );
      },
    ),
  );
  return pdf.save();
}

Widget paddedText(
  final String text, {
  final TextAlign align = TextAlign.left,
}) =>
    Padding(
      padding: const EdgeInsets.all(10),
      child: Text(
        text,
        textAlign: align,
      ),
    );
