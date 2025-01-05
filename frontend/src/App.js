import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LandingPage from './components/LandingPage';
import RegistrationPage from './components/RegistrationPage';
import VerificationPage from './components/VerificationPage'; 
import LoginPage  from './components/LoginPage';  
import DashBoardPage from './components/DashboardPage';
import ProfilePage  from './components/ProfilePage';
import VerifyEmailPage from './components/forgot-password';
import PasswordResetPage from './components/PasswordResetPage';

const App = () => {
  return (
    <Router>
      <Routes>
        {/* Landing Page Route */}
        <Route path="/" element={<LandingPage />} />

        {/* Registration Page Route */}
        <Route path="/register" element={<RegistrationPage />} />

        {/* Verification Page Route */}
        <Route path="/verify" element={<VerificationPage />} /> 
        <Route path="/login" element={<LoginPage />} />
        <Route path="/dashboard" element={<DashBoardPage/>} />
        <Route path="/create-profile" element={<ProfilePage/>} />
        <Route path="/forgot-password" element= {<VerifyEmailPage/>}/>
        <Route path="/reset-password" element= {<PasswordResetPage/>}/>
        
      </Routes>
    </Router>
  );
};

export default App;
