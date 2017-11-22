import os
import base64
import urllib
import numpy as np
from flask import Flask, render_template, request, json, jsonify
from flask_cors import cross_origin
from io import BytesIO
from PIL import Image, ImageOps

from keras.models import model_from_json
import cv2
import skimage.feature
import h5py

app = Flask(__name__)


@app.route("/")
def index():
    return render_template('index.html')

@app.route("/api/uploader", methods=['POST'])
@cross_origin()
def api_upload_file():
    img = Image.open(BytesIO(request.files['imagefile'].read())).convert('RGB')
    img = ImageOps.fit(img, (64, 64), Image.ANTIALIAS)
    prelables = run_deep_learning_model(img)
    result = {"Predict": " ".join(str(x) for x in prelables)}
    return jsonify(result)




def run_deep_learning_model(rgb_pil_image):
    # Convert to BGR
    rgb_image = np.array(rgb_pil_image, dtype=np.float32)
    bgr_image = rgb_image[..., [2, 1, 0]]
    preX = []
    preX.append(bgr_image)
    preX = np.array(preX)
    preY = modelX.predict(preX)
    preY_labels = np.argmax(preY, axis=1)
    return  preY_labels



modelX=model_from_json(open('Model/my_model_architecture.json').read())  
modelX.load_weights('Model/my_model_weights.h5')

