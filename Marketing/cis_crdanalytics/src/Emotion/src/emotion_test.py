#encoding=utf-8


def test_dict():
    #"""test the classifier based on Sentiment Dict"""
    #print("DictClassifier")
    #print("---" * 45)

    from emotion_classifiers import EmotionDictClassifier

    ds = EmotionDictClassifier()


    #sentence = "外观漂亮,外观很美"
    sentence = "太丑了"
    print sentence
    result = ds.analyse_sentence(sentence, None, False)
    print "emotion analysis : " + str(result)


if __name__ == "__main__":
    # pass
    test_dict()

