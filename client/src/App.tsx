import { BrowserRouter, Route, Routes, useLocation } from 'react-router-dom';
import { Box, Grommet } from 'grommet';
import './App.css';

import Home from './pages/Home';
import NotFound from './pages/NotFound';
import Login from './pages/Login';
import EmployeeDashboard from './pages/EmployeeDashboard';
import EmployerDashboard from './pages/EmployerDashboard';
import NavigationTopBar from './components/NavigationTopBar';
import ProfilePage from './pages/ProfilePage';
import Timesheets from './pages/Timesheets';
import Schedule from './pages/Schedule';
import TimeOff from './pages/TimeOff';
import EmployerTimesheets from './pages/EmployerTimesheets';
import Payslip from './pages/Payslip';
import EmployerRequests from './pages/EmployerRequests';
import CompanyPolicy from './pages/CompanyPolicy';

export function App() {
  const { pathname } = useLocation();
  const toggleNav = pathname === '/Login' || pathname === '/';
  const availableRoutes = [
    '/employeedashboard',
    '/employeraccount',
    '/employerdashboard',
    '/employer/timesheets',
    '/employer/requests',
    '/application',
    '/applicationreferral',
    '/applicationmanagement',
    '/companydashboard',
    '/employeedashboard/companypolicy',
    '/profilepage',
    '/employeedashboard/timesheets',
    '/employeedashboard/schedule',
    '/employeedashboard/payslip',
    '/employeedashboard/timeoff',
  ];
  const errorRoute = availableRoutes.includes(pathname.toLowerCase());
  return (
    <div className="App">
      <Grommet full>
        {!toggleNav && errorRoute && <NavigationTopBar />}
        <Box direction="row" height={{ min: '90vh' }} background="light-1">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/EmployeeDashboard" element={<EmployeeDashboard />} />
            <Route path="/EmployerDashboard" element={<EmployerDashboard />} />
            <Route path="/Employer/Requests" element={<EmployerRequests />} />
            <Route
              path="/Employer/Timesheets"
              element={<EmployerTimesheets />}
            />
            <Route
              path="/EmployeeDashboard/CompanyPolicy"
              element={<CompanyPolicy />}
            />
            <Route path="/ProfilePage" element={<ProfilePage />} />
            <Route
              path="/EmployeeDashboard/Timesheets"
              element={<Timesheets />}
            />
            <Route path="/EmployeeDashboard/Schedule" element={<Schedule />} />
            <Route path="/EmployeeDashboard/Payslip" element={<Payslip />} />
            <Route path="/EmployeeDashboard/TimeOff" element={<TimeOff />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </Box>
      </Grommet>
    </div>
  );
}

export function WrappedApp() {
  return (
    <BrowserRouter>
      <App />
    </BrowserRouter>
  );
}
