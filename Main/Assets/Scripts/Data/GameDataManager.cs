//=========================================================================================================
//Note: Data Managing.
//Date Created: 2012/04/17 by 风宇冲
//Date Modified: 2012/12/14 by 风宇冲
//=========================================================================================================
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
//GameData,储存数据的类，把需要储存的数据定义在GameData之内就行//
public class GameData
{
	public string key;
	public GameData(){
		//设定密钥，根据具体平台设定//
		key = SystemInfo.deviceUniqueIdentifier;
	}
}
//管理数据储存的类//
public class GameDataManager:MonoBehaviour
{
	private static XmlSaver xs = new XmlSaver();
	//存档时调用的函数//
	public static void Save<T>(T t) where T:GameData
	{
		string dataFileName = typeof(T).Name + ".dat";
		string gameDataFile = GetDataPath() + "/"+dataFileName;
		string dataString= xs.SerializeObject(t,typeof(T));
		xs.CreateXML(gameDataFile,dataString);
	}
	public static void Fuck<T>(T t){
		Debug.Log (t.GetType ().Name);
	}
	//读档时调用的函数//
	public static T Load<T>() where T:GameData
	{
		string dataFileName = typeof(T).Name + ".dat";
		string gameDataFile = GetDataPath() + "/"+dataFileName;
		if(xs.hasFile(gameDataFile))
		{
			string dataString = xs.LoadXML(gameDataFile);
			T gameDataFromXML = xs.DeserializeObject(dataString,typeof(T)) as T;
			
			//是合法存档//
			if(gameDataFromXML.key == SystemInfo.deviceUniqueIdentifier)
			{
				return gameDataFromXML;
			}
			return null;
		}
		return null;
	}
	
	//获取路径//
	private static string GetDataPath()
	{
		// Your game has read+write access to /var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/Documents
		// Application.dataPath returns ar/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data             
		// Strip "/Data" from path
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string path = Application.dataPath.Substring (0, Application.dataPath.Length - 5);
			// Strip application name
			path = path.Substring(0, path.LastIndexOf('/')); 
			return path + "/Documents";
		}
		else
			//    return Application.dataPath + "/Resources";
			return Application.streamingAssetsPath;
	}
}