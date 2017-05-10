## Overview
For retail company, the best way to understand the quality of product and service is by analyzing customers' feedback, as the most common way for customers to express their emotion is  ranking or commenting the product. With this tutorial, Market and Operation teams can analyze, monitor the hot issues of the product by analyzing the customer review data crawled from e-commerce websites, and gain multi-dimensions insights by qualitative analyzing.

Customer Feedback Analytics Tutorial is a data analytics project for digital marketing, it is built atop NLP models and Azure services. The customer review data used in this tutorial is for IT products, which crawled from the most popular e-commerce websites in China - JD and TMall. The tutorial can calculate the customer satisfaction from multi-dimensions (e.g. pre-sales service, after-sales service, pricing, etc.) to help market team understanding major problems reported by customers. The major model used in the solution is Chinese sentiment analytics model, this model had trained with Azure services based on Chinese review data.

## Technical Diagram
![Technical Diagram]({PatternAssetBaseUrl}\CRDAnalytics.png)

## Technical Details
The tutorial uses many Azure services to build e2e business pipeline.

### Data Storage
The original customer review data and analytics result will be stored in SQL Azure database.

* "comment" table for original customer review data
* "sentimence" table for emotion model analytics results
* "topic" table for topic model analytics results
* "hotkeys" table for words model analytics results

### Data Models

#### Emotion Data Model

The Emotion WebApp contains a Chinese Sentiment Data Model, could receive post request as following:

> POST http://{EmotionWebAppUrl}/emotion HTTP/1.1
> 
> Content-Type: application/x-www-form-urlencoded; charset=utf-8
> 
> query={{Chinese Sentence}}

Sample response:

> {"score": 2.0}

The score means the sentiment result is positive or negative.

#### Topic Data Model

The Topic WebApp contains a Chinese Topic Classification Data Model, could receive post request as following:

> POST http://{TopicWebAppUrl}/topic HTTP/1.1
> 
> Content-Type: application/x-www-form-urlencoded; charset=utf-8
> 
> query={{Chinese Sentence}}

Sample response:

> {"category": "Performance"}

The category means the topic for the given sentence.

#### Words Data Model

The Words WebApp contains a Chinese Keywords Extraction Data Model, could receive post request as following:

> POST http://{WordsWebAppUrl}/words HTTP/1.1
> 
> Content-Type: application/x-www-form-urlencoded; charset=utf-8
> 
> query={{Chinese Sentence}}

Sample response:

> {"words": ["Computer", "Battery", "Problem"]}

The response contains the extracted keywords for the given sentence.

### Data Pipeline

The Pipeline WebApp host a scheduler task to process customer review data with the above three data models.

### Data Visualization

The PowerBI embedded dashboards contains some built-in diagrams for common analytics requirements from Marketing and Operation perspective.

## Solution for Azure China
> We also provides solution for Azure China, please refer to the solution [introduction](https://www.azure.cn/solutions/industry-precisionmarketing/) of digital marketing solutions in Chinese.

We share the **sample code**, **deployable solution package**, **PBI demo project file** of the solution on [github](https://github.com/Azure/China-Data-Solutions/tree/master/Marketing/CRDAnalytics) for Azure China customers.
