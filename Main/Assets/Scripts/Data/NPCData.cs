using UnityEngine;
using System.Collections;
public class NPCData{
	public string name;
	public int id;//abcd  a:scene level,bc:npc id
	public string path;
	public int type;
	public NPCData(string inName,int inId,string inPath,int inType){
		name=inName;
		id=inId;
		path=inPath;
		type=inType;
	}
}

