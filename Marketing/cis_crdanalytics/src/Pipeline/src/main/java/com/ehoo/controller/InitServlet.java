package com.ehoo.controller;

import com.ehoo.common.config.Config;
import org.dom4j.Document;
import org.dom4j.Element;
import org.dom4j.io.SAXReader;

import javax.servlet.ServletConfig;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import java.io.*;
import java.util.Iterator;

/**
 * Created by guoqing.zhou on 2017/3/24.
 */
public class InitServlet extends HttpServlet {

    @Override
    public void init(ServletConfig config) throws ServletException {
        String path = config.getServletContext().getRealPath("");
        String configFilePath = path.substring(0,(path.lastIndexOf("\\webapps") + 9))+"config.xml";
        //String configFilePath = "D:\\apache-tomcat-8.0.33\\webapps\\config.xml";
        try {
            InputStream is = new FileInputStream(new File(configFilePath));
            //创建SAXReader读取器
            SAXReader reader = new SAXReader();
            Document document = reader.read(is);
            Element element = document.getRootElement();
            Iterator<Element> ite = element.elementIterator();
            Element appkey = element.element("appkey");
            Config.appkey = appkey.getText();
            Element workspaceId = element.element("workspaceId");
            Config.workspaceId = workspaceId.getText();
            Element reportId = element.element("reportId");
            Config.reportId = reportId.getText();
            Element wcn = element.element("wcn");
            Config.wcn = wcn.getText();
            Element words = element.element("words");
            Config.words = words.getText();
            Element emotion = element.element("emotion");
            Config.emotion = emotion.getText();
            Element topic = element.element("topic");
            Config.topic = topic.getText();
            Element dataUrl = element.element("dataUrl");
            Config.dataUrl = dataUrl.getText();
        } catch (Exception e) {
            e.printStackTrace();
        }
        super.init(config);
    }

}
