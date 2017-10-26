# Image Classification with Microsoft AI Platform [中文](README.md)
## Summary
Image classification is among the most popular applications of deep learning. For example, the insurance company can classify the car accidents based on the photo of the damaged part and make decision whether the claim is valid or not. For manufacturing factories, they may inspect whether there is defect in the product based on the images. For this project, we will use the images from the parts of the vehicle to judge whether there is a defect with it, such as dents or scratches on the surface. Microsoft’s Azure cloud ecosystem provides the scalable, elastic and intelligent AI platform. We will demonstrate how to develop the end to end AI solution with Azure AI platform. The solution will include:

- Image processing: process the images for feature extraction;
- Deep learning model: building a CNN (Convolutional Neural Networks);
- Model training: show how to create a CNN to train the model;
- Transfer learning: using pre-trained deep learning model to fine tune the new model;
- Deploy the model as a web service: demonstrate how to deploy the trained model as a web service;
- Consume the web service: How to invoke the model;
- PowerBI dashboard: building a dashboard to track the performance 

## Repository Structure 
- **Code**: this directory contains the instruction and Jupyter script for the CNN model training
- **Data**: this directory stores sample datasets for training and testing.
- **Deploy**: this directory contains an end to end deploy package to help you deploy this solution to your Azure subscription.