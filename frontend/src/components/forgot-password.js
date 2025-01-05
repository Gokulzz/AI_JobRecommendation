import React, { useState } from 'react';
import { Card, CardContent } from './ui/card.js';
import Button from './ui/button.js';
import Input from './ui/input.js';
import { Mail } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

const ForgotPasswordPage = () => {
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();

  const handlePasswordReset = async (e) => {
    e.preventDefault();
    setMessage('');

    if (!email) {
      setMessage('Please enter your email address');
      return;
    }

    try {
        const response = await fetch(`https://localhost:7222/PasswordResetToken?email=${encodeURIComponent(email)}`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' }, 
        });
      console.log(email);

      const data = await response.json();

      if (response.ok) {
        setMessage('Password reset token has been sent to your email.');
        setTimeout(() => {
          navigate('/reset-password');
        }, 2000); // Redirect after 2 seconds
      } else {
        setMessage(data.message || 'Failed to send reset link. Try again later.');
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
          <h2 className="text-3xl font-bold text-center text-blue-600 mb-6">Forgot Password</h2>

          {message && (
            <div className="mb-4 text-center font-semibold">
              <span className={message.includes('error') ? 'text-red-500' : 'text-green-500'}>
                {message}
              </span>
            </div>
          )}

          <form onSubmit={handlePasswordReset}>
            <div className="mb-4">
              <label className="block text-gray-600 font-semibold mb-2" htmlFor="email">Email</label>
              <div className="flex items-center border border-gray-300 rounded-lg overflow-hidden">
                <Mail className="text-gray-400 w-10 h-10 p-2" />
                <Input
                  id="email"
                  type="email"
                  placeholder="Enter your email"
                  className="flex-1 p-3 focus:outline-none"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>
            </div>

            <Button
              type="submit"
              className="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded-lg shadow-lg"
            >
              Send Reset Link
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
};

export default ForgotPasswordPage;
