import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Card, CardContent } from './ui/card.js';
import Button from './ui/button.js';
import Input from './ui/input.js';

const ResetPasswordPage = () => {
  const [token, setToken] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();
  const location = useLocation();
  //const email = location.state?.email || ''; 

  const handlePasswordReset = async (e) => {
    e.preventDefault();
    setMessage('');

    if (!token || !password || !confirmPassword) {
      setMessage('Please fill in all fields');
      return;
    }

    if (password !== confirmPassword) {
      setMessage('Passwords do not match');
      return;
    }

    try {
      const response = await fetch('https://localhost:7222/ChangePasswordWithToken', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ token, password, confirmPassword }),
      });

      const data = await response.json();

      if (response.ok) {
        setMessage('Password reset successful. Redirecting to login...');
        setTimeout(() => {
          navigate('/login'); // Redirect to login
        }, 2000);
      } else {
        setMessage(data.message || 'Password reset failed. Try again later.');
      }
    } catch (error) {
      console.error('Error during password reset:', error);
      setMessage('An error occurred. Please try again.');
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-blue-100 to-white">
      <Card className="w-full max-w-md shadow-lg">
        <CardContent className="p-6">
          <h2 className="text-3xl font-bold text-center text-blue-600 mb-6">Reset Password</h2>

          {message && (
            <div className="mb-4 text-center font-semibold">
              <span className={message.includes('error') ? 'text-red-500' : 'text-green-500'}>
                {message}
              </span>
            </div>
          )}

          <form onSubmit={handlePasswordReset}>
            <div className="mb-4">
              <label className="block text-gray-600 font-semibold mb-2" htmlFor="token">Reset Token</label>
              <Input
                id="token"
                type="text"
                placeholder="Enter your reset token"
                className="w-full p-3 border border-gray-300 rounded-lg"
                value={token}
                onChange={(e) => setToken(e.target.value)}
              />
            </div>

            <div className="mb-4">
              <label className="block text-gray-600 font-semibold mb-2" htmlFor="password">New Password</label>
              <Input
                id="password"
                type="password"
                placeholder="Enter your new password"
                className="w-full p-3 border border-gray-300 rounded-lg"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>

            <div className="mb-6">
              <label className="block text-gray-600 font-semibold mb-2" htmlFor="confirmPassword">Confirm Password</label>
              <Input
                id="confirmPassword"
                type="password"
                placeholder="Confirm your new password"
                className="w-full p-3 border border-gray-300 rounded-lg"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
              />
            </div>

            <Button
              type="submit"
              className="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded-lg shadow-lg"
            >
              Reset Password
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
};

export default ResetPasswordPage;
