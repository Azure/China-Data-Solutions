# /usr/bin/env python
# encoding=utf-8

import json

from emotion_classifiers import EmotionDictClassifier

from bottle import route, request

ds = EmotionDictClassifier()


@route('/emotion', method="POST")
def index():
    query = request.forms.get("query")
    score = ds.analyse_sentence(query, None, False)
    datas = {"score": score}
    json_data = json.dumps(datas)
    return json_data
