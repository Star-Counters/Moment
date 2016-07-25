using UnityEngine;
using System;
using System.Collections;

public class FingerInputHandler : MonoBehaviour {
	public static FingerInputHandler Instance{ get { return instance; } }
	private static FingerInputHandler instance;
	public delegate void MainSceneDragEventHandler(float x,float y);
	public event MainSceneDragEventHandler MainSceneDrag;
	Vector2 fingerDownPos;
	Vector2 dragDirection;
	public bool dragging=false;
#if UNITY_EDITOR
	public bool useKeyBoard=true;
	float outX=0;
	float outY=0;
	void Awake(){
		instance = this;
	}
	void Start(){
		Messenger.AddListener<bool> (CombatEventType.Pause, OnPause);
	}
	bool isAxisDown{
		get{
			return  Input.GetKeyDown(KeyCode.A)||
					Input.GetKeyDown(KeyCode.S)||
					Input.GetKeyDown(KeyCode.D)||
					Input.GetKeyDown(KeyCode.W)||
					Input.GetKeyDown(KeyCode.UpArrow)||
					Input.GetKeyDown(KeyCode.DownArrow)||
					Input.GetKeyDown(KeyCode.RightArrow)||
					Input.GetKeyDown(KeyCode.LeftArrow);
		}
	}
	bool isAxisUp{
		get{
			return  Input.GetKeyUp(KeyCode.A)||
					Input.GetKeyUp(KeyCode.S)||
					Input.GetKeyUp(KeyCode.D)||
					Input.GetKeyUp(KeyCode.W)||
					Input.GetKeyUp(KeyCode.UpArrow)||
					Input.GetKeyUp(KeyCode.DownArrow)||
					Input.GetKeyUp(KeyCode.RightArrow)||
					Input.GetKeyUp(KeyCode.LeftArrow);
		}
	}
	bool isAxis{
		get{
			return  Input.GetKey(KeyCode.A)||
					Input.GetKey(KeyCode.S)||
					Input.GetKey(KeyCode.D)||
					Input.GetKey(KeyCode.W)||
					Input.GetKey(KeyCode.UpArrow)||
					Input.GetKey(KeyCode.DownArrow)||
					Input.GetKey(KeyCode.RightArrow)||
					Input.GetKey(KeyCode.LeftArrow);
		}
	}
	#endif
	void Update(){
#if UNITY_EDITOR
		if(useKeyBoard){
			if(isAxisDown){
				CombatPanel.Instance.gameObject.SetActive (true);
			}
			else if(isAxis){
				outX = Input.GetAxis ("Horizontal");
				outY = Input.GetAxis ("Vertical");
				MainSceneDrag(outX,outY);	
			}
			else if(isAxisUp){
				CombatPanel.Instance.gameObject.SetActive (false);
			}
		}
#endif
	}
	void OnDrag(DragGesture gesture){
		if(useKeyBoard)
			return;
		if(UICamera.isOverUI)
			return;
		if(gesture.Phase==ContinuousGesturePhase.Started){
			if(PlayerController.Instance.groundType==GroundType.Road||PlayerController.Instance.groundType==GroundType.Slope)
				PlayerController.Instance.ChangeState(PlayerState.Run);
		}
		else if( gesture.Phase == ContinuousGesturePhase.Updated ){
			dragging=true;
			dragDirection = gesture.Position - fingerDownPos;
			dragDirection.Normalize ();
			if(MainSceneDrag!=null)
				MainSceneDrag (dragDirection.x, dragDirection.y);
		}
		else if( gesture.Phase == ContinuousGesturePhase.Ended ){
			dragging=false;
			if(PlayerController.Instance.groundType==GroundType.Road||PlayerController.Instance.groundType==GroundType.Slope)
				PlayerController.Instance.ChangeState(PlayerState.Idle);
		}
	}
	void OnFingerDown( FingerDownEvent e )
	{	
		if(UICamera.isOverUI)
			return;
		fingerDownPos = e.Position;
		CombatPanel.Instance.OnSetActive (true);
	}
	void OnFingerUp( FingerUpEvent e ){
		CombatPanel.Instance.OnSetActive (false);
	}
	void OnPause(bool isPause){
		GetComponent<FingerDownDetector>().UseSendMessage=!isPause;
		GetComponent<DragRecognizer>().UseSendMessage=!isPause;
	}
}
