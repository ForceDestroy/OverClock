import 'package:mobile/utils/payslip_utils.dart';
import 'package:flutter_test/flutter_test.dart';

void main() {
  test(
      'createPayslips() test with empty string,'
      ' initialize the empty payslip list value and returns', () {
    const String payslipData = '';
    final payslipList = createPayslips(payslipData);
    expect(payslipList, []);
  });

  test(
      'createPayslips() test with valid payslip string, '
      'fill the payslip list values', () {
    const String payslipData =
        '[{"Name":"Dushdush truong","Address":"0 Beach Avenue", '
        '"Salary":9.0000,"StartDate":"2022-12-04T00:00:00","EndDate":'
        '"2022-12-10T00:00:00","HoursWorked":8.00,"HoursAccumulated":'
        '8.00,"GrossAmount":72.0000,"AmountAccumulated":72.0000,"EI":0.8640000,'
        '"FIT":4.80960000,"QCPIT":7.27920000,"QPIP":0.35280000,"QPP":4.11840000'
        ',"EIAccumulated":0.8640,"FITAccumulated":4.8096,"QCPITAccumulated"'
        ':7.2792,"QPIPAccumulated":0.3528,"QPPAccumulated":4.1184,"NetAmount"'
        ':54.57600000}]';
    final payslipList = createPayslips(payslipData);
    final Payslip expectedPayslip = Payslip(
      "Dushdush truong",
      "0 Beach Avenue",
      9.0000,
      DateTime.parse("2022-12-04T00:00:00"),
      DateTime.parse("2022-12-10T00:00:00"),
      8.00,
      8.00,
      72.0000,
      72.0000,
      0.8640000,
      4.80960000,
      7.27920000,
      0.35280000,
      4.11840000,
      0.8640,
      4.8096,
      7.2792,
      0.3528,
      4.1184,
      54.57600000,
    );

    expect(payslipList[0].name, expectedPayslip.name);
    expect(payslipList[0].address, expectedPayslip.address);
    expect(payslipList[0].salary, expectedPayslip.salary);
    expect(payslipList[0].startDate, expectedPayslip.startDate);
    expect(payslipList[0].endDate, expectedPayslip.endDate);
    expect(payslipList[0].hoursWorked, expectedPayslip.hoursWorked);
    expect(payslipList[0].hoursAccumulated, expectedPayslip.hoursAccumulated);
    expect(payslipList[0].grossAmount, expectedPayslip.grossAmount);
    expect(payslipList[0].amountAccumulated, expectedPayslip.amountAccumulated);
    expect(payslipList[0].ei, expectedPayslip.ei);
    expect(payslipList[0].fit, expectedPayslip.fit);
    expect(payslipList[0].qcpit, expectedPayslip.qcpit);
    expect(payslipList[0].qpip, expectedPayslip.qpip);
    expect(payslipList[0].qpp, expectedPayslip.qpp);
    expect(payslipList[0].eiAccumulated, expectedPayslip.eiAccumulated);
    expect(payslipList[0].fitAccumulated, expectedPayslip.fitAccumulated);
    expect(payslipList[0].qcpitAccumulated, expectedPayslip.qcpitAccumulated);
    expect(payslipList[0].qpipaAccumulated, expectedPayslip.qpipaAccumulated);
    expect(payslipList[0].qppAccumulated, expectedPayslip.qppAccumulated);
    expect(payslipList[0].netAmount, expectedPayslip.netAmount);
  });
}
