import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const VerificationPage = () => {
  const [token, setToken] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    setToken(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch('https://localhost:7222/VerifyUser', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(token),
      });

      if (response.ok) {
        const result = await response.json();
        if (result.statusCode===200) {
          setMessage('Registered successfully!');
          // Optionally navigate to another page
          navigate('/');
        } else {
          setMessage('Verification failed. Please check your token.');
        }
      } else {
        setMessage('Error occurred while verifying. Please try again.');
      }
    } catch (error) {
      console.error('Error occurred:', error);
      setMessage('Error occurred. Please try again.');
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-blue-100 to-white">
      <div className="bg-white shadow-lg rounded-lg p-8 max-w-md w-full">
        <h2 className="text-2xl font-bold text-center mb-6 text-blue-600">Verify Your Account</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="token" className="block text-gray-700">Verification Token</label>
            <input
              type="text"
              id="token"
              value={token}
              onChange={handleChange}
              className="w-full mt-1 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring focus:ring-blue-200"
              required
            />
          </div>
          <div>
            <button
              type="submit"
              className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-lg"
            >
              Verify
            </button>
          </div>
        </form>
        {message && <p className="text-center text-red-500 mt-4">{message}</p>}
      </div>
    </div>
  );
};

export default VerificationPage;
