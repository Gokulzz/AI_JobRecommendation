import React from 'react';
import { useNavigate } from 'react-router-dom';
import Button from './ui/button.js';
import { Card, CardContent } from './ui/card.js';
import { Briefcase, Search } from 'lucide-react';

const JobDashboard = () => {
  const navigate = useNavigate();

  const handleJobDetails = (jobId) => {
    // Navigate to the Job Details page for the selected job
    navigate(`/job/${jobId}`);
  };

  const handleCreateProfile = () => {
    // Navigate to the Create Profile page
    navigate('/create-profile');
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-blue-100 to-white py-8">
      <div className="container mx-auto px-4">
        {/* Welcome Message */}
        <header className="text-center mb-12">
          <h1 className="text-4xl font-bold text-blue-600">Welcome to Your Job Dashboard</h1>
          <p className="mt-2 text-lg text-gray-600">Explore job recommendations tailored to your profile</p>
        </header>

        {/* Profile Section */}
        <Card className="mb-8 shadow-lg">
          <CardContent className="p-6 text-center">
            <h2 className="text-2xl font-semibold text-gray-700 mb-4">Complete Your Profile</h2>
            <p className="text-gray-600 mb-6">
               Click below to create your profile and enhance your job recommendations!
            </p>
            <Button 
              className="bg-green-600 hover:bg-green-700 text-white py-2 px-6 rounded-lg" 
              onClick={handleCreateProfile}
            >
              Create Profile
            </Button>
          </CardContent>
        </Card>

        {/* Job Search Bar */}
        <div className="flex items-center justify-center mb-8">
          <div className="flex items-center w-full max-w-2xl border border-gray-300 rounded-lg overflow-hidden">
            <Search className="text-gray-400 w-10 h-10 p-2" />
            <input
              type="text"
              placeholder="Search for jobs"
              className="flex-1 p-3 focus:outline-none"
            />
          </div>
        </div>

        {/* Recommended Jobs Section */}
        <section>
          <h2 className="text-3xl font-semibold mb-6 text-gray-900">Recommended for You</h2>
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
            {/* Example Job Card */}
            <Card className="shadow-lg hover:shadow-xl transition duration-300">
              <CardContent className="p-6">
                <div className="flex items-center mb-4">
                  <Briefcase className="w-10 h-10 text-blue-600" />
                  <h3 className="ml-4 text-2xl font-bold text-gray-900">Software Engineer</h3>
                </div>
                <p className="text-gray-600 mb-4">Company: TechCorp</p>
                <p className="text-gray-600 mb-4">Location: Toronto, ON</p>
                <Button 
                  className="bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded-lg" 
                  onClick={() => handleJobDetails(1)}
                >
                  View Details
                </Button>
              </CardContent>
            </Card>

            {/* More Job Cards */}
            <Card className="shadow-lg hover:shadow-xl transition duration-300">
              <CardContent className="p-6">
                <div className="flex items-center mb-4">
                  <Briefcase className="w-10 h-10 text-blue-600" />
                  <h3 className="ml-4 text-2xl font-bold text-gray-900">Data Scientist</h3>
                </div>
                <p className="text-gray-600 mb-4">Company: DataCorp</p>
                <p className="text-gray-600 mb-4">Location: Vancouver, BC</p>
                <Button 
                  className="bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded-lg" 
                  onClick={() => handleJobDetails(2)}
                >
                  View Details
                </Button>
              </CardContent>
            </Card>

            {/* Add more cards as necessary */}
          </div>
        </section>
      </div>
    </div>
  );
};

export default JobDashboard;
