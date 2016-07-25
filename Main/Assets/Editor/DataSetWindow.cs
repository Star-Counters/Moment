using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
public class DataSetWindow : EditorWindow {
	public string dataName="";
	public int newSelected = 0;
	string[] dataNames=new string[]{"ArchiveData","MonsterData","BossData"};
	Dictionary<Type,object> dataSetDict=new Dictionary<Type, object>();
	List<Type> typeList=new List<Type>();
	List<string> nameList=new List<string>();
	List<object> valueList=new List<object>();
	int listIndex=0;
	int groupIndex=0;
	const bool guiHide=false;
	[MenuItem ("CommonTool/DataSetWindow")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		DataSetWindow window = (DataSetWindow)GetWindow (typeof (DataSetWindow));
	}
	void OnGUI () {
		if(guiHide)
			return;
		GUILayout.Label(dataName, EditorStyles.boldLabel);
		newSelected = EditorGUILayout.Popup (newSelected, dataNames);
		if (GUI.changed && newSelected >= 0)
		{
			dataName=dataNames[newSelected];
			Assembly asmb = Assembly.Load("Assembly-CSharp") ;
			Type dataType=asmb.GetType(dataName);			
			Type dataManagerType = asmb.GetType("GameDataManager");
			MethodInfo loadMethod = dataManagerType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
			loadMethod=loadMethod.MakeGenericMethod(dataType);
			object data=loadMethod.Invoke(dataManagerType,null);
			FieldInfo[] fields=dataType.GetFields();
			dataSetDict=new Dictionary<Type, object>();
			listIndex=0;
			typeList.Clear();
			nameList.Clear();
			foreach(FieldInfo fieldInfo in fields){
				if(fieldInfo.Name!="key"){
					if(data==null){
						typeList.Add(fieldInfo.FieldType);
						nameList.Add(fieldInfo.Name);
						//valueList.Add(newfieldInfo.FieldType());
						listIndex++;
					}
					else{
						Debug.Log(fieldInfo.Name+","+fieldInfo.FieldType+","+fieldInfo.GetValue(data));
						typeList.Add(fieldInfo.FieldType);
						nameList.Add(fieldInfo.Name);
						valueList.Add(fieldInfo.GetValue(data));
						listIndex++;
					}
					//dataSetDict.Add(fieldInfo.FieldType,fieldInfo.GetValue(data));
				}
			}
		}
		else
		{
		}//index Type object name

		if(listIndex>0){
			for(int i=0;i<listIndex;i++) {
				ShowValueList(i);
			}
		}
	}
	void ShowValueList(int i){
		if (valueList.Count == 0)
			return;
		GUILayout.BeginHorizontal ();
		if(typeList[i]==typeof(string)){
				valueList[i]=EditorGUILayout.TextField(nameList[i],valueList[i].ToString());
			}
			else if(typeList[i] == typeof(int)){
				valueList[i]=EditorGUILayout.IntField(nameList[i],(int)valueList[i]);
			}
			else if(typeList[i]==typeof(float)){
				valueList[i]=EditorGUILayout.FloatField(nameList[i],(float)valueList[i]);
			}
			else if(typeList[i]==typeof(Array)){
				
			}
			GUILayout.EndHorizontal ();		
	}
}
