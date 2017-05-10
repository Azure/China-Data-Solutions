package com.ehoo.common.util;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.http.*;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpRequestRetryHandler;
import org.apache.http.client.config.RequestConfig;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.params.ClientPNames;
import org.apache.http.client.params.CookiePolicy;
import org.apache.http.config.Registry;
import org.apache.http.config.RegistryBuilder;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.socket.ConnectionSocketFactory;
import org.apache.http.conn.socket.LayeredConnectionSocketFactory;
import org.apache.http.conn.socket.PlainConnectionSocketFactory;
import org.apache.http.conn.ssl.SSLConnectionSocketFactory;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.impl.conn.PoolingHttpClientConnectionManager;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.CoreConnectionPNames;
import org.apache.http.params.CoreProtocolPNames;
import org.apache.http.params.HttpParams;
import org.apache.http.protocol.ExecutionContext;
import org.apache.http.protocol.HttpContext;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLHandshakeException;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;
import java.io.*;
import java.net.URLEncoder;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.*;

@SuppressWarnings("all")
public class HttpRequestUtils {
	/**
	 * 连接池里的最大连接数
	 */
	public static final int MAX_TOTAL_CONNECTIONS = 500;

	/**
	 * 每个路由的默认最大连接数
	 */
	public static final int MAX_ROUTE_CONNECTIONS = 500;

	/**
	 * 连接超时时间
	 */
	public static final int CONNECT_TIMEOUT = 300000;

	/**
	 * 套接字超时时间
	 */
	public static final int SOCKET_TIMEOUT = 300000;

	/**
	 * 连接池中 连接请求执行被阻塞的超时时间
	 */
	public static final long CONN_MANAGER_TIMEOUT = 300000;

	/**
	 * http连接相关参数
	 */
	private static HttpParams parentParams;

	/**
	 * http线程池管理器
	 */
	private static PoolingHttpClientConnectionManager cm;

	/**
	 * http客户端
	 */
	private static CloseableHttpClient httpClient;



	/**
	 * 初始化http连接池，设置参数、http头等等信息
	 */
	static {
		// 创建一个线程安全的HTTP连接池
		LayeredConnectionSocketFactory sslsf = null;
		try {
			sslsf = new SSLConnectionSocketFactory(SSLContext.getDefault());
		}catch (NoSuchAlgorithmException e){
			e.printStackTrace();
		}

		Registry<ConnectionSocketFactory> socketFactoryRegistry = RegistryBuilder.<ConnectionSocketFactory>create().register("https",sslsf)
			.register("http",new PlainConnectionSocketFactory()).build();
		cm = new PoolingHttpClientConnectionManager(socketFactoryRegistry);
		cm.setMaxTotal(500);
		cm.setDefaultMaxPerRoute(20);

		cm.setMaxTotal(MAX_TOTAL_CONNECTIONS);

		cm.setDefaultMaxPerRoute(MAX_ROUTE_CONNECTIONS);

		parentParams = new BasicHttpParams();
		parentParams.setParameter(CoreProtocolPNames.PROTOCOL_VERSION,
				HttpVersion.HTTP_1_1);

		parentParams.setParameter(ClientPNames.COOKIE_POLICY,
				CookiePolicy.BROWSER_COMPATIBILITY);

		parentParams.setParameter(ClientPNames.CONN_MANAGER_TIMEOUT,
				CONN_MANAGER_TIMEOUT);
		parentParams.setParameter(CoreConnectionPNames.CONNECTION_TIMEOUT,
				CONNECT_TIMEOUT);
		parentParams.setParameter(CoreConnectionPNames.SO_TIMEOUT,
				SOCKET_TIMEOUT);

		parentParams.setParameter(ClientPNames.ALLOW_CIRCULAR_REDIRECTS, true);
		parentParams.setParameter(ClientPNames.HANDLE_REDIRECTS, true);

		// 设置头信息,模拟浏览器
		Collection collection = new ArrayList();
		collection
				.add(new BasicHeader("User-Agent",
						"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0)"));
		collection
				.add(new BasicHeader("Accept",
						"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"));
		collection.add(new BasicHeader("Accept-Language",
				"zh-cn,zh,en-US,en;q=0.5"));
		collection.add(new BasicHeader("Accept-Charset",
				"ISO-8859-1,utf-8,gbk,gb2312;q=0.7,*;q=0.7"));
		//collection.add(new BasicHeader("Accept-Encoding", "gzip, deflate"));

		parentParams.setParameter(ClientPNames.DEFAULT_HEADERS, collection);
		

/*//		httpClient = new DefaultHttpClient(cm, parentParams);
//
//		httpClient.setHttpRequestRetryHandler(httpRequestRetryHandler);
		
		ScheduledExecutorService scheduler = Executors.newScheduledThreadPool(1);
		scheduler.scheduleAtFixedRate(new HttpConnectionClear(cm),CONNECT_TIMEOUT,CONNECT_TIMEOUT*2,TimeUnit.MILLISECONDS);
//		new Thread(new HttpConnectionClear(cm,CONNECT_TIMEOUT*2)).start();*/
	}
	
