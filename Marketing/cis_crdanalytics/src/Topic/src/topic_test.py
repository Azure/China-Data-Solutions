# encoding=utf-8


def test_dict():
    # """test the classifier based on Sentiment Dict"""
    # print("DictClassifier")
    # print("---" * 45)

    from topic_classifiers import TopicDictClassifier

    ds = TopicDictClassifier()

    sentence = "外壳很漂亮"
    print sentence
    result = ds.analyse_sentence(sentence, None, False)
    print "topic analysis : " + str(result)


if __name__ == "__main__":
    # pass
    test_dict()
