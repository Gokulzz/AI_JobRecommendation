import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from './ui/button.js';
import { Card, CardContent } from './ui/card.js';

const CreateProfile = () => {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    phoneNumber: '',
    experienceLevel: '',
    currentJobTitle: '',
    currentCompany: '',
    Address: '',
    PreferredJobTitle: '',
    PreferredLocation: ''
  });

  const [resumeFile, setResumeFile] = useState(null); // State for the resume file
  const [successMessage, setSuccessMessage] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value, 
    });
  };

  const handleFileChange = (e) => {
    setResumeFile(e.target.files[0]); // Set the selected file
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const token = localStorage.getItem('token');

    // Create an object to send as JSON for profile
    const profileDataToSend = {
      ...formData, // Spread the formData into a new object
    };

    try {
      // Submit profile data as JSON
      const profileResponse = await fetch('https://localhost:7222/AddUserProfile', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json', // Set the content type to JSON as my server expects the input field in JSON
          'Authorization': `Bearer ${token}`,//send the jwt token with request 
        },
        body: JSON.stringify(profileDataToSend), // Convert the object to a JSON string
      });

      if (profileResponse.ok) {
        const profileData = await profileResponse.json();
        console.log('Profile created:', profileData);

        // Now upload the resume
        const resumeFormData = new FormData();
        resumeFormData.append('fileData', resumeFile); // Append the file to FormData

        const resumeResponse = await fetch('https://localhost:7222/AddResume', {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${token}`,
          },
          body: resumeFormData, // Send the FormData with the resume file
        });

        if (resumeResponse.ok) {
          console.log('Resume uploaded successfully!');
          setSuccessMessage(true);
          setTimeout(() => {
            navigate('/dashboard');
          }, 2000);
        } else {
          // Handle resume upload error
          console.error('Resume upload error:', await resumeResponse.text());
          setErrorMessage('Failed to upload your resume. Please try again.'); 
        }
      } else {
        // Handle profile creation error
        console.error('Profile creation error:', await profileResponse.text());
        setErrorMessage('Failed to create your profile. Please check your inputs and try again.'); 
      }
    } catch (error) {
      console.error('Error:', error);
      setErrorMessage('An unexpected error occurred. Please try again later.'); 
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-blue-100 to-white py-8">
      <div className="container mx-auto px-4">
        <header className="text-center mb-12">
          <h1 className="text-4xl font-bold text-blue-600">Create Your Profile</h1>
          <p className="mt-2 text-lg text-gray-600">Fill in your details to enhance your job recommendations</p>
        </header>

        {/* Show success message if profile was created */}
        {successMessage ? (
          <Card className="mb-8 shadow-lg">
            <CardContent className="p-6">
              <div className="text-center">
                <h2 className="text-3xl font-bold text-green-600 mb-4">Profile Created Successfully!</h2>
                <p className="text-lg text-gray-700">You will be redirected to your dashboard shortly...</p>
              </div>
            </CardContent>
          </Card>
        ) : (
          <Card className="mb-8 shadow-lg">
            <CardContent className="p-6">
              <form onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {/* Form Fields */}
                  {['firstName', 'lastName', 'phoneNumber', 'experienceLevel', 'currentJobTitle', 'currentCompany', 'Address', 'PreferredJobTitle', 'PreferredLocation'].map((field) => (
                    <div key={field}>
                      <label className="block text-gray-700 mb-1" htmlFor={field}>
                        {field.charAt(0).toUpperCase() + field.slice(1).replace(/([A-Z])/g, ' $1')}
                      </label>
                      <input
                        type="text"
                        id={field}
                        name={field}
                        value={formData[field]}
                        onChange={handleChange}
                        className="w-full border border-gray-300 rounded-lg p-2"
                        required={field !== 'currentJobTitle' && field !== 'currentCompany'} 
                      />
                    </div>
                  ))}
                  {/* Resume File Upload */}
                  <div>
                    <label className="block text-gray-700 mb-1" htmlFor="resumeFile">
                      Upload Resume
                    </label>
                    <input
                      type="file"
                      id="resumeFile"
                      onChange={handleFileChange}
                      className="w-full border border-gray-300 rounded-lg p-2"
                      required 
                    />
                  </div>
                </div>
                {errorMessage && <div className="text-red-600 mt-2">{errorMessage}</div>} {/* Display error message */}
                <Button type="submit" className="bg-blue-600 hover:bg-blue-700 text-white py-2 px-6 rounded-lg mt-6">
                  Submit Profile
                </Button>
              </form>
            </CardContent>
          </Card>
        )}
      </div>
    </div>
  );
};

export default CreateProfile;