	private static synchronized CloseableHttpClient getHttpClient()
	{
		// 请求重试处理
		HttpRequestRetryHandler httpRequestRetryHandler = new HttpRequestRetryHandler() {
			public boolean retryRequest(IOException exception,
					int executionCount, HttpContext context) {
				if (executionCount >= 5) {
					// 如果超过最大重试次数，那么就不要继续了
					return false;
				}
				if (exception instanceof NoHttpResponseException) {
					// 如果服务器丢掉了连接，那么就重试
					return true;
				}
				if (exception instanceof SSLHandshakeException) {
					// 不要重试SSL握手异常
					return false;
				}
				HttpRequest request = (HttpRequest) context
						.getAttribute(ExecutionContext.HTTP_REQUEST);
				boolean idempotent = !(request instanceof HttpEntityEnclosingRequest);
				if (idempotent) {
					// 如果请求被认为是幂等的，那么就重试
					return true;
				}
				return false;
			}
		};

		RequestConfig requestConfig = RequestConfig.custom().setSocketTimeout(CONNECT_TIMEOUT)
			.setConnectionRequestTimeout(SOCKET_TIMEOUT).build();
		CloseableHttpClient httpclient = HttpClients.custom().setConnectionManager(cm)
			.setDefaultRequestConfig(requestConfig).build();

		return httpclient;
	}
	
	public static String doGet(String url, Map param)
	{
		String html = "";
		String paramstr = "?";
		if(param != null)
		{
			Iterator<Map.Entry<String,String>> ie = param.entrySet().iterator();
			while(ie.hasNext())
			{
				Map.Entry<String,String> en = ie.next();
				paramstr += en.getKey()+"="+en.getValue()+"&";
			}
			if(!paramstr.equals("?"))
			{
				paramstr = paramstr.substring(0, paramstr.length()-1);
			}
		}
		String httpurl = url;
		if(!paramstr.equals("?"))
			httpurl += paramstr;
		HttpGet httpGet = new HttpGet(httpurl);
		HttpResponse httpResponse;
        HttpEntity httpEntity;
        try {
        	httpResponse = getHttpClient().execute(httpGet);
            
            StatusLine statusLine = httpResponse.getStatusLine();
            int statusCode = statusLine.getStatusCode();
            if(200 != statusCode) {
                return "publicServerError";
            }
            httpEntity = httpResponse.getEntity();
            if(httpEntity != null)
            {
            	html = handleEntity(httpEntity);;
            }
            
		} catch (Exception e) {
			e.printStackTrace();
			return "publicServerError";
		}
		finally {
            if(httpGet != null){
                httpGet.releaseConnection();
            }
        }
		return html;

	}
	
	
	public static final String REQUEST_TYPE_GET = "get";
	public static final String REQUEST_TYPE_POST = "post";
	
	private static Log log = LogFactory.getLog(HttpRequestUtils.class);
	
	
	// 基础方法
	
