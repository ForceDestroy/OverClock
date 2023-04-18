/* eslint-disable react/prop-types */
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Text, DataTable } from 'grommet';
import jsPDF from 'jspdf';
import 'jspdf-autotable';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';

// const formatAmount = (amount: number) => {
//   return (Math.round(amount * 100) / 100).toFixed(2);
// };

// const generatePDF = (props: any) => {
//   console.log(props.name);
//   const totalTaxes = formatAmount(
//     props.fit + props.ei + props.qcpit + props.qpip + props.qpp
//   );
//   const totalTaxesAccumulated = formatAmount(
//     props.fitAccumulated +
//       props.eiAccumulated +
//       props.qcpitAccumulated +
//       props.qpipAccumulated +
//       props.qppAccumulated
//   );
//   const grossAmount = formatAmount(props.grossAmount);
//   const amountAccumulated = formatAmount(props.amountAccumulated);
//   const netAmount = formatAmount(props.netAmount);
//   const netAccumulated = formatAmount(props.netAccumulated);

//   const ei = formatAmount(props.ei);
//   const eiAccumulated = formatAmount(props.eiAccumulated);
//   const fit = formatAmount(props.fit);
//   const fitAccumulated = formatAmount(props.fitAccumulated);
//   const qcpit = formatAmount(props.qcpit);
//   const qcpitAccumulated = formatAmount(props.qcpitAccumulated);
//   const qpip = formatAmount(props.qpip);
//   const qpipAccumulated = formatAmount(props.qpipAccumulated);
//   const qpp = formatAmount(props.qpp);
//   const qppAccumulated = formatAmount(props.qppAccumulated);

//   const doc = new jsPDF();
//   // width x height
//   doc.text('DIRECT DEPOSIT NOTICE - NON NEGOTIABLE', 45, 10);
//   doc.setCreationDate(new Date());
//   doc.autoTable({
//     head: [['To: ', 'From:']],
//     body: [
//       [`${props.name}`, 'Mode Durgaa'],
//       [`${props.address}`, '4787 Boul. Des Sources, Pierrefonds-Roxboro'],
//       [`QC, Canada`, 'QC, Canada'],
//     ],
//   });
//   doc.autoTable({
//     head: [['PAY PERIOD', 'ISSUE DATE (YEAR/MONTH/DAY): ', 'AMOUNT']],
//     body: [
//       [
//         `From ${props.startDate.slice(0, 10)} To ${props.endDate.slice(0, 10)}`,
//         `${props.issueDate.slice(0, 10)}`,
//         `${grossAmount}`,
//       ],
//     ],
//   });
//   doc.autoTable({
//     head: [
//       [
//         'DESCRIPTION OF EARNINGS',
//         'HOURS/UNITS',
//         'RATE',
//         'AMOUNT',
//         'YTD AMOUNT',
//       ],
//     ],
//     body: [
//       [
//         'REGULAR',
//         `${props.hoursWorked}`,
//         `${props.salary}`,
//         `${grossAmount}`,
//         `${amountAccumulated}`,
//       ],
//       ['STATUTORY HOLIDAY', ``, ``, ``, ``],
//     ],
//   });
//   doc.autoTable({
//     head: [['TAXABLE WAGES', 'AMOUNT', 'YTD AMOUNT']],
//     body: [
//       ['Employment Insurance - EI', `${grossAmount}`, `${amountAccumulated}`],
//       ['Federal Income Tax (FIT)', `${grossAmount}`, `${amountAccumulated}`],
//       [
//         'Province Income Tax (QC-PIT)',
//         `${grossAmount}`,
//         `${amountAccumulated}`,
//       ],
//       [
//         'Quebec Parental Insurance Plan - QPIP',
//         `${grossAmount}`,
//         `${amountAccumulated}`,
//       ],
//       ['Quebec Pension Plan - QPP', `${grossAmount}`, `${amountAccumulated}`],
//     ],
//   });
//   doc.autoTable({
//     head: [['EMPLOYEE TAXES', 'AMOUNT', 'YTD AMOUNT']],
//     body: [
//       ['Employment Insurance - EI', `${ei}`, `${eiAccumulated}`],
//       ['Federal Income Tax (FIT)', `${fit}`, `${fitAccumulated}`],
//       ['Province Income Tax (QC-PIT)', `${qcpit}`, `${qcpitAccumulated}`],
//       [
//         'Quebec Parental Insurance Plan - QPIP',
//         `${qpip}`,
//         `${qpipAccumulated}`,
//       ],
//       ['Quebec Pension Plan - QPP', `${qpp}`, `${qppAccumulated}`],
//     ],
//   });
//   doc.autoTable({
//     head: [['WITHHOLDING', 'FEDERAL', 'PROVINCIAL']],
//     body: [
//       ['Living Prescribed Zone', '0', '0'],
//       ['Additional Tax Amounts', '0', '0'],
//       ['Annual Deductions/Credits                   ', '0', '0'],
//       ['Labour - Sponsored Fund', '0', '0'],
//     ],
//   });
//   doc.autoTable({
//     head: [
//       [' ', 'GROSS EARNINGS', 'TOTAL TAXES', 'TOTAL DEDUCTIONS', 'NET PAY'],
//     ],
//     body: [
//       ['CURRENT', `${grossAmount}`, `${totalTaxes}`, `0`, `${netAmount}`],
//       [
//         'YTD',
//         `${amountAccumulated}`,
//         `${totalTaxesAccumulated}`,
//         `0`,
//         `${netAccumulated}`,
//       ],
//     ],
//   });
//   doc.output('dataurlnewwindow');
//   doc.save(`payslip-${props.name}-${props.startDate.slice(0, 10)}.pdf`);
// };

