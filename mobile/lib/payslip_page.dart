import 'package:flutter/material.dart';
import '../utils/payslip_utils.dart';
import 'payslip_detail_page.dart';
import 'package:intl/intl.dart';
import '../themes/colors.dart';

class PayslipPage extends StatelessWidget {
  final List<Payslip> payslips;
  final int theme;
  const PayslipPage({Key? key, required this.payslips, required this.theme})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    Color headerColor = MyColors.featurePageMainColor0;
    if (theme == 0) {
      headerColor = MyColors.buttonColorsGradiant1;
    } else if (theme == 1) {
      headerColor = MyColors.buttonColorsGradient4;
    } else if (theme == 2) {
      headerColor = MyColors.buttonColorsGradient6;
    } else if (theme == 3) {
      headerColor = MyColors.buttonColorsGradient8;
    }
    return Scaffold(
      appBar: AppBar(
        title: const Text('Payslips'),
        backgroundColor: headerColor,
        titleTextStyle: const TextStyle(
          color: Colors.white,
          fontWeight: FontWeight.w500,
          fontSize: 20,
          fontFamily: 'Montserrat',
        ),
      ),
//if payslips is null, write a message to the user
      body: payslips.isEmpty
          ? const Center(
              child: Text('No payslips available'),
            )
          : ListView(
              children: [
                ...payslips.map(
                  (e) => ListTile(
                    title: Text(
                        '${DateFormat('yyyy-MM-dd').format(e.startDate)}'
                        ' to ${DateFormat('yyyy-MM-dd').format(e.endDate)}'),
                    subtitle:
                        Text('Hours worked: ${e.hoursWorked.toString()} hours'),
                    trailing:
                        Text('\$${e.amountAccumulated.toStringAsFixed(2)}'),
                    onTap: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(
                          builder: (builder) =>
                              DetailPage(payslip: e, theme: theme),
                        ),
                      );
                    },
                  ),
                )
              ],
            ),
    );
  }
}
