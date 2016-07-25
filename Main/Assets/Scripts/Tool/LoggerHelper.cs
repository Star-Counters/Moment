using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
public class LoggerHelper
{
	public static LogLevel CurrentLogLevels = (LogLevel.CRITICAL | LogLevel.EXCEPT | LogLevel.ERROR | LogLevel.WARNING | LogLevel.INFO | LogLevel.DEBUG);
	//public static LogLevel CurrentLogLevels = (LogLevel.CRITICAL | LogLevel.EXCEPT | LogLevel.ERROR | LogLevel.WARNING | LogLevel.INFO );
	public static string DebugFilterStr = string.Empty;
	private static ulong index = 0L;
	private static LogWriter m_logWriter = new LogWriter();
	private const bool SHOW_STACK = true;
	static Queue<string> postErrors;
	public static void Dispose()
	{
		if (postErrThread != null)
			postErrThread.Abort();
		postErrThread = null;
	}
	static Thread postErrThread;
	static LoggerHelper()
	{
		Application.RegisterLogCallback(new Application.LogCallback(LoggerHelper.ProcessExceptionReport));         
	}
	public static string ContextInfo="";
	public static void StartPostError()
	{
		postErrors = new Queue<string>();
		postErrThread = new Thread(new ThreadStart(PostError));
		postErrThread.Start();
	}
	private static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
	{
		string ret = string.Empty;
		try
		{
			byte[] byteArray = dataEncode.GetBytes(paramData); //转化
			HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
			webReq.Method = "POST";
			webReq.ContentType = "application/x-www-form-urlencoded";
			webReq.Timeout = 600000;
			webReq.ContentLength = byteArray.Length;
			Stream newStream = webReq.GetRequestStream();
			newStream.Write(byteArray, 0, byteArray.Length);//写入参数
			newStream.Close();
			HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
			StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
			ret = sr.ReadToEnd();
			sr.Close();
			response.Close();
			newStream.Close();
		}
		catch (Exception ex)
		{
			//Log("post error log error:"+ex.Message, LogLevel.CRITICAL, true);
		}
		return ret;
	}
	public static string reportUrl = "http://192.168.1.11:88/int_error_inside.php";
	static void PostError()
	{	 
		while(postErrors !=null)
		{
			string msg=null;
			lock (postErrors)
			{
				  if (postErrors.Count > 0)
				{
					msg = postErrors.Dequeue();                        
				}
			}
			if(msg == null)
				Thread.Sleep(50);
			else
				PostWebRequest(reportUrl, new StringBuilder().Append("msg=").Append(WWW.EscapeURL(ContextInfo + "\n")).Append(WWW.EscapeURL(msg)).Append("&tp=0&appVer=0&device=&userid=").ToString(), Encoding.UTF8);				
			}		   
	}
	public static void Critical(object message, bool isShowStack = true)
	{
		if (LogLevel.CRITICAL == (CurrentLogLevels & LogLevel.CRITICAL))
		{
			string msg = string.Concat(new object[] { " [CRITICAL]: ", message, '\n', isShowStack ? GetStacksInfo() : "" });
			Log(msg, LogLevel.CRITICAL, true);
			if (postErrors != null)
			{
				lock (postErrors)
				{
					postErrors.Enqueue(msg);
				}
			}
		}
	}

