using UnityEngine;
using System.Collections;

public class LoginPanel : MonoBehaviour {
	public static LoginPanel Instance{ get { return instance; } }
	private static LoginPanel instance;
	void Awake () {
		instance = this;	
	}
	void Start(){
		EventDelegate.Add (transform.FindChild ("NewGame").GetComponent<UIEventTrigger> ().onClick, OnNewGame);
		EventDelegate.Add (transform.FindChild ("Continue").GetComponent<UIEventTrigger> ().onClick,OnContinue);
	}
	void OnNewGame(){
		//GameDataManager.Instance.gameData=new GameData();
		ArchiveData data = new ArchiveData ("", 1f);
		GameDataManager.Save<ArchiveData>(data);
	}
	void OnContinue(){
		ArchiveData data = GameDataManager.Load<ArchiveData> ();;
		//GameData data=GameDataManager.Instance.gameData;
		Debug.Log ("PlayerName:" + data.playerName + ",MusicVolum:" + data.musicVolume + ",UDID:" + data.key);//+",Monsters:"+data.monsterDatas[0].id+",Monsters:"+data.monsterDatas[1].id);
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			ArchiveData data = new ArchiveData ("Melody", 0.2f);
			GameDataManager.Save<ArchiveData>(data);
		}
	}
}
