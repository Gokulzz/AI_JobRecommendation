import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Button from './ui/button.js';
import Input from './ui/input.js';
import { Card, CardContent } from './ui/card.js';
import { Mail, Lock } from 'lucide-react';

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setErrorMessage('');

    if (!email || !password) {
      setErrorMessage('Please enter both email and password');
      return;
    }

    try {
      const response = await fetch('https://localhost:7222/UserLogin', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json();

      if (response.ok) {
        const token = data.result;
        localStorage.setItem('token', token);

        // Fetch user title and location
        const preferencesResponse = await fetch('https://localhost:7222/GetTitleAndLocation', {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
          },
        });

        const preferencesData = await preferencesResponse.json();

        if (preferencesResponse.ok) {
          const { preferredJobTitle, preferredLocation } = preferencesData.result;

          // Call ScrapJob API with job preferences
          const scrapResponse = await fetch('https://localhost:7222/ScrapJob', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
              job_Title: preferredJobTitle,
              location: preferredLocation,
            }),
          });

          const scrapData = await scrapResponse.json();

          if (scrapResponse.ok) {
            // Save the scraped jobs to local storage
            localStorage.setItem('scrapedJobs', JSON.stringify(scrapData));
            console.log('Scraped Jobs:', scrapData);
            navigate('/dashboard');
          } else {
            setErrorMessage(scrapData.message || 'Error scraping jobs');
          }
        } else {
          setErrorMessage(preferencesData.message || 'Error retrieving preferences');
        }
      } else {
        setErrorMessage(data.message || 'Email or password is incorrect');
      }
    } catch (error) {
      console.error('Error during login:', error);
      setErrorMessage('An error occurred. Please try again.');
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-blue-100 to-white">
      <Card className="w-full max-w-md shadow-lg">
        <CardContent className="p-6">
          <h2 className="text-3xl font-bold text-center text-blue-600 mb-6">Login to Your Account</h2>

          {errorMessage && (
            <div className="mb-4 text-red-500 text-center font-semibold">
              {errorMessage}
            </div>
          )}

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

          <div className="mb-6">
            <label className="block text-gray-600 font-semibold mb-2" htmlFor="password">Password</label>
            <div className="flex items-center border border-gray-300 rounded-lg overflow-hidden">
              <Lock className="text-gray-400 w-10 h-10 p-2" />
              <Input
                id="password"
                type="password"
                placeholder="Enter your password"
                className="flex-1 p-3 focus:outline-none"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>

          <Button
            className="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded-lg shadow-lg mb-4"
            onClick={handleLogin}
          >
            Login
          </Button>

          <p className="text-center text-gray-600">
            Don't have an account?{' '}
            <Link to="/register" className="text-blue-600 hover:text-blue-700 font-semibold">
              Register
            </Link>
          </p>
        </CardContent>
      </Card>
    </div>
  );
};

export default LoginPage;
