package com.ehoo.DAO;

import com.ehoo.common.config.Config;
import com.ehoo.task.SentimentProcessTask;
import com.ehoo.vo.Comment;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.sql.*;
import java.util.*;

/**
 * Created by guoqing.zhou on 2017/3/5.
 */
@Repository
public class SentimentDAO {
    private String driverName = "com.microsoft.sqlserver.jdbc.SQLServerDriver";


    public void updateDatetime() {
        Connection conn = null;
        Statement st = null;
        try {
            Class.forName(driverName);
            conn = DriverManager.getConnection(Config.dataUrl);
            String sql = "update comment set data_time = CONVERT(VARCHAR(11),DATEADD(day, 7*RAND(CHECKSUM(NEWID())), '2017-01-01'),120)";
            st = conn.createStatement();
            st.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            try {
                if (st != null) {
                    st.close();
                }
                if (conn != null) {
                    conn.close();
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public void updateSatification() {
        Connection conn = null;
        Statement st = null;
        try {
            Class.forName(driverName);
            conn = DriverManager.getConnection(Config.dataUrl);
            String sql = "update comment  set satification = \n" +
                    "CASE \n" +
                    "WHEN sentimence > 100 THEN 100\n" +
                    "WHEN sentimence >= 60 AND sentimence < 100 THEN 80\n" +
                    "WHEN sentimence >= 30 AND sentimence < 60 THEN 60\n" +
                    "WHEN sentimence >= 10 AND sentimence < 30 THEN 40\n" +
                    "WHEN sentimence > 0 AND sentimence < 10 THEN 20\n" +
                    "WHEN sentimence <= 0 THEN 0\n" +
                    "END";
            st = conn.createStatement();
            st.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            try {
                if (st != null) {
                    st.close();
                }
                if (conn != null) {
                    conn.close();
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public void updateSnetimentTag() {
        Connection conn = null;
        Statement st = null;
        try {
            Class.forName(driverName);
            conn = DriverManager.getConnection(Config.dataUrl);
            String sql = "update comment  set sentiment_tag = \n" +
                    "CASE \n" +
                    "WHEN sentimence > 0 THEN N'正面'\n" +
                    "WHEN sentimence = 0 THEN N'中立'\n" +
                    "WHEN sentimence < 0 THEN N'负面'\n" +
                    "END";
            st = conn.createStatement();
            st.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            try {
                if (st != null) {
                    st.close();
                }
                if (conn != null) {
                    conn.close();
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }


    public List<Map<String, Object>> getUnProcessData(int count) {
        List<Map<String, Object>> datas = new ArrayList<Map<String, Object>>();
        Connection con = null;
        Statement stmt = null;
        ResultSet rs = null;


        try {
            Class.forName(driverName);
            con = DriverManager.getConnection(Config.dataUrl);
            String sql = "SELECT TOP " + count + " c.id,c.processed,c.comment,c.rate_time,c.topic,c.sentimence,c.process_time,c.product_name,c.source from comment c" +
                    " WHERE c.processed = 0 ORDER BY c.create_time";
            stmt = con.createStatement();
            rs = stmt.executeQuery(sql);
            ResultSetMetaData rsmd = rs.getMetaData();
            int columnCount = rsmd.getColumnCount();
            Map<String, Object> data = null;
            while (rs.next()) {
                data = new HashMap<String, Object>();
                for (int i = 1; i <= columnCount; i++) {
                    data.put(rsmd.getColumnLabel(i), rs.getObject(rsmd
                            .getColumnLabel(i)));
                }
                datas.add(data);
            }
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            if (rs != null) {
                try {
                    rs.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (stmt != null) {
                try {
                    stmt.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (con != null) {
                try {
                    con.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
        return datas;
    }

    public void updateCommentSentiment(Comment comment) {
        long start = System.currentTimeMillis();
        Connection con = null;
        PreparedStatement pComment = null;
        PreparedStatement pTopic = null;
        PreparedStatement pSentimence = null;
        PreparedStatement pHotKeys = null;
        try {
            Class.forName(driverName);
            con = DriverManager.getConnection(Config.dataUrl);
            con.setAutoCommit(false);
            String updateCommentSql = "UPDATE comment set processed = ?,topic = ?,sentimence = ?,process_time = ? WHERE id = ?";
            pComment = con.prepareStatement(updateCommentSql);
            pComment.setInt(1, 1);
            pComment.setString(2, comment.getTopic());
            pComment.setString(3, comment.getSentimence());
            pComment.setString(4, comment.getProcess_time());
            pComment.setString(5, comment.getId());
            pComment.execute();
            String insertTopicSql = "INSERT INTO topic(id,comment_id,topic,source,rate_time,create_time) VALUES (?,?,?,?,?,?)";
            pTopic = con.prepareStatement(insertTopicSql);
            pTopic.setString(1, UUID.randomUUID().toString().replaceAll("-", ""));
            pTopic.setString(2, comment.getId());
            pTopic.setString(3, comment.getTopic());
            pTopic.setString(4, comment.getSource());
            pTopic.setString(5, comment.getRate_time());
            pTopic.setString(6, comment.getProcess_time());
            pTopic.execute();
            String insertSentimenceSql = "INSERT INTO sentimence(id,comment_id,sentimence,source,rate_time,create_time) VALUES (?,?,?,?,?,?)";
            pSentimence = con.prepareStatement(insertSentimenceSql);
            pSentimence.setString(1, UUID.randomUUID().toString().replaceAll("-", ""));
            pSentimence.setString(2, comment.getId());
            pSentimence.setString(3, comment.getSentimence());
            pSentimence.setString(4, comment.getSource());
            pSentimence.setString(5, comment.getRate_time());
            pSentimence.setString(6, comment.getProcess_time());
            pSentimence.execute();
            List<String> hotkeys = comment.hotkeys;
            if (hotkeys.size() > 0) {

                String insertHotkeySql = "INSERT INTO hotkeys(id,comment_id,product,hotkey,source,rate_time,create_time) VALUES (?,?,?,?,?,?,?)";
                pHotKeys = con.prepareStatement(insertHotkeySql);
                for (int i = 0; i < hotkeys.size(); i++) {
                    pHotKeys.setString(1, UUID.randomUUID().toString().replaceAll("-", ""));
                    pHotKeys.setString(2, comment.getId());
                    pHotKeys.setString(3, comment.getProduct_name());
                    pHotKeys.setString(4, hotkeys.get(i));
                    pHotKeys.setString(5, comment.getSource());
                    pHotKeys.setString(6, comment.getRate_time());
                    pHotKeys.setString(7, comment.getProcess_time());
                    pHotKeys.addBatch();
                }
                pHotKeys.executeBatch();
            }
            con.commit();
        } catch (Exception e) {
            try {
                con.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
            e.printStackTrace();

        } finally {
            if (con != null) {
                try {
                    con.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (pComment != null) {
                try {
                    pComment.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (pTopic != null) {
                try {
                    pTopic.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (pHotKeys != null) {
                try {
                    pHotKeys.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
        long end = System.currentTimeMillis();
    }

    public void updateCommentSentimentBatch(List<Comment> comments) {
        List<String> ignoreList = new ArrayList<String>();
        ignoreList.add("问题");
        ignoreList.add("电脑");
        ignoreList.add("渠道");
        ignoreList.add("东西");
        ignoreList.add("感觉");
        ignoreList.add("公司X");
        ignoreList.add("产品A");
        ignoreList.add("产品B");
        ignoreList.add("公司Y");
        ignoreList.add("有点");
        ignoreList.add("平板");
        ignoreList.add("产品");
        ignoreList.add("A");
        ignoreList.add("X");
        ignoreList.add("Y");
        ignoreList.add("B");
        ignoreList.add("时候");
        ignoreList.add("机器");
        ignoreList.add("nik");
        ignoreList.add("eve");
        ignoreList.add("xx");
        ignoreList.add("But");
        ignoreList.add("ZM5");
        ignoreList.add("ZM5");
        ignoreList.add("IE");
        ignoreList.add("See");
        ignoreList.add("Z");
        ignoreList.add("C");
        ignoreList.add("ime");
        ignoreList.add("ID");
        Connection con = null;
        PreparedStatement pComment = null;
        PreparedStatement pTopic = null;
        PreparedStatement pSentimence = null;
        PreparedStatement pHotKeys = null;
        try {
            Class.forName(driverName);
            con = DriverManager.getConnection(Config.dataUrl);
            con.setAutoCommit(false);
            String updateCommentSql = "UPDATE comment set processed = ?,topic = ?,sentimence = ?,process_time = ? WHERE id = ?";
            pComment = con.prepareStatement(updateCommentSql);
            String insertTopicSql = "INSERT INTO topic(id,comment_id,topic,source,rate_time,create_time) VALUES (?,?,?,?,?,?)";
            pTopic = con.prepareStatement(insertTopicSql);
            String insertSentimenceSql = "INSERT INTO sentimence(id,comment_id,sentimence,source,rate_time,create_time) VALUES (?,?,?,?,?,?)";
            pSentimence = con.prepareStatement(insertSentimenceSql);
            String insertHotkeySql = "INSERT INTO hotkeys(id,comment_id,product,hotkey,source,rate_time,create_time) VALUES (?,?,?,?,?,?,?)";
            pHotKeys = con.prepareStatement(insertHotkeySql);
            for (Comment comment : comments) {

                pComment.setInt(1, 1);
                pComment.setString(2, comment.getTopic());
                pComment.setString(3, comment.getSentimence());
                pComment.setString(4, comment.getProcess_time());
                pComment.setString(5, comment.getId());
                pComment.addBatch();


                pTopic.setString(1, UUID.randomUUID().toString().replaceAll("-", ""));
                pTopic.setString(2, comment.getId());
                pTopic.setString(3, comment.getTopic());
                pTopic.setString(4, comment.getSource());
                pTopic.setString(5, comment.getRate_time());
                pTopic.setString(6, comment.getProcess_time());
                pTopic.addBatch();


                pSentimence.setString(1, UUID.randomUUID().toString().replaceAll("-", ""));
                pSentimence.setString(2, comment.getId());
                pSentimence.setString(3, comment.getSentimence());
                pSentimence.setString(4, comment.getSource());
                pSentimence.setString(5, comment.getRate_time());
                pSentimence.setString(6, comment.getProcess_time());
                pSentimence.addBatch();
                List<String> hotkeys = comment.hotkeys;
                if (hotkeys.size() > 0) {
                    for (int i = 0; i < hotkeys.size(); i++) {
                        if (ignoreList.contains(hotkeys.get(i))) {
                            continue;
                        }
                        pHotKeys.setString(1, UUID.randomUUID().toString().replaceAll("-", ""));
                        pHotKeys.setString(2, comment.getId());
                        pHotKeys.setString(3, comment.getProduct_name());
                        pHotKeys.setString(4, hotkeys.get(i));
                        pHotKeys.setString(5, comment.getSource());
                        pHotKeys.setString(6, comment.getRate_time());
                        pHotKeys.setString(7, comment.getProcess_time());
                        pHotKeys.addBatch();
                    }

                }
            }
            pComment.executeBatch();
            pTopic.executeBatch();
            pSentimence.executeBatch();
            pHotKeys.executeBatch();
            con.commit();
        } catch (Exception e) {
            try {
                con.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
            e.printStackTrace();

        } finally {
            if (con != null) {
                try {
                    con.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (pComment != null) {
                try {
                    pComment.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (pTopic != null) {
                try {
                    pTopic.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
            if (pHotKeys != null) {
                try {
                    pHotKeys.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
        long end = System.currentTimeMillis();
    }
}