	/**
	 * 模拟发出Http请求
	 * 
	 * @param url 请求资源,如：http://www.baidu.com/,注意严谨的格式
	 * @param params 请求参数
	 * @param method 请求方式,目前只支持get/post
	 * @param encoding 网页编码
	 * 
	 * @return HttpResponseBody
	 * 
	 * @throws IOException
	 * @throws ClientProtocolException 
	 */
	public static String request(String url, Map<String,String> params, String method, String encoding) throws ClientProtocolException, IOException {
		String result = "";
		DefaultHttpClient httpClient = new DefaultHttpClient();
		HttpResponse httpResponse = null;
		
		// GET方式请求
		if(HttpRequestUtils.REQUEST_TYPE_GET.equals(method)){
			// 加入请求参数
			if(params != null ){
				if(url.indexOf("?") != -1){
					url += "&";
				}else{
					url += "?";
				}
				for(String key : params.keySet()){
					if(params.get(key)!=null){
						url += key +"="+ URLEncoder.encode(params.get(key), "UTF-8")+"&";
					}
				}
			}
			HttpGet httpGet = new HttpGet(url);
			//httpResponse = httpClient.execute(httpGet);
			httpResponse = getHttpClient().execute(httpGet);
			
		// POST方式请求
		}else if(HttpRequestUtils.REQUEST_TYPE_POST.equals(method)){
			HttpPost httpPost = new HttpPost(url);
			httpPost.addHeader("Content-Type",
	                "application/x-www-form-urlencoded; text/html; charset=" + encoding);
	        httpPost.addHeader("User-Agent", "Mozilla/4.0");
			// 加入请求参数
			if(params != null ){
				List<NameValuePair> paramList = new ArrayList<NameValuePair>();
				for(String key : params.keySet()){
					if(key != null){
						paramList.add(new BasicNameValuePair(key, params.get(key)));
					}
				}
				UrlEncodedFormEntity entity = new UrlEncodedFormEntity(paramList,encoding);
				httpPost.setEntity(entity);
			}
			//httpResponse = httpClient.execute(httpPost);
			httpResponse = getHttpClient().execute(httpPost);
		}
		
		// 获取返回内容
		HttpEntity entity = httpResponse.getEntity();
		if(entity!=null){
			/*InputStream is = entity.getContent();
			int l ;
			byte[] buff = new byte[1024];
			while( (l = is.read(buff)) != -1){
				result += new String(buff, 0, l, encoding);
			}
			if(is != null){
				is.close();
			}*/
			BufferedReader br = new BufferedReader(new InputStreamReader(entity.getContent()));
			String s = "";
			while ((s = br.readLine()) != null) {  
				result += s;
			}
			if(br != null) {
				br.close();  
			}

		}
		return result;
	}
	

	/**
	 * 直接调用get方法，获取url返回内容
	 *
	 * @param url get请求的地址,带http完整格式
	 * @param encoding 网页编码
	 */
	public static String get(String url, String encoding){
		String result = "";
		try {
			result = HttpRequestUtils.request(url, null, HttpRequestUtils.REQUEST_TYPE_GET, encoding);
		} catch (Exception e) {
			result = e.getMessage();
		}
		return result;
	}

	/**
	 * 调用post请求url地址
	 *
	 * @param url 请求地址,带http完整格式
	 * @param params 请求参数
	 * @param encoding 网页编码
	 * @return
	 */
	public static String post(String url, Map<String, String> params, String encoding){
		String result = "";
		try {
			result = HttpRequestUtils.request(url, params, HttpRequestUtils.REQUEST_TYPE_POST, encoding);
		} catch (Exception e) {
			result = e.getMessage();
		}
		return result;
	}

