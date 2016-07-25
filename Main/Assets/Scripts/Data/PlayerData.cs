using UnityEngine;
using System.Collections;
public class PlayerData{
	public string name;
	public int id;
	//public string path;
	//public int type;
	public int hp;
	public int maxHP;
	public PlayerData(string inName,int inId,int inHp,int inMaxHP){
		name=inName;
		id=inId;
		hp=inHp;
		maxHP=inMaxHP;
	}
}

