from flask import Flask, request, json
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
from bs4 import BeautifulSoup
import time
import spacy
import nltk
from collections import Counter
from nltk.corpus import stopwords
from spacy.matcher import PhraseMatcher

# Download stopwords if not already downloaded
nltk.download('stopwords')

# Load the pre-trained NLP model (English)
nlp = spacy.load('en_core_web_sm')

# Initialize Flask app
app = Flask(__name__)

# Predefined list of relevant skills
predefined_skills = {
    "sql", "c", "c++", "python", "javascript", "java", "docker", "api", "database",
    "azure", "aws", "machine learning", "nlp", "git", "cloud", "c#",
    "react", "nodejs", "css", "html", "linux", "json", "xml", "rest", "agile", 
    "programming", "data structures and algorithms", "problem-solving", 
    "adaptability", "communication", "kubernetes", "nosql"
}

# Custom stopwords for further filtering
custom_stopwords = {"job", "experience", "skills", "work", "development", "team", "reliable"}

# Combined stopwords list
stop_words = set(stopwords.words('english')) | custom_stopwords

class JobScraper:
    def __init__(self, job_title, location, max_jobs=5):
        # Initialize PhraseMatcher with the predefined skills
        self.phrase_matcher = PhraseMatcher(nlp.vocab)
        patterns = [nlp.make_doc(skill) for skill in predefined_skills]
        self.phrase_matcher.add("SKILLS", patterns)

        self.job_title = job_title
        self.location = location
        self.jobs = []
        self.max_jobs = max_jobs

    def scrape_job_links(self):
        # Open the main driver instance for all operations
        driver = webdriver.Chrome(service=Service(ChromeDriverManager().install()))
        
        url = f'https://ca.indeed.com/jobs?q={self.job_title}&l={self.location}'
        driver.get(url)
        time.sleep(15)  # Initial wait for page to load
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
        time.sleep(10)  # Wait after scrolling
        soup = BeautifulSoup(driver.page_source, 'html.parser')

        job_count = 0
        for job_card in soup.select('div.job_seen_beacon'):
            if job_count >= self.max_jobs:
                break

            job_link_element = job_card.select_one('a.jcs-JobTitle')
            job_title = job_link_element.text.strip() if job_link_element else None
            job_link = job_link_element['href'] if job_link_element and 'href' in job_link_element.attrs else None
            job_link = f"https://ca.indeed.com{job_link}" if job_link else None

            company_location_element = job_card.select_one('div.company_location.css-i375s1.e37uo190')
            company = company_location_element.select_one('span[data-testid="company-name"]').text.strip() if company_location_element else None
            location = company_location_element.select_one('div[data-testid="text-location"]').text.strip() if company_location_element else None

            posted_date_element = job_card.select_one('span.date')
            posted_date = posted_date_element.text.strip() if posted_date_element else None

            # Extract job description and skills using the same driver instance
            job_description = self.extract_job_description(driver, job_link)
            skills = self.extract_skills(job_description)

            # Add job details to the jobs list
            self.jobs.append({
                "Job Title": job_title,
                "Company": company,
                "Location": location,
                "Job Link": job_link,
                "Posted Date": posted_date,
                "Job Skills": skills
            })

            job_count += 1

        # Close the driver once all jobs are scraped
        driver.quit()

    def extract_job_description(self, driver, job_link):
        """Fetch job description from the job link using the same driver instance."""
        driver.get(job_link)
        time.sleep(5)  # Wait for the job description to load
        soup = BeautifulSoup(driver.page_source, 'html.parser')
        
        # Extract job description text
        description_element = soup.select_one('div#jobDescriptionText')
        return description_element.get_text(strip=True) if description_element else ""

    def extract_skills(self, job_description):
        """Extract skills using entity recognition, predefined skills, and phrase matching."""
        doc = nlp(job_description)
        skills = set()

        # Entity Recognition for organization/product names
        for entity in doc.ents:
            if entity.label_ in ["ORG", "PRODUCT", "GPE"] and entity.text.lower() in predefined_skills:
                skills.add(entity.text.lower())

        # Phrase matching for multi-word skills
        matches = self.phrase_matcher(doc)
        skills.update({doc[start:end].text.lower() for _, start, end in matches})

        # POS tagging and frequency filtering
        word_freq = Counter(token.text.lower() for token in doc if token.pos_ in ["NOUN", "PROPN", "ADJ"])
        for word, freq in word_freq.items():
            if (
                (word in predefined_skills or freq > 2) and  # Frequency and predefined list filter
                word not in stop_words
            ):
                skills.add(word)

        return list(skills)

    def get_jobs(self):
        return self.jobs

@app.route('/scrape_jobs', methods=['POST'])
def scrape_jobs():
    data = request.get_json()
    job_title = data.get('job_Title')
    location = data.get('location')

    scraper = JobScraper(job_title, location)
    scraper.scrape_job_links()
    job_links = scraper.get_jobs()

    # Construct the response string with each job as a separate line
    response = ""
    for job in job_links:
        response += f"{json.dumps(job)}\n"

    return response, 200, {'Content-Type': 'application/json; charset=utf-8'}

if __name__ == '__main__':
    app.run(port=5000)
