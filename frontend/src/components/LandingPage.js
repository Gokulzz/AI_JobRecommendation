import React from 'react';
import Button from './ui/button.js';
import { Card, CardContent } from './ui/card.js';
import { Briefcase, Zap, Target } from 'lucide-react';

const LandingPage = () => {
  return (
    <div className="min-h-screen bg-gradient-to-b from-blue-100 to-white">
      {/* Header */}
      <header className="container mx-auto px-4 py-12 text-center">
        <h1 className="text-5xl font-extrabold text-blue-600 drop-shadow-lg">AI Job Matcher</h1>
        <p className="mt-4 text-lg text-gray-600">Your path to finding the perfect job starts here.</p>
      </header>

      {/* Main Content */}
      <main className="container mx-auto px-4">
        {/* Hero Section */}
        <section className="text-center py-20">
          <h2 className="text-5xl font-bold mb-6 text-gray-900">Find Your Dream Job with AI</h2>
          <p className="text-xl text-gray-600 mb-8">
            Let our advanced AI algorithm match you with the perfect job opportunities.
          </p>
          <Button size="lg" className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg shadow-lg">
            Get Started
          </Button>
        </section>

        {/* Feature Cards */}
        <section className="grid md:grid-cols-3 gap-12 py-16">
          <Card className="shadow-lg hover:shadow-xl transition duration-300">
            <CardContent className="flex flex-col items-center p-6">
              <Briefcase className="w-12 h-12 text-blue-600 mb-4" />
              <h3 className="text-2xl font-semibold mb-2 text-gray-900">Personalized Matches</h3>
              <p className="text-center text-gray-600">
                Our AI analyzes your skills and preferences to find the best job fits.
              </p>
            </CardContent>
          </Card>

          <Card className="shadow-lg hover:shadow-xl transition duration-300">
            <CardContent className="flex flex-col items-center p-6">
              <Zap className="w-12 h-12 text-blue-600 mb-4" />
              <h3 className="text-2xl font-semibold mb-2 text-gray-900">Real-time Updates</h3>
              <p className="text-center text-gray-600">
                Get instant notifications for new job openings that match your profile.
              </p>
            </CardContent>
          </Card>

          <Card className="shadow-lg hover:shadow-xl transition duration-300">
            <CardContent className="flex flex-col items-center p-6">
              <Target className="w-12 h-12 text-blue-600 mb-4" />
              <h3 className="text-2xl font-semibold mb-2 text-gray-900">Career Insights</h3>
              <p className="text-center text-gray-600">
                Receive AI-powered suggestions to improve your job search strategy.
              </p>
            </CardContent>
          </Card>
        </section>

        {/* Call to Action Section */}
        <section className="text-center py-16 bg-gray-50">
          <h2 className="text-4xl font-bold mb-6 text-gray-900">Ready to Boost Your Career?</h2>
          <Button size="lg" className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg shadow-lg">
            Sign Up Now
          </Button>
        </section>
      </main>

      {/* Footer */}
      <footer className="bg-gray-100 py-8">
        <div className="container mx-auto px-4 text-center text-gray-600">
          &copy; 2024 AI Job Matcher. All rights reserved.
        </div>
      </footer>
    </div>
  );
};

export default LandingPage;
