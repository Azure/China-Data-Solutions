package com.ehoo.common.util;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Locale;

/**
 * Created by guoqing.zhou on 2017/3/4.
 */
public class DateUtils {

        public static String getCurrTime(){
                SimpleDateFormat sDateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
                String date = sDateFormat.format(new   java.util.Date());
                return date;
        }

        public static String dateFormat(String date) throws Exception {
                SimpleDateFormat sf = new SimpleDateFormat("EEE MMM dd HH:mm:ss Z yyyy", Locale.US);
                Date s = sf.parse(date);
                SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
                return sdf.format(s);
        }
        //获取偏移之后的日期
        public static String getAddDate(int num){
                Date date=new Date();//取时间
                Calendar calendar = Calendar.getInstance();
                calendar.setTime(date);
                calendar.add(calendar.DATE,num);//把日期往后增加一天.整数往后推,负数往前移动
                date=calendar.getTime(); //这个时间就是日期往后推一天的结果
                SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd");
                String dateString = formatter.format(date);
                return dateString;
        }

        public static String dateFormatFL(String date) throws Exception {
                SimpleDateFormat sf = new SimpleDateFormat("yyyy-MM-dd HH:mm");
                Date s = sf.parse(date);
                SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
                return sdf.format(s);
        }
}