function Payslip() {
  const [payslips, setPayslips] = useState<any[]>([]);

  const formatAmount = (amount: number) => {
    return (Math.round(amount * 100) / 100).toFixed(2);
  };
  const generatePDF = (props: any) => {
    console.log(props.name);
    const totalTaxes = formatAmount(
      props.fit + props.ei + props.qcpit + props.qpip + props.qpp
    );
    const totalTaxesAccumulated = formatAmount(
      props.fitAccumulated +
        props.eiAccumulated +
        props.qcpitAccumulated +
        props.qpipAccumulated +
        props.qppAccumulated
    );
    const grossAmount = formatAmount(props.grossAmount);
    const amountAccumulated = formatAmount(props.amountAccumulated);
    const netAmount = formatAmount(props.netAmount);
    const netAccumulated = formatAmount(props.netAccumulated);

    const ei = formatAmount(props.ei);
    const eiAccumulated = formatAmount(props.eiAccumulated);
    const fit = formatAmount(props.fit);
    const fitAccumulated = formatAmount(props.fitAccumulated);
    const qcpit = formatAmount(props.qcpit);
    const qcpitAccumulated = formatAmount(props.qcpitAccumulated);
    const qpip = formatAmount(props.qpip);
    const qpipAccumulated = formatAmount(props.qpipAccumulated);
    const qpp = formatAmount(props.qpp);
    const qppAccumulated = formatAmount(props.qppAccumulated);

    const doc = new jsPDF();
    // width x height
    doc.text('DIRECT DEPOSIT NOTICE - NON NEGOTIABLE', 45, 10);
    doc.setCreationDate(new Date());
    doc.autoTable({
      head: [['To: ', 'From:']],
      body: [
        [`${props.name}`, 'Mode Durgaa'],
        [`${props.address}`, '4787 Boul. Des Sources, Pierrefonds-Roxboro'],
        [`QC, Canada`, 'QC, Canada'],
      ],
    });
    doc.autoTable({
      head: [['PAY PERIOD', 'ISSUE DATE (YEAR/MONTH/DAY): ', 'AMOUNT']],
      body: [
        [
          `From ${props.startDate.slice(0, 10)} To ${props.endDate.slice(
            0,
            10
          )}`,
          `${props.issueDate.slice(0, 10)}`,
          `${grossAmount}`,
        ],
      ],
    });
    doc.autoTable({
      head: [
        [
          'DESCRIPTION OF EARNINGS',
          'HOURS/UNITS',
          'RATE',
          'AMOUNT',
          'YTD AMOUNT',
        ],
      ],
      body: [
        [
          'REGULAR',
          `${props.hoursWorked}`,
          `${props.salary}`,
          `${grossAmount}`,
          `${amountAccumulated}`,
        ],
        ['STATUTORY HOLIDAY', ``, ``, ``, ``],
      ],
    });
    doc.autoTable({
      head: [['TAXABLE WAGES', 'AMOUNT', 'YTD AMOUNT']],
      body: [
        ['Employment Insurance - EI', `${grossAmount}`, `${amountAccumulated}`],
        ['Federal Income Tax (FIT)', `${grossAmount}`, `${amountAccumulated}`],
        [
          'Province Income Tax (QC-PIT)',
          `${grossAmount}`,
          `${amountAccumulated}`,
        ],
        [
          'Quebec Parental Insurance Plan - QPIP',
          `${grossAmount}`,
          `${amountAccumulated}`,
        ],
        ['Quebec Pension Plan - QPP', `${grossAmount}`, `${amountAccumulated}`],
      ],
    });
    doc.autoTable({
      head: [['EMPLOYEE TAXES', 'AMOUNT', 'YTD AMOUNT']],
      body: [
        ['Employment Insurance - EI', `${ei}`, `${eiAccumulated}`],
        ['Federal Income Tax (FIT)', `${fit}`, `${fitAccumulated}`],
        ['Province Income Tax (QC-PIT)', `${qcpit}`, `${qcpitAccumulated}`],
        [
          'Quebec Parental Insurance Plan - QPIP',
          `${qpip}`,
          `${qpipAccumulated}`,
        ],
        ['Quebec Pension Plan - QPP', `${qpp}`, `${qppAccumulated}`],
      ],
    });
    doc.autoTable({
      head: [['WITHHOLDING', 'FEDERAL', 'PROVINCIAL']],
      body: [
        ['Living Prescribed Zone', '0', '0'],
        ['Additional Tax Amounts', '0', '0'],
        ['Annual Deductions/Credits                   ', '0', '0'],
        ['Labour - Sponsored Fund', '0', '0'],
      ],
    });
    doc.autoTable({
      head: [
        [' ', 'GROSS EARNINGS', 'TOTAL TAXES', 'TOTAL DEDUCTIONS', 'NET PAY'],
      ],
      body: [
        ['CURRENT', `${grossAmount}`, `${totalTaxes}`, `0`, `${netAmount}`],
        [
          'YTD',
          `${amountAccumulated}`,
          `${totalTaxesAccumulated}`,
          `0`,
          `${netAccumulated}`,
        ],
      ],
    });
    doc.output('dataurlnewwindow');
    doc.save(`payslip-${props.name}-${props.startDate.slice(0, 10)}.pdf`);
  };

  const payslipsColumns = [
    {
      property: 'name',
      header: 'Issued For',
      render: (data: any) => <Text>{data.name}</Text>,
    },
    {
      property: 'issueDate',
      header: 'Issue Date',
      render: (data: any) => <Text>{data.issueDate.slice(0, 10)}</Text>,
    },
    {
      property: 'startDate',
      header: 'Start Date',
      render: (data: any) => <Text>{data.startDate.slice(0, 10)}</Text>,
    },
    {
      property: 'endDate',
      header: 'End Date',
      render: (data: any) => <Text>{data.endDate.slice(0, 10)}</Text>,
    },
    {
      property: 'hoursWorked',
      header: 'Hours Worked',
      render: (data: any) => <Text>{data.hoursWorked}</Text>,
    },
    {
      property: 'hoursAccumulated',
      header: 'Hours Accumulated',
      render: (data: any) => <Text>{data.hoursAccumulated}</Text>,
    },
    {
      property: 'salary',
      header: 'Salary',
      render: (data: any) => <Text>{formatAmount(data.salary)}</Text>,
    },
    {
      property: 'currentNetPay',
      header: 'Current Net Pay',
      render: (data: any) => <Text>{formatAmount(data.netAmount)}</Text>,
    },
    {
      property: 'YTDNetPay',
      header: 'YTD Net Pay',
      render: (data: any) => <Text>{formatAmount(data.netAccumulated)}</Text>,
    },
  ];

  const getPayslips = async () => {
    const token = AuthTokenHelper.getFromLocalStorage();
    const url = `${NetworkResources.API_URL}/PaySlip/GetPaySlips`;
    const params = JSON.stringify({
      sessionToken: token,
    });
    const { data } = await RestCallHelper.GetRequest(url, params);
    if (data != null) {
      try {
        const dataObject = JSON.parse(data);
        setPayslips(dataObject);
      } catch (error) {
        console.log(error);
      }
    }
  };
  const navigate = useNavigate();
  useEffect(() => {
    getPayslips();
  }, []);
  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    } else {
      getPayslips();
    }
  }, [navigate]);
  return (
    <Box>
      <h1>&nbsp;&nbsp;Payslips</h1>
      <DataTable
        columns={payslipsColumns}
        data={payslips}
        step={10}
        onClickRow={(event) => {
          generatePDF(event.datum);
        }}
        size="small"
        sortable
      />
    </Box>
  );
}

export default Payslip;
