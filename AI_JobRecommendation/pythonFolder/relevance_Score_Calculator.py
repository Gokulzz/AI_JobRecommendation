from flask import Flask, request, jsonify
from celery import Celery
import numpy as np
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
from trainModel import train_model

app = Flask(__name__)

# Configure Celery
app.config['CELERY_BROKER_URL'] = 'redis://host.docker.internal:6379/0' #defines URL of Celery
app.config['CELERY_RESULT_BACKEND'] = 'redis://host.docker.internal:6379/0' #stores the result of task
celery = Celery(app.name, broker=app.config['CELERY_BROKER_URL'])
celery.conf.update(app.config)

# Function to calculate relevance score using cosine similarity
def calculate_relevance_score(job_preferences, scraped_jobs_with_skills, skills):
    preferred_title = job_preferences.get("preferred_job_title", "")
    preferred_location = job_preferences.get("preferred_location", "")
    job_title = scraped_jobs_with_skills.get("title", "")
    job_location = scraped_jobs_with_skills.get("location", "")
    
    # Ensure job_skills and user_skills are treated as lists
    job_skills = scraped_jobs_with_skills.get("job_skills", [])
    user_skills = skills if skills else []

    # Join skills lists into a single string
    job_skills_str = " ".join(job_skills)
    user_skills_str = " ".join(user_skills)

    # Concatenate title, location, job skills, and user skills to form descriptions
    job_description = f"{job_title} {job_location} {job_skills_str}"
    user_description = f"{preferred_title} {preferred_location} {user_skills_str}"

    # Vectorize the text using TF-IDF (Term Frequency-Inverse Document Frequency)
    vectorizer = TfidfVectorizer()
    vectors = vectorizer.fit_transform([job_description, user_description])

    # Calculate cosine similarity between the user and job descriptions
    cosine_sim = cosine_similarity(vectors[0:1], vectors[1:2])

    # The relevance score is the cosine similarity value
    relevance_score = cosine_sim[0][0]

    return relevance_score

# Endpoint to calculate the relevance score
@app.route('/calculate_RelevanceScore', methods=['POST'])
def calculate():
    try:
        data = request.get_json()

        # Extract job preferences, job details, and skills from the incoming JSON payload
        job_preferences = data['job_Preferences']
        scraped_jobs_with_skills = data['scrapedJobs_withSkills']
        skills = data['Skills']

        # Calculate the relevance score
        relevance_score = calculate_relevance_score(job_preferences, scraped_jobs_with_skills, skills)

        # Call the asynchronous training task (send the data for training)
        train_model.delay([job_preferences], [scraped_jobs_with_skills], [relevance_score])

         # Return the relevance score as JSON
        return jsonify(relevance_score)

    except Exception as e:
        return jsonify({"error": str(e)}), 400

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=6000)

