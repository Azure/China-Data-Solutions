# /usr/bin/env python
# encoding=utf-8

import json

from bottle import route, request

import jieba
from jieba import posseg

jieba.load_userdict("dicts/emotion/user.dict")  # 准备分词词典


@route('/words', method="POST")
def index():
    query = request.forms.get("query")
    tokens = posseg.lcut(query)
    terms = []
    m_exist = {}
    for word, flag in tokens:
        if flag in ['n', 'nr', 'ns', 'nt', 'nz', 'nl', 'ng', 'vd', 'vn', 'ad', 'an', 'ag', 'al', 'eng']:
            if word not in m_exist:
                terms.append(word)
                m_exist[word] = 1
    datas = {'words': terms}
    json_data = json.dumps(datas, ensure_ascii=False)
    return json_data
