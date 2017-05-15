#!/usr/bin/evn python
# -*- coding:utf-8 -*-
from sklearn.ensemble import GradientBoostingClassifier
from sklearn.metrics import roc_auc_score
from sklearn import preprocessing
import sklearn.utils
import pandas as pd
import numpy as np
import networkx as nx
import os
import random


def process_features(data):
    """addressing features"""
    data.loc[data.user_province != src_user_info.user_province.iloc[0], "user_province"] = 0
    data.loc[data.user_province == src_user_info.user_province.iloc[0], "user_province"] = 1
    # better log
    data.user_followers_count -= src_user_info.user_followers_count.iloc[0]
    data.user_statuses_count -= src_user_info.user_statuses_count.iloc[0]
    # gender difference
    data["user_gender_diff"] = 0
    data.loc[data.user_gender != src_user_info.user_gender.iloc[0], "user_gender_diff"] = 1
    gender_dict = {u'\u7537': 0, u'\u5973': 1, u'\ufeff\u5973': 2}
    data.user_gender = data.user_gender.map(gender_dict)
    return data

cur_dir = os.path.dirname(os.path.abspath(__file__)) or os.getcwd()
base_dir = os.path.dirname(cur_dir)

sn = nx.DiGraph()
data = pd.read_json("D:/Models/sna-demo/data/weibo.json")

print("building social network...")
for _, fr, to, w in data.itertuples():
    sn.add_edge(str(fr), str(to), weight=w)
nx.write_gpickle(sn, "D:/Models/sna-demo/data/weibo2.gpickle")
#user_details = pd.read_json(base_dir + '/data/weibo_user_detail.json')
user_details = pd.read_json("D:/Models/sna-demo/data/weibo_user_data.json")
#sn = nx.read_gpickle("C:/Models/sna-demo/data/weibo.gpickle")
print("Total Users: ", user_details.shape[0])

# source user, picked the most powerful KOL
src_uidlist = [1645005104, 5122220173, 6055750198, 6038290538,2434411070, 3910813988, 2656274875, 2286908003, 1496814565,6001885837]
for srcuid in src_uidlist:
    #srcuid = 1645005104
    src_user_info = user_details[user_details.user_uid == srcuid]

    # select positive cases, that user B retweeted user A, classs label is 1.
    pos_uid = sn.successors(str(srcuid))
    pos_user_info = user_details[user_details.user_uid.isin(map(int, pos_uid))][[#"user_uid",
                                                                                 "user_followers_count",
                                                                                 "user_statuses_count", 
                                                                                 "user_province",
                                                                                 "user_gender",
                                                                                 "user_verified"]]
    pos_user_info["label"] = 1
    pos_num = pos_user_info.shape[0]
    # select negative cases, that user B didn't retweet user A, classs label is
    # 0.
    neg_user_info = user_details[~user_details.user_uid.isin(map(int, pos_uid)) & user_details.user_uid != srcuid][[#"user_uid",
                                                                   "user_statuses_count",
                                                                   "user_province", 
                                                                   "user_followers_count", 
                                                                   "user_gender", 
                                                                   "user_verified"]][:pos_num]
    neg_user_info["label"] = 0

    training = process_features(pos_user_info.append(neg_user_info))
    print("Total training records:", training.shape[0])

    test_size = 0.2

    training = sklearn.utils.shuffle(training)

    training.to_csv('./trainingdatanew.csv', sep=',', encoding='utf-8')

    training = training.sample(frac=1).reset_index(drop=True)

    training.to_csv('./trainingdatasample.csv', sep=',', encoding='utf-8')
    

    
    features = training.loc[:, [# "user_uid",
                                "user_followers_count",
                                "user_gender", "user_province", "user_statuses_count",
                                "user_verified", "user_gender_diff"]]
    # print np.where(pd.isnull(features))
    # for gender has unrecognized characters
    features.loc[features.user_gender.isnull(), "user_gender"] = 3
    scaled_features = preprocessing.scale(features)
    split_index = int(round(pos_num * 2 * test_size))
    test_x, train_x = scaled_features[:split_index], scaled_features[split_index:]
    test_y, train_y = training["label"][:split_index], training["label"][split_index:]

    print("Training and evaluating GBDT model...")
    clf = GradientBoostingClassifier(n_estimators=100, learning_rate=1.0,
                                     max_depth=1, random_state=0).fit(train_x, train_y)

    print(clf.score(test_x, test_y))
    predictions = clf.predict(test_x)
    auc = roc_auc_score(test_y, predictions)
    print(auc)

    to_predict = user_details[user_details.user_uid != srcuid][["user_uid",
                                                                "user_followers_count",
                                                                "user_statuses_count", "user_province",
                                                                "user_gender", "user_verified"]]
    to_predict = process_features(to_predict)
    features = to_predict.loc[:, [#"user_uid",
                                "user_followers_count", 
                                  "user_gender", "user_province", "user_statuses_count",
                                  "user_verified", "user_gender_diff"]]

    # print np.where(pd.isnull(features))
    # for gender has unrecognized characters
    features.loc[features.user_gender.isnull(), "user_gender"] = 3
    scaled_features = preprocessing.scale(features)
    prob = clf.predict_proba(scaled_features)
    to_predict["prob"] = prob[:, 0]
    # writing result
    prob_pd = to_predict
    prob_pd["src_uid"] = srcuid
    #to_predict.loc[to_predict.shape[0]] = [srcuid, 0.5]
    prob_pd.to_json("D:/Projects/SNAGBDT/SNAGBDT/data/diffusion_prob{0}.json".format(srcuid), orient='records')
print("done!")
