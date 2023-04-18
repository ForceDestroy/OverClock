import 'dart:convert';

class Payslip {
  final String name;
  final String address;
  final double salary;
  final DateTime startDate;
  final DateTime endDate;
  final double netAmount;
  final double grossAmount;
  final double hoursWorked;
  final double amountAccumulated;
  final double ei;
  final double fit;
  final double qcpit;
  final double qpip;
  final double qpp;
  final double eiAccumulated;
  final double fitAccumulated;
  final double qcpitAccumulated;
  final double qpipaAccumulated;
  final double qppAccumulated;
  final double hoursAccumulated;

  Payslip(
      this.name,
      this.address,
      this.salary,
      this.startDate,
      this.endDate,
      this.hoursWorked,
      this.hoursAccumulated,
      this.grossAmount,
      this.amountAccumulated,
      this.ei,
      this.fit,
      this.qcpit,
      this.qpip,
      this.qpp,
      this.eiAccumulated,
      this.fitAccumulated,
      this.qcpitAccumulated,
      this.qpipaAccumulated,
      this.qppAccumulated,
      this.netAmount);
}

List<Payslip> createPayslips(String payslipData) {
  if (payslipData.isEmpty ||
      payslipData == "getPayslips failed: response.statusCode != 200" ||
      payslipData == "getPayslips failed: Exception caught") {
    return <Payslip>[];
  }
  // print(payslipData);
  final jsonPayslipData = jsonDecode(payslipData);
  final payslipList = <Payslip>[];
  for (final payslip in jsonPayslipData) {
    final String name = payslip['Name'];
    final String address = payslip['Address'];
    final double salary = payslip['Salary'];
    final DateTime startDate = DateTime.parse(payslip['StartDate']);
    final DateTime endDate = DateTime.parse(payslip['EndDate']);
    final double netAmount = payslip['NetAmount'];
    final double grossAmount = payslip['GrossAmount'];
    final double hoursWorked = payslip['HoursWorked'].toDouble();
    final double amountAccumulated = payslip['AmountAccumulated'];
    final double ei = payslip['EI'];
    final double fit = payslip['FIT'];
    final double qcpit = payslip['QCPIT'];
    final double qpip = payslip['QPIP'];
    final double qpp = payslip['QPP'];
    final double eiAccumulated = payslip['EIAccumulated'];
    final double fitAccumulated = payslip['FITAccumulated'];
    final double qcpitAccumulated = payslip['QCPITAccumulated'];
    final double qpipaAccumulated = payslip['QPIPAccumulated'];
    final double qppAccumulated = payslip['QPPAccumulated'];
    final double hoursAccumulated = payslip['HoursAccumulated'].toDouble();
    payslipList.add(Payslip(
        name,
        address,
        salary,
        startDate,
        endDate,
        hoursWorked,
        hoursAccumulated,
        grossAmount,
        amountAccumulated,
        ei,
        fit,
        qcpit,
        qpip,
        qpp,
        eiAccumulated,
        fitAccumulated,
        qcpitAccumulated,
        qpipaAccumulated,
        qppAccumulated,
        netAmount));
  }
  return payslipList;
}
