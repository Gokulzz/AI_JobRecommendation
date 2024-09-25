import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import LandingPage from './components/LandingPage';
import RegistrationPage from './components/RegistrationPage';
import VerificationPage from './components/VerificationPage'; 
import LoginPage  from './components/LoginPage';  
import DashBoardPage from './components/DashboardPage';

const App = () => {
  return (
    <Router>
      <Routes>
        {/* Landing Page Route */}
        <Route path="/" element={<LandingPage />} />

        {/* Registration Page Route */}
        <Route path="/register" element={<RegistrationPage />} />

        {/* Verification Page Route */}
        <Route path="/verify" element={<VerificationPage />} /> {/* Add this line */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/dashboard" element={<DashBoardPage/>} />
        
      </Routes>
    </Router>
  );
};

export default App;
