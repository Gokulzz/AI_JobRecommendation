# 🚀 AI POWERED JOB RECOMMENDATION SYSTEM 

## 🌟 OVERVIEW

Finding the right job can be overwhelming, with endless searches across multiple job platforms like LinkedIn, Indeed, and Glassdoor. This AI-powered Job Recommendation System simplifies the process by scraping job listings, analyzing your profile, and presenting the most relevant opportunities—all in one place.

This system integrates a .NET backend with a Python-based AI model to deliver highly relevant job recommendations. The AI model parses resumes, extracts key skills, and matches job descriptions using advanced NLP techniques.

## 🔑 KEY FEATURES

### 🏆 USER PROFILE MANAGEMENT

Create and manage profiles, including job preferences (title, location, and skills) for better recommendations.

Store parsed skills from uploaded resumes for enhanced job matching.

## 📄 RESUME PARSING

Upload your resume, and let the AI extract key skills and experience.

Automatically integrates extracted information into the user profile.

## 🎯 TAILORED JOB RECOMMENDATIONS

Scrapes job listings from Indeed and other platforms using Selenium & Beautiful Soup.

Uses TF-IDF vectorization & cosine similarity to analyze job descriptions and match them with user profiles.

Assigns a relevance score and recommends jobs above a predefined threshold.

## ⏳ REAL-TIME JOB SCRAPING & UPDATES

Continuously scrapes and updates job postings to ensure fresh recommendations.

Stores job listings efficiently in SQL Server.

## 🏗 ASYNCHRONOUS MODEL TRAINING

Uses Celery & Redis to enable background training without performance impact.

Applies linear regression to improve job relevance scoring dynamically.

## 📦 FULLY CONTAINERIZED DEPLOYMENT

Dockerized services for seamless scalability and deployment.

Separate containers for Flask API, Job Scraper, Redis, and Relevance Calculator.

## 🛠 TECH STACK

🔹 BACKEND

.NET Core (C#) for main application logic.

Flask API (Python) for AI-based recommendation engine.

SQL Server for structured job and user data storage.

JWT Authentication for secure user sessions.

## 🔹 AI & MACHINE LEARNING

TF-IDF & Cosine Similarity for job relevance scoring.

Linear Regression for adaptive job matching.

NLP-based Resume Parsing to extract key skills.

🔹 FRONTEND

React.js for a responsive and dynamic UI.

🔹 DEVOPS & CONTAINERIZATION

Docker for containerized microservices.

Redis & Celery for asynchronous background tasks.

Selenium for automated job scraping.

## ⚡ GETTING STARTED

🔹 PREREQUISITES

Ensure you have the following installed:

.NET SDK

Python 3.x

SQL Server

Docker

Redis

🔹 INSTALLATION

# Clone the repository
git clone [https://github.com/your-username/job-recommendation-system.git](https://github.com/Gokulzz/AI_JobRecommendation.git)
cd job-recommendation-system

# Start backend services
cd backend
(dotnet run OR docker-compose up)

# Start the Flask AI API
cd ../flask_api
pip install -r requirements.txt
python app.py

# Start frontend
cd ../frontend
npm install
npm start

## 🎯 HOW IT WORKS

1️⃣ User Registration & Resume Upload: Users sign up and upload resumes.
2️⃣ Resume Parsing: AI extracts skills and job preferences from the resume.
3️⃣ Job Scraping: Selenium fetches job listings from Indeed & other sources.
4️⃣ Relevance Scoring: AI model scores jobs using cosine similarity.
5️⃣ Job Recommendations: Users get personalized job matches based on their profile.

## 📜 DESIGN PATTERNS USED

Repository Pattern for decoupled data access.

Unit of Work Pattern for efficient database transactions.

Dependency Injection for scalable and testable code.
