package com.ehoo.task;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.ehoo.DAO.SentimentDAO;
import com.ehoo.common.config.Config;
import com.ehoo.common.util.DateUtils;
import com.ehoo.common.util.HttpRequestUtils;
import com.ehoo.vo.Comment;
import org.apache.commons.lang3.StringUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import java.util.*;
import java.util.concurrent.*;

/**
 * Created by guoqing.zhou on 2017/3/5.
 */
@Component
@EnableScheduling
public class SentimentProcessTask {


    // 对数据进行二次处理，主要处理满意度相关
    private static boolean processed = false;

    @Autowired
    private SentimentDAO sentimentDAO;

    private static ExecutorService executor = Executors.newCachedThreadPool();

    private static final int COUNT = 100;
    private static long start = 0;

    @Scheduled(fixedDelay = 1)
    public void sentimentProcess() {
        if(start == 0){
            start = System.currentTimeMillis();
        }
        //从comment中取出未做情感分析的数据
        List<Map<String, Object>> comments = sentimentDAO.getUnProcessData(COUNT);
        List<Comment> resComments = new ArrayList<Comment>();
        List<String> fail = new ArrayList<String>();

        if (comments.size() > 0) {
            List<FutureTask<Comment>> processList = new ArrayList<FutureTask<Comment>>();

            for (Map<String, Object> commentMap : comments) {
                processList.add(new FutureTask<Comment>(new process(commentMap)));
            }
            for (int i = 0; i < processList.size(); i++) {
                executor.execute(processList.get(i));
            }
            for (int i = 0; i < processList.size(); i++) {
                try {
                    Comment result = processList.get(i).get();
                    if (result != null) {
                        resComments.add(result);
                    }
                } catch (InterruptedException e) {
                    e.printStackTrace();
                } catch (ExecutionException e) {
                    e.printStackTrace();
                }
            }
            sentimentDAO.updateCommentSentimentBatch(resComments);
        } else {
            if (!processed) {
                // 修改情感标签
                sentimentDAO.updateSnetimentTag();
                // 修改满意度
                sentimentDAO.updateSatification();
                // 将日期修改为2017-01-01的一周内
                sentimentDAO.updateDatetime();
                processed = true;
                long end = System.currentTimeMillis();
                System.out.println("总重耗时：" + (end - start) / 1000 + " 秒");
            }

        }

    }
}


class process implements Callable<Comment> {
    private Map<String, Object> commentMap;

    public process(Map<String, Object> commentMap) {
        this.commentMap = commentMap;
    }

    public Comment call() throws Exception {
        String id = commentMap.get("id").toString();
        try {
            String content = commentMap.get("comment").toString();
            Map<String, String> params = new HashMap<String, String>();
            params.put("query", content);
            String topicJson = HttpRequestUtils.doPost(Config.topic, params);
            if (StringUtils.isBlank(topicJson)) {
                return null;
            }
            JSONObject topic = JSONObject.parseObject(topicJson);
            String emotionJson = HttpRequestUtils.doPost(Config.emotion, params);
            if (StringUtils.isBlank(emotionJson)) {
                return null;
            }
            JSONObject emotion = JSONObject.parseObject(emotionJson);
            String wordsJson = HttpRequestUtils.doPost(Config.words, params);
            if (StringUtils.isBlank(wordsJson)) {
                return null;
            }
            JSONObject words = JSONObject.parseObject(wordsJson);
            Comment comment = new Comment();
            comment.setProduct_name(commentMap.get("product_name").toString());
            comment.setRate_time(commentMap.get("rate_time").toString());
            comment.setSource(commentMap.get("source").toString());
            comment.setId(id);
            comment.setProcessed(1);
            comment.setTopic(topic.getString("category"));
            comment.setSentimence(emotion.getString("score"));
            JSONArray hotkeys = words.getJSONArray("words");
            if (hotkeys != null && hotkeys.size() > 0) {
                Iterator<Object> ih = hotkeys.iterator();
                while (ih.hasNext()) {
                    comment.hotkeys.add(ih.next().toString());
                }
            }
            comment.setProcess_time(DateUtils.getCurrTime());
            return comment;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }
}
