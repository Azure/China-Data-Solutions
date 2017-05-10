# /usr/bin/env python
# encoding=utf-8

import json

from topic_classifiers import TopicDictClassifier
from bottle import route, request

ds = TopicDictClassifier()


@route('/topic', method="POST")
def index():
    query = request.forms.get("query")
    topic = ds.analyse_sentence(query, None, False)
    datas = {"category": topic}
    json_data = json.dumps(datas, ensure_ascii=False)
    return json_data


