from celery import Celery
import numpy as np
import pickle
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import redis

# Configure Celery
app = Celery('trainModel', broker='redis://host.docker.internal:6379/0')


# Initialize Redis
r = redis.Redis(host='host.docker.internal', port=6379, db=0)



# Function to train the model 
@app.task
def train_model(job_preferences, scraped_jobs_with_skills, relevance_score):
    try:
        # Prepare data (this would normally be more complex depending on your model)
        job_title = scraped_jobs_with_skills[0].get('title', '')
        job_location = scraped_jobs_with_skills[0].get('location', '')
        job_skills = scraped_jobs_with_skills[0].get('job_skills', [])
        user_skills = job_preferences[0].get('skills', [])
        
        job_skills_str = " ".join(job_skills)
        user_skills_str = " ".join(user_skills)

        # Combine all relevant features (title, location, skills)
        job_description = f"{job_title} {job_location} {job_skills_str}"
        user_description = f"{job_preferences[0].get('preferred_job_title', '')} {job_preferences[0].get('preferred_location', '')} {user_skills_str}"

        # Use TF-IDF to vectorize the text data
        vectorizer = TfidfVectorizer()
        vectors = vectorizer.fit_transform([job_description, user_description])

        # Calculate cosine similarity (could be used as a feature for training)
        cosine_sim = cosine_similarity(vectors[0:1], vectors[1:2])
        model_data = {'relevance_score': relevance_score[0], 'cosine_sim': cosine_sim[0][0]}

       
        
        r.set(f"relevance_model_{job_title}", pickle.dumps(model_data))

        print(f"Model trained for job title: {job_title} with relevance score: {relevance_score[0]}")
        return "Model trained successfully"

    except Exception as e:
        print(f"Error during training: {str(e)}")
        return "Error during model training"
