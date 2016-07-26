using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatPanel : MonoBehaviour {
	public static CombatPanel Instance{ get { return instance; } }
	private static CombatPanel instance;
	Transform[] hearts;
	int tempHP;
	Transform navigator;
	GameObject navigatorGroup;
	//public List<Action> mainButtonDownList;
	public Action MainButtonDown;
	public Action AssistButtonDown;
	public Action MainButtonPress;
	[HideInInspector]
	public float mainButtonReleaseTime=0f;
	[HideInInspector]
	public float assistButtonReleaseTime=0f;
	void Awake(){
		instance = this;
		navigatorGroup=transform.FindChild("NavigatorGroup").gameObject;
	}
	public void OnSetActive(bool isActive){
		navigatorGroup.SetActive(isActive);
		if(!isActive)
			return;
		Vector3 temp = new Vector3 ();
#if UNITY_EDITOR
		if(FingerInputHandler.Instance&&FingerInputHandler.Instance.useKeyBoard)
			temp = transform.parent.GetComponent<Camera>().ScreenToWorldPoint (new Vector3(60f,60f,2f));
		else
#endif
			temp = UICamera.currentCamera.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f));
		navigatorGroup.transform.position = new Vector3 (temp.x, temp.y,transform.position.z);
	}
	void Start () {
		navigator = transform.GetChild (0).GetChild (0).GetChild (0);
		navigatorGroup.SetActive (false);
		FingerInputHandler.Instance.MainSceneDrag += OnDrag;
		Messenger.AddListener<bool> (CombatEventType.Pause, OnPause);
		UIEventListener.Get(CombatPanel.Instance.transform.FindChild("Main").gameObject).onPress+=OnPressMainButton;
		UIEventListener.Get(CombatPanel.Instance.transform.FindChild("Assist").gameObject).onPress+=OnPressAssistButton;
		InitPlayerHP((int)DataManager.Instance.GetPlayerData().hp);
		Messenger.AddListener<int>(CombatEventType.ModifyPlayerHP,OnModifyPlayerHP);
		Transform heartsParent=transform.FindChild("Hearts");
		hearts=new Transform[8]{
			heartsParent.GetChild(0),heartsParent.GetChild(1),heartsParent.GetChild(2),heartsParent.GetChild(3),
			heartsParent.GetChild(4),heartsParent.GetChild(5),heartsParent.GetChild(6),heartsParent.GetChild(7)
		};
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(MainButtonDown!=null)
				OnPressMainButton(gameObject,true);
		}
	}
	void OnDestroy(){
		FingerInputHandler.Instance.MainSceneDrag -= OnDrag;
		Messenger.RemoveListener<int>(CombatEventType.ModifyPlayerHP,OnModifyPlayerHP);
	}
	//void OnGUI(){
	//	GUI.Label (new Rect (300, 0, 200, 50), navigator.rotation.ToString());
	//}
	void OnDrag(float x,float y){
		navigator.rotation = Quaternion.Euler(new Vector3(0, 0,45 - Mathf.Atan2 (x, y)*180.0f/Mathf.PI));
	}
	public void InitHeart(){

	}
	void OnPressAssistButton(GameObject button,bool state){
		if(state){
			if(AssistButtonDown!=null)
				AssistButtonDown();
		}
		else{
			
		}
		Debug.Log("Click"+button.name);
	}
	void OnPressMainButton(GameObject button,bool state){
		if(state){
			if(MainButtonDown!=null)
				MainButtonDown();
			StartCoroutine(AddReleaseTime());
		}
		else{
			if(mainButtonReleaseTime>1.0f){
				Debug.LogWarning(mainButtonReleaseTime);
				Debug.Log("RELEASE!");
			}
			StopAllCoroutines();
			StopCoroutine("AddReleaseTime");
			mainButtonReleaseTime=0f;
		}
	}
	IEnumerator AddReleaseTime(){
		while(true){
			//Debug.LogWarning(mainButtonReleaseTime);
			mainButtonReleaseTime+=Time.deltaTime;
			yield return null;
		}
	}
	void OnPause(bool isPause){
		/*if(isPause)
			FingerInputHandler.Instance.MainSceneDrag -= OnDrag;
		else
			FingerInputHandler.Instance.MainSceneDrag += OnDrag;
			*/
	}
	public void InitPlayerHP(int hp){
		tempHP=hp;
		Transform hearts=transform.FindChild("Hearts");
		for(int i=0;i<hearts.childCount;i++){
			hearts.GetChild(i).gameObject.SetActive(i<hp);
		}
	}
	public void OnModifyPlayerHP(int n){
//		Debug.Log(n);
		if(n<0){
			for(int i=0;i<-n;i++){
				hearts[tempHP-1-i].gameObject.SetActive(false);
				tempHP-=1;
				if(tempHP==0){
					Messenger.Broadcast(CombatEventType.GameOver);
				}
			}
		}
		else if(n>0){
			for(int i=0;i<n;i++){
				hearts[tempHP].gameObject.SetActive(true);
				tempHP+=1;
			}
		}
	}
}
