from flask import Flask

app = Flask(__name__)
counter = 1

@app.route("/")
def home():
    global counter
    counter += 1
    return str(counter)
    
@app.route("/salvador")
def salvador():
    return "Hello, Salvador"
    
if __name__ == "__main__":
    app.run(debug=True)