import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from './ui/button.js';
import { Card, CardContent } from './ui/card.js';
import { Briefcase } from 'lucide-react';


const JobDashboard = () => {
  const navigate = useNavigate();
  const [jobs, setJobs] = useState([]); // State to hold the jobs
  const [loading, setLoading] = useState(true); // State for loading indicator
  const [errorMessage, setErrorMessage] = useState(''); // State for error messages
  const [showProfilePrompt, setShowProfilePrompt] = useState(false); // State for profile prompt

  const handleCreateProfile = () => {
    navigate('/create-profile'); // Navigate to the Create Profile page
  };

  const fetchJobRecommendations = async (token) => {
    try {
      const getRecommendationResponse = await fetch('https://localhost:7222/GetJobRecommendation', {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });

      if (getRecommendationResponse.ok) {
        const recommendationsData = await getRecommendationResponse.json();
        const jobList = recommendationsData.result.map((item) => item.scrapedJob);
        setJobs(jobList || []);
        localStorage.setItem('jobRecommendations', JSON.stringify(jobList || [])); // Save recommendations to local storage
      } else {
        setErrorMessage('Error retrieving job recommendations.');
      }
    } catch (error) {
      console.error('Error fetching job recommendations:', error);
      setErrorMessage('An error occurred while fetching job recommendations.');
    }
  };

  const fetchAndScrapeJobData = async (token) => {
    try {
      const preferencesResponse = await fetch('https://localhost:7222/GetTitleAndLocation', {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });

      const preferencesData = await preferencesResponse.json();

      if (preferencesResponse.ok) {
        const { preferredJobTitle, preferredLocation } = preferencesData.result;

        if (preferredJobTitle && preferredLocation) {
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

          if (scrapResponse.ok) {
            const postRecommendationResponse = await fetch('https://localhost:7222/AddJobRecommendation', {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
              },
            });

            if (postRecommendationResponse.ok) {
              // Fetch updated recommendations after adding new ones
              await fetchJobRecommendations(token);
            } else {
              const postErrorData = await postRecommendationResponse.json();
              setErrorMessage(postErrorData.message || 'Error posting job recommendations.');
            }
          } else {
            const scrapError = await scrapResponse.json();
            console.error(scrapError.message);
          }
        } else {
          setErrorMessage('No job preferences found. Please complete your profile.');
        }
      } else {
        console.error('Unable to fetch job preferences.');
      }
    } catch (error) {
      console.error('Error scraping job data:', error);
      setErrorMessage('An error occurred while fetching job data.');
    } finally {
      // Always fetch recommendations in case they exist independently of scraping
      await fetchJobRecommendations(token);
      setLoading(false);
    }
  };

  const fetchJobData = async () => {
    const token = localStorage.getItem('token');
    if (!token) return;

    try {
      const userProfile = await fetch('https://localhost:7222/GetUserProfileByUserId', {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });

      if (!userProfile.ok) {
        setShowProfilePrompt(true); // Show the profile prompt if profile is missing
        setLoading(false);
        return;
      }

      await fetchAndScrapeJobData(token); // Fetch and scrape job data
    } catch (error) {
      console.error('Error fetching job data:', error);
      setErrorMessage('An error occurred while fetching job data.');
    }
  };

  useEffect(() => {
    const savedJobs = localStorage.getItem('jobRecommendations');
    if (savedJobs) {
      setJobs(JSON.parse(savedJobs)); // Set jobs from local storage
    }

    fetchJobData(); // Fetch job data on component mount
  }, []);

  return (
    <div className="min-h-screen bg-gradient-to-b from-blue-100 to-white py-8">
      <div className="container mx-auto px-4">
        <header className="text-center mb-12">
          <h1 className="text-4xl font-bold text-blue-600">Welcome to Your Job Dashboard</h1>
          <p className="mt-2 text-lg text-gray-600">Explore job recommendations tailored to your profile</p>
        </header>

        {showProfilePrompt ? (
          <Card className="mb-8 shadow-lg">
            <CardContent className="p-8 text-center">
              <img
                src="https://via.placeholder.com/300x200.png?text=Create+Profile"
                alt="Create Profile"
                className="mb-6 mx-auto rounded-lg"
              />
              <h2 className="text-2xl font-semibold text-gray-700 mb-4">
                Your Profile is Incomplete
              </h2>
              <p className="text-gray-600 mb-6">
                To access personalized job recommendations, create your profile now!
              </p>
              <Button
                className="bg-green-600 hover:bg-green-700 text-white py-2 px-6 rounded-lg"
                onClick={handleCreateProfile}
              >
                Create Profile
              </Button>
            </CardContent>
          </Card>
        ) : loading ? (
          <p className="text-gray-700 text-center">Loading jobs...</p>
        ) : (
          <section>
            <h2 className="text-3xl font-semibold mb-6 text-gray-900">Recommended for You</h2>
            <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
              {errorMessage && (
                <div className="mb-4 text-red-500 text-center font-semibold">{errorMessage}</div>
              )}
              {jobs.length > 0 ? (
                jobs.map((job, index) => (
                  <Card key={index} className="shadow-lg hover:shadow-xl transition duration-300">
                    <CardContent className="p-6">
                      <div className="flex items-center mb-4">
                        <Briefcase className="w-10 h-10 text-blue-600" />
                        <a
                          href={job['Job Link']}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="ml-4 text-2xl font-bold text-gray-900 hover:underline"
                        >
                          {job['Job Title']}
                        </a>
                      </div>
                      <p className="text-gray-600 mb-4">Company: {job.Company || 'Unknown'}</p>
                      {job.Location && <p className="text-gray-600 mb-4">Location: {job.Location}</p>}
                    </CardContent>
                  </Card>
                ))
              ) : (
                <p className="text-gray-700">No job recommendations available.</p>
              )}
            </div>
          </section>
        )}
      </div>
    </div>
  );
};

export default JobDashboard;
