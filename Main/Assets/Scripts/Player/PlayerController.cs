using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum GroundType{
	None=0,
	Road=1,
	Slope=2,
}
public class PlayerController : MonoBehaviour {
	public static PlayerController Instance{
		get{
			return instance;
		}
	}
	public Transform currentNPC;
	private Animator animator;
	//RandomAction randomAction;
	int attackIndex;
	public float radiusToSlope;
	public GroundType groundType;
	private static PlayerController instance;
	CharacterController controller;
	private float gravityScale=9.80f;
	private float verticalSpeed=0f;
	public PlayerState currentPlayerState;
	public PlayerState previousPlayerState;
	//delegate void UpdatePlayerStateEventHandler();
	Action UpdatePlayerState;
	AnimatorStateInfo stateInfo;
	//bool c
	void Awake(){
		instance = this;
		animator=GetComponent<Animator>();
		attackIndex = 1;
		//randomAction=new RandomAction(2);//Attack num.
	}
	void Start () {
		ChangeState (PlayerState.Fall);
		//UpdatePlayerState+=UpdateFall;
		controller = GetComponent<CharacterController> ();
		FingerInputHandler.Instance.MainSceneDrag += OnDrag;
		Messenger.AddListener<bool> (CombatEventType.Pause, OnPause);
		Messenger.AddListener<int>(CombatEventType.ModifyPlayerHP,OnModifyPlayerHP);
		//CombatPanel.Instance.MainButtonDown+=OnMainButtonDown;
	}
	public void ChangeState(PlayerState inState){
		if(inState==currentPlayerState)
			return;
//		Debug.Log("PlayerState from "+currentPlayerState+" change to "+inState+".");
		previousPlayerState = currentPlayerState;
		currentPlayerState = inState;
		//UpdatePlayerState=null;
		switch (currentPlayerState) {
		case PlayerState.Idle:
			//verticalSpeed = 0.0f;
			animator.SetInteger("Speed",0);
			UpdatePlayerState=UpdateIdle;break;
		case PlayerState.Run:
			//verticalSpeed = 0.0f;
			animator.SetInteger("Speed",2);
			UpdatePlayerState=UpdateRun;break;
		case PlayerState.Fall:
			//animator.SetBool("Fall",true);
			animator.SetInteger("Speed",-1);
			verticalSpeed = 0.0f;
			UpdatePlayerState=UpdateFall;break;
		case PlayerState.Dead:
			UpdatePlayerState=UpdateDead;break;
		case PlayerState.Slope:
			animator.SetInteger("Speed",1);
			UpdatePlayerState=UpdateSlope;break;
		case PlayerState.Attack:
			UpdatePlayerState=UpdateAttack;break;
		case PlayerState.Talk:
			//CombatPanel.Instance.MainButtonDown-=OnMainButtonDown;
			UpdatePlayerState=UpdateTalk;break;
		case PlayerState.Charge:
			UpdatePlayerState=UpdateCharge;break;
		default:
			break;
		}
	}
	void UpdateTalk(){
		//if(CombatPanel
	}
	void UpdateAttack(){

	}
	void UpdateCharge(){
		//animator.runtimeAnimatorController=
	}
	public void OnAttackEnd(){
		List<MonsterAI> monsters=DataManager.Instance.GetAllAIList();
		int i=0;
		while(i<monsters.Count){
//			Debug.Log(monsters[i].name);
			if(monsters[i].CheckPlayerFace()){
				monsters[i].OnHurt(1);
			}
			i++;
		}
		switch(animator.GetInteger("Speed")){
		case 0:
			ChangeState(PlayerState.Idle);break;
		case 1:
			ChangeState(PlayerState.Slope);break;
		case 2:
			ChangeState(PlayerState.Run);break;
		default:
			break;
		}
	}
	public void OnAttackBegin(){
		animator.SetBool ("Attack", false);
	}
	void UpdateSlope(){
		if(stateInfo.IsTag("General")){
			Vector3 horizontalDirection=transform.forward*Time.deltaTime*1.5f/2f;
			controller.Move (new Vector3(horizontalDirection.x,Time.deltaTime*-1.5f,horizontalDirection.z));
		}
		CheckGroundType ();
		if (groundType == GroundType.Road) {
			ChangeState(PlayerState.Run);		
		}
		else if(groundType==GroundType.None){
			CombatPanel.Instance.MainButtonDown-=OnMainButtonDown;
			ChangeState(PlayerState.Fall);
		}
	}
	void UpdateIdle(){
		//CheckGroundType();
	}
	void UpdateRun(){
		CheckGroundType ();
		if (groundType==GroundType.None) {
			CombatPanel.Instance.MainButtonDown-=OnMainButtonDown;
			ChangeState(PlayerState.Fall);
			return;
		}
		else if(groundType==GroundType.Slope){
			ChangeState(PlayerState.Slope);
			return;
		}
		if(stateInfo.IsTag("General")){
			Vector3 horizontalDirection=transform.forward*Time.deltaTime*1.5f;
			controller.Move (new Vector3(horizontalDirection.x,-1.5f*Time.deltaTime,horizontalDirection.z));
		}
	}
	void UpdateFall(){
		verticalSpeed -= gravityScale * Time.deltaTime;
		controller.Move(transform.forward*1.0f*Time.deltaTime+new Vector3(0,verticalSpeed*Time.deltaTime,0));
		CheckGroundType ();
		if (groundType==GroundType.Road) {
			if(FingerInputHandler.Instance.dragging){
				CombatPanel.Instance.MainButtonDown+=OnMainButtonDown;
				ChangeState(PlayerState.Run);
			}
			else{
				CombatPanel.Instance.MainButtonDown+=OnMainButtonDown;
				ChangeState(PlayerState.Idle);	
			}
		}
		else if(groundType==GroundType.Slope){
			if(FingerInputHandler.Instance.dragging){
				CombatPanel.Instance.MainButtonDown+=OnMainButtonDown;
				ChangeState(PlayerState.Slope);
			}
			else{
				CombatPanel.Instance.MainButtonDown+=OnMainButtonDown;
				ChangeState(PlayerState.Idle);	
			}
		}
	}
	void UpdateDead(){

	}
	/*
	void OnGUI(){
		GUILayout.Label("State:"+currentPlayerState);
		GUILayout.Label("GroundType:"+groundType);
		GUILayout.Label("Vertical:"+verticalSpeed);
	}
	*/
	public float cao=0f;
	//int lastUpdateFrame=0;
	void Update(){
		// already updated this frame, skip
		//if( lastUpdateFrame == Time.frameCount ){
			//Debug.LogWarning("The Same"+Time.frameCount);
			//return;
		//}
		//lastUpdateFrame = Time.frameCount;
		stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		UpdatePlayerState();
#if UNITY_EDITOR
		/*if(Input.GetKeyDown(KeyCode.T)){
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Sword-Attack1")||animator.GetNextAnimatorStateInfo(0).IsName("Sword-Attack1")){

			}
			else
				animator.Play("Sword-Attack1",0,cao);
		}
		*/
#endif
	}
	void OnDrag(float x,float y){
		transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2 (x, y)*180.0f/Mathf.PI,0));
	}
	private void CheckGroundType () {
		Ray ray = new Ray (transform.position, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if(CheckSlope()){
				//ChangeState(PlayerState.Slope);
				groundType=GroundType.Slope;
			}
			//Debug.Log(transform.position.y-hit.point.y);
			else if(transform.position.y-hit.point.y<0.05f){
				groundType=GroundType.Road;
			}
			//else if(CheckOther()){
				//groundType=GroundType.Road;
			//}
			else if(transform.position.y-hit.point.y>0.5f){
				groundType=GroundType.None;
			}
		}
		else
			groundType=GroundType.None;
	}
	bool CheckSlope(){
//		Debug.Log(transform.forward);
		Vector3[] origins = new Vector3[]{
			//transform.position+Vector3.up,
			transform.position+transform.forward*radiusToSlope+Vector3.up,//[x*cosA-y*sinA  x*sinA+y*cosA] 
			transform.position-transform.forward*radiusToSlope+Vector3.up,
			new Vector3(transform.forward.x*Mathf.Cos(45f)-transform.forward.z*Mathf.Sin(45f) ,0, transform.forward.x*Mathf.Sin(45f)+transform.forward.z*Mathf.Cos(45f))*radiusToSlope+Vector3.up*1f+transform.position,
			new Vector3(transform.forward.x*Mathf.Cos(-45f)-transform.forward.z*Mathf.Sin(-45f) ,0, transform.forward.x*Mathf.Sin(-45f)+transform.forward.z*Mathf.Cos(-45f))*radiusToSlope+Vector3.up*1f+transform.position,
			//transform.position-transform.forward*radiusToSlope+Vector3.up,
			//transform.rotation
			//transform.position+new Vector3(transform.forward.x,transform.forward.y,-transform.forward.z)*radiusToSlope+Vector3.up,
			//transform.position+new Vector3(-transform.forward.x,transform.forward.y,transform.forward.z)*radiusToSlope+Vector3.up,
			//transform.position+transform.TransformDirection(transform.forward)*radiusToSlope+Vector3.up,
			//transform.position+transform.forward*radiusToSlope+Vector3.up,
			//transform.position+transform.forward*radiusToSlope+Vector3.up,
			//new Vector3(transform.position.x+radiusToSlope,transform.position.y+1f,transform.position.z),
			//new Vector3(transform.position.x-radiusToSlope,transform.position.y+1f,transform.position.z),
			//new Vector3(transform.position.x,transform.position.y+1f,transform.position.z+radiusToSlope),
			//new Vector3(transform.position.x,transform.position.y+1f,transform.position.z-radiusToSlope)
		};
		for(int i=0;i<origins.Length;i++){
			Ray ray = new Ray (origins[i], 1.5f*Vector3.down);
			Debug.DrawLine(origins[i],origins[i]+Vector3.down,Color.green);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if((hit.transform.gameObject.layer)==12){
					return true;
				}
			}
		}
		return false;
	}
	bool CheckOther(){
		//		Debug.Log(transform.forward);
		Vector3[] origins = new Vector3[]{
			//transform.position+Vector3.up,
			transform.position+transform.forward*radiusToSlope+Vector3.up,//[x*cosA-y*sinA  x*sinA+y*cosA] 
			transform.position-transform.forward*radiusToSlope+Vector3.up,
			new Vector3(transform.forward.x*Mathf.Cos(45f)-transform.forward.z*Mathf.Sin(45f) ,0, transform.forward.x*Mathf.Sin(45f)+transform.forward.z*Mathf.Cos(45f))*radiusToSlope+Vector3.up*1f+transform.position,
			new Vector3(transform.forward.x*Mathf.Cos(-45f)-transform.forward.z*Mathf.Sin(-45f) ,0, transform.forward.x*Mathf.Sin(-45f)+transform.forward.z*Mathf.Cos(-45f))*radiusToSlope+Vector3.up*1f+transform.position,
			//transform.position-transform.forward*radiusToSlope+Vector3.up,
			//transform.rotation
			//transform.position+new Vector3(transform.forward.x,transform.forward.y,-transform.forward.z)*radiusToSlope+Vector3.up,
			//transform.position+new Vector3(-transform.forward.x,transform.forward.y,transform.forward.z)*radiusToSlope+Vector3.up,
			//transform.position+transform.TransformDirection(transform.forward)*radiusToSlope+Vector3.up,
			//transform.position+transform.forward*radiusToSlope+Vector3.up,
			//transform.position+transform.forward*radiusToSlope+Vector3.up,
			//new Vector3(transform.position.x+radiusToSlope,transform.position.y+1f,transform.position.z),
			//new Vector3(transform.position.x-radiusToSlope,transform.position.y+1f,transform.position.z),
			//new Vector3(transform.position.x,transform.position.y+1f,transform.position.z+radiusToSlope),
			//new Vector3(transform.position.x,transform.position.y+1f,transform.position.z-radiusToSlope)
		};
		for(int i=0;i<origins.Length;i++){
			Ray ray = new Ray (origins[i], 1.5f*Vector3.down);
			Debug.DrawLine(origins[i],origins[i]+Vector3.down,Color.green);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if(hit.transform.gameObject.layer==9){
					return true;
				}
				else if(hit.transform.gameObject.layer==10){
					return true;
				}
			}
		}
		return false;
	}
	public void OnMainButtonDown(){
		//Debug.Log (currentPlayerState.ToString());
		//Debug.Log("Notify!");
		ChangeState(PlayerState.Attack);
		attackIndex = attackIndex == 1 ? 2 : 1;
		animator.SetBool("Attack",true);
		//Debug.Log("Notify222!");
		//CombatPanel.Instance.MainButtonDown ();
	}
	void OnPause(bool isPause){
		Debug.Log(isPause);
		if(isPause){
			CombatPanel.Instance.MainButtonDown-=OnMainButtonDown;
			FingerInputHandler.Instance.MainSceneDrag -= OnDrag;

		}
		else{
			CombatPanel.Instance.MainButtonDown+=OnMainButtonDown;
			FingerInputHandler.Instance.MainSceneDrag += OnDrag;
		}
	}
	void OnModifyPlayerHP(int n){

	}
}