	/**
	 * 模拟发出https请求，https://graph.qq.com/user/get_user_info
	 * sslRequest  
	 * @param    
	 * @return  
	 * @throws 
	 * @author zhougq  
	 * @date   2014年8月14日 下午1:40:31
	 */
	public static String sslRequest(String uri, Map<String,String> params, String type, String encoding){
		String res = "";
		DefaultHttpClient httpClient = new DefaultHttpClient();
		HttpResponse httpResponse = null;
		X509TrustManager xtm = new X509TrustManager() {
			
			public X509Certificate[] getAcceptedIssuers() {
				return null;
			}
			
			public void checkServerTrusted(X509Certificate[] chain, String authType)
				throws CertificateException {
				
			}
			
			public void checkClientTrusted(X509Certificate[] chain, String authType)
				throws CertificateException {
			}
		};
		
		try {
			SSLContext ctx = SSLContext.getInstance("TLS");
			ctx.init(null, new TrustManager[]{xtm}, null);
			SSLSocketFactory socketFactory = new SSLSocketFactory(ctx);
			
			httpClient.getConnectionManager().getSchemeRegistry().register(new Scheme("https", 443, socketFactory));
			
			// GET方式请求
			if(HttpRequestUtils.REQUEST_TYPE_GET.equals(type)){
				// 加入请求参数
				if(params != null ){
					if(uri.indexOf("?") != -1){
						uri += "&";
					}else{
						uri += "?";
					}
					for(String key : params.keySet()){
						uri += key +"="+params.get(key) +"&";
					}
				}
				HttpGet httpGet = new HttpGet(uri);
				httpResponse = getHttpClient().execute(httpGet);
				
			// POST方式请求
			}else if(HttpRequestUtils.REQUEST_TYPE_POST.equals(type)){
				HttpPost httpPost = new HttpPost(uri);
				// 加入请求参数
				if(params != null ){
					List<NameValuePair> paramList = new ArrayList<NameValuePair>();
					for(String key : params.keySet()){
						if(key != null){
							paramList.add(new BasicNameValuePair(key, params.get(key)));
						}
					}
					UrlEncodedFormEntity entity = new UrlEncodedFormEntity(paramList,encoding);
					httpPost.setEntity(entity);
				}
				httpResponse = getHttpClient().execute(httpPost);
			}
			
			// 获取返回内容
			HttpEntity entity = httpResponse.getEntity();
			if(entity!=null){
				InputStream is = entity.getContent();
				int l ;
				byte[] buff = new byte[9192];
				while( (l = is.read(buff)) != -1){
					res += new String(buff, 0, l, encoding);
				}
			}
			
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		} catch (KeyManagementException e) {
			e.printStackTrace();
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		} catch (IllegalStateException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
		return res;
	}

	public static String doPost(String url, Map param) throws Exception
	{
		String html = "";
		HttpPost httpost = new HttpPost(url); 
		
		try {
			// 添加参数  
            List<NameValuePair> nvps = new ArrayList<NameValuePair>();
            if(param != null)
    		{
    			Iterator<Map.Entry<String,String>> ie = param.entrySet().iterator();
    			while(ie.hasNext())
    			{
    				Map.Entry<String,String> en = ie.next();
    				nvps.add(new BasicNameValuePair(en.getKey(),  
                			en.getValue())); 
    			}
    		}
            httpost.setEntity(new UrlEncodedFormEntity(nvps, Consts.UTF_8));
            HttpResponse response = getHttpClient().execute(httpost);  
            StatusLine statusLine = response.getStatusLine();
            int statusCode = statusLine.getStatusCode();
            if(200 != statusCode) {
            	throw new Exception();
            }
            HttpEntity entity = response.getEntity();  
            if(entity != null)
            {
            	return handleEntity(entity);
            }
		} catch (Exception e) {
			throw e;
		}
		finally {
            if(httpost != null){
            	httpost.releaseConnection();
            }
        }
		return html;
	}
	private static String handleEntity(HttpEntity entity)
	{
		BufferedReader br = null;
		try {
			br = new BufferedReader(new InputStreamReader(
                    entity.getContent(), "utf-8"));  
            return br.readLine(); 
		} catch (Exception e) {
			return "publicServerError";
		}
		finally{
			try {
				if(br != null)
					br.close();
			} catch (Exception e2) {
			}
		}
	}
	public static void main(String args[])
	{
		Map<String, String> map = new HashMap<String, String>();
		map.put("busyFlag", "Z");
		System.out.println(HttpRequestUtils.doGet("http://172.18.0.62:8080/publicservice/getCacheObject.action?mainKey=web_ctw_top_t012&ruleValue=1♀holiday_sub_20150818161348♀3♀1♀1363&isMulti=Y&userId=", map));
//		System.out.println(HttpClientManager.doPost("http://172.18.0.19:8088/foreignservice/cityList.action", map));
	}
}
