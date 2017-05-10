package com.ehoo.vo;

import java.util.ArrayList;
import java.util.List;

/**
 * 电商评论
 * Created by guoqing.zhou on 2017/3/3.
 */
public class Comment {
        private String id;
        private int raw_id;//原始数据id
        private Long comment_id;//评论id
        private String product_name;//产品名称
        private String comment;//评论
        private String after_comment;//追加评价内容
        private int score;//评分
        private String rate_time;//评价时间
        private int useful_count;//有用数量
        private int useless_count;//无用数量
        private int reply_count;//讨论数
        private String core;//cpu型号
        private String product_size;//存储大小
        private String package_version;//套餐版本
        private String product_color;//颜色分类
        private String nick_name;//评论人
        private String user_level;//评论人等级
        private String lacation;//位置
        private String client;//客户端
        private String source;//来源
        private int processed;//是否情感分析处理
        private String topic;//情感分析得出的分类
        private String sentimence;//情感评级
        private String process_time;//情感分析时间
        private String update_time;//修改时间
        private String create_time;//生成时间
        public List<String> hotkeys = new ArrayList<String>();

        public String getId() {
                return id;
        }

        public void setId(String id) {
                this.id = id;
        }

        public int getRaw_id() {
                return raw_id;
        }

        public void setRaw_id(int raw_id) {
                this.raw_id = raw_id;
        }

        public Long getComment_id() {
                return comment_id;
        }

        public void setComment_id(Long comment_id) {
                this.comment_id = comment_id;
        }

        public String getProduct_name() {
                return product_name;
        }

        public void setProduct_name(String product_name) {
                this.product_name = product_name;
        }

        public String getComment() {
                return comment;
        }

        public void setComment(String comment) {
                this.comment = comment;
        }

        public String getAfter_comment() {
                return after_comment;
        }

        public void setAfter_comment(String after_comment) {
                this.after_comment = after_comment;
        }

        public int getScore() {
                return score;
        }

        public void setScore(int score) {
                this.score = score;
        }

        public String getRate_time() {
                return rate_time;
        }

        public void setRate_time(String rate_time) {
                this.rate_time = rate_time;
        }

        public int getUseful_count() {
                return useful_count;
        }

        public void setUseful_count(int useful_count) {
                this.useful_count = useful_count;
        }

        public int getUseless_count() {
                return useless_count;
        }

        public void setUseless_count(int useless_count) {
                this.useless_count = useless_count;
        }

        public int getReply_count() {
                return reply_count;
        }

        public void setReply_count(int reply_count) {
                this.reply_count = reply_count;
        }

        public String getCore() {
                return core;
        }

        public void setCore(String core) {
                this.core = core;
        }

        public String getProduct_size() {
                return product_size;
        }

        public void setProduct_size(String product_size) {
                this.product_size = product_size;
        }

        public String getPackage_version() {
                return package_version;
        }

        public void setPackage_version(String package_version) {
                this.package_version = package_version;
        }

        public String getProduct_color() {
                return product_color;
        }

        public void setProduct_color(String product_color) {
                this.product_color = product_color;
        }

        public String getNick_name() {
                return nick_name;
        }

        public void setNick_name(String nick_name) {
                this.nick_name = nick_name;
        }

        public String getUser_level() {
                return user_level;
        }

        public void setUser_level(String user_level) {
                this.user_level = user_level;
        }

        public String getLacation() {
                return lacation;
        }

        public void setLacation(String lacation) {
                this.lacation = lacation;
        }

        public String getClient() {
                return client;
        }

        public void setClient(String client) {
                this.client = client;
        }

        public String getSource() {
                return source;
        }

        public void setSource(String source) {
                this.source = source;
        }

        public int getProcessed() {
                return processed;
        }

        public void setProcessed(int processed) {
                this.processed = processed;
        }

        public String getTopic() {
                return topic;
        }

        public void setTopic(String topic) {
                this.topic = topic;
        }

        public String getSentimence() {
                return sentimence;
        }

        public void setSentimence(String sentimence) {
                this.sentimence = sentimence;
        }

        public String getProcess_time() {
                return process_time;
        }

        public void setProcess_time(String process_time) {
                this.process_time = process_time;
        }

        public String getUpdate_time() {
                return update_time;
        }

        public void setUpdate_time(String update_time) {
                this.update_time = update_time;
        }

        public String getCreate_time() {
                return create_time;
        }

        public void setCreate_time(String create_time) {
                this.create_time = create_time;
        }
}