	public static void Debug(object message, bool isShowStack = true, int user = 0)
	{
		if (!(DebugFilterStr != "") && (LogLevel.DEBUG == (CurrentLogLevels & LogLevel.DEBUG)))
		{
			object[] objArray = new object[5];
			objArray[0] = " [DEBUG]: ";
			objArray[1] = isShowStack ? GetStackInfo() : "";
			objArray[2] = message;
			objArray[3] = " Index = ";
			index += (ulong) 1L;
			objArray[4] = index;
			Log(string.Concat(objArray), LogLevel.DEBUG, true);
		}
	}
	public static void Debug(string filter, object message, bool isShowStack = true)
	{
		if ((!(DebugFilterStr != "") || !(DebugFilterStr != filter)) && (LogLevel.DEBUG == (CurrentLogLevels & LogLevel.DEBUG)))
		{
			Log(" [DEBUG]: " + (isShowStack ? GetStackInfo() : "") + message, LogLevel.DEBUG, true);
		}
	}
	public static void Error(object message, bool isShowStack = true)
	{
		if (LogLevel.ERROR == (CurrentLogLevels & LogLevel.ERROR))
		{
			string msg = string.Concat(new object[] { " [ERROR]: ", message, '\n', isShowStack ? GetStacksInfo() : "" });
			Log(msg, LogLevel.ERROR, true);
			 if(postErrors != null)
			{
				lock(postErrors)
				{
					postErrors.Enqueue(msg);
				}
			}
		}	   
	}
	public static void Except(Exception ex, object message  )
	{
		if (LogLevel.EXCEPT == (CurrentLogLevels & LogLevel.EXCEPT))
		{
			Exception innerException = ex;
			while (innerException.InnerException != null)
			{
				innerException = innerException.InnerException;
			}
			Log(" [EXCEPT]: " + ((message == null) ? "" : (message + "\n")) + ex.Message + innerException.StackTrace, LogLevel.CRITICAL, true);
		}
	}
	private static string GetStackInfo()
	{
		StackTrace trace = new StackTrace();
		MethodBase method = trace.GetFrame(2).GetMethod();
		return string.Format("{0}.{1}(): ", method.ReflectedType.Name, method.Name);
	}
	private static string GetStacksInfo()
	{
		StringBuilder builder = new StringBuilder();
		StackFrame[] frames = new StackTrace().GetFrames();
		for (int i = 2; i < frames.Length; i++)
		{
			builder.AppendLine(frames[i].ToString());
		}
		return builder.ToString();
	}
	public static void Info(object message, bool isShowStack = true)
	{
		if (LogLevel.INFO == (CurrentLogLevels & LogLevel.INFO))
		{
			Log(" [INFO]: " + (isShowStack ? GetStackInfo() : "") + message, LogLevel.INFO, true);
		}
	}
	public static Action<string, LogLevel> OnLog;
	private static void Log(string message, LogLevel level, bool writeEditorLog = true)
	{
		string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + message;
		m_logWriter.WriteLog(msg, level, writeEditorLog);
		if (OnLog != null)
			OnLog(msg, level);
		//UnityEngine.Debug.Log(message);
		//Messenger.Broadcast<string>(CommonEvent.SHOW_STATUS, message);
	
	}
	public static string GetInfo()
	{
		return m_logWriter.LogPath + "," + m_logWriter.error;
	}
	private static void ProcessExceptionReport(string message, string stackTrace, LogType type)
	{
		LogLevel dEBUG = LogLevel.DEBUG;
		switch (type)
		{
		case LogType.Error:
			dEBUG = LogLevel.ERROR;
			break;
		case LogType.Assert:
			dEBUG = LogLevel.DEBUG;
			break;
		case LogType.Warning:
			dEBUG = LogLevel.WARNING;
			break;
		case LogType.Log:
			dEBUG = LogLevel.DEBUG;
			return;
			break;
		case LogType.Exception:
			dEBUG = LogLevel.EXCEPT;
			break;
		}
		if (dEBUG == (CurrentLogLevels & dEBUG))
		{
			string msg = string.Concat(new object[] { " [SYS_", dEBUG, "]: ", message, '\n', stackTrace });
			Log(msg, dEBUG, false);
			if(postErrors != null && dEBUG == LogLevel.ERROR || dEBUG == LogLevel.EXCEPT)
			{
				lock (postErrors)
				{
					postErrors.Enqueue(msg);
				}
					 
			}
		}
	}
	public static void Release()
	{
		m_logWriter.Release();
	}
	public static void UploadLogFile()
	{
		m_logWriter.UploadTodayLog();
	}
	public static void Warning(object message, bool isShowStack = true)
	{
		if (LogLevel.WARNING == (CurrentLogLevels & LogLevel.WARNING))
		{
			Log(" [WARNING]: " + (isShowStack ? GetStackInfo() : "") + message, LogLevel.WARNING, true);
		}
	}
	public static LogWriter LogWriter
	{
		get
		{
			return m_logWriter;
		}
	}
	static Queue<string> postReportData;
	static Thread postReportDataThread;
	public static string reportDataUrl = "http://s1370.app100715380.qqopenapp.com:8010/report3.php";
	public static void StartPostReportData()
	{
		postReportData = new Queue<string>();
		postReportDataThread = new Thread(new ThreadStart(PostReportData));
		postReportDataThread.Start();
	}
	static void PostReportData()
	{
		while (postReportData != null)
		{
			string msg = null;
			lock (postReportData)
			{
				if (postReportData.Count > 0)
				{
					msg = postReportData.Dequeue();
				}
			}
			if (msg == null)
				Thread.Sleep(50);
			else
				PostWebRequest(reportDataUrl, msg, Encoding.UTF8);
		}
	}
	public static void reportData(string message)
	{
		if (postReportData != null)
		{
			lock (postReportData)
			{
				postReportData.Enqueue(message.ToString());
			}
		}
	}
}

