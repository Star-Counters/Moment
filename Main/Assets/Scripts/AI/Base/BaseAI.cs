using UnityEngine;
[RequireComponent(typeof(Seeker))]
public abstract class BaseAI:MonoBehaviour
{
	protected int id;
	public Animator animator;
	protected virtual void Awake(){
		animator=GetComponent<Animator>();
		fsm = new FSMSystem();
		//Debug.Log ("BaseAI Awake");
	}
	protected virtual void Start(){
		//Debug.Log ("BaseAI Start");
		InitData();
	}
	/// <summary>
	/// The distance that AI will lost player,only when the AI's state is in chase state.It's also called the attack distance or face distance.
	/// </summary>
	public const float nearDistance=1.0f;
	/// <summary>
	/// The distance that AI will chase player,only when the AI's state is not in saw state
	/// </summary>
	public const float farDistance=4f;
	protected FSMSystem fsm;
	protected abstract void InitData ();
	public void SetTransition(Transition t) {
		fsm.PerformTransition(t); 
	}
	/// <summary>
	/// Checks if player is in the AI's sight range,then the AI can chase the player.
	/// </summary>
	/// <returns><c>true</c>, if player far was checked, <c>false</c> otherwise.</returns>
	public bool CheckPlayerFar(){
		if (Mathf.Abs (transform.position.y - PlayerController.Instance.transform.position.y) < 1.4f) {
			//float xDistance=Mathf.Pow((transform.position.z-PlayerController.Instance.transform.position.z),2f);
			//float zDistance=Mathf.Pow((transform.position.x-PlayerController.Instance.transform.position.x),2f);
			if(Mathf.Pow((transform.position.x-PlayerController.Instance.transform.position.x),2f)
			   +Mathf.Pow((transform.position.z-PlayerController.Instance.transform.position.z),2f)
			   <Mathf.Pow(farDistance,2f)){
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// Checks the player if AI is closed enough to the Player,then the AI can change to talk or attack the player.
	/// </summary>
	/// <returns><c>true</c>, if player near was checked, <c>false</c> otherwise.</returns>
	public bool CheckPlayerNear(bool checkFace=false){
		if (Mathf.Abs (transform.position.y - PlayerController.Instance.transform.position.y) < 0.9f) {
			Vector3 relativeDir=new Vector3(transform.position.x-PlayerController.Instance.transform.position.x,0,transform.position.z-PlayerController.Instance.transform.position.z);
			if(checkFace){
				if(relativeDir.magnitude<nearDistance){
					float myAngle=Mathf.Abs(Vector3.Angle(PlayerController.Instance.transform.forward,relativeDir));
					if(PlayerController.Instance.currentNPC==null){
						if(myAngle<45f){
							PlayerController.Instance.currentNPC=transform;
							return true;
						}
						else
							return false;
					}
					else if(PlayerController.Instance.currentNPC==transform){
						if(myAngle<45f)
							return true;
						else{
							PlayerController.Instance.currentNPC=null;
							return false;
						}
					}
					else {
						float currentNPCAngle=Mathf.Abs(Vector3.Angle(PlayerController.Instance.transform.forward,PlayerController.Instance.currentNPC.position-PlayerController.Instance.transform.position));
						if(currentNPCAngle>myAngle){
							PlayerController.Instance.currentNPC=transform;
							return true;
						}
						else
							return false;
					}
				}
				else
					return false;
			}
			else{
				if(relativeDir.magnitude<nearDistance){
					return true;
				}
				else
					return false;
			}
		}
		else 
			return false;
	}
	public bool CheckPlayerFace(){
		Vector3 relativeDir=new Vector3(transform.position.x-PlayerController.Instance.transform.position.x,0,transform.position.z-PlayerController.Instance.transform.position.z);
		if(relativeDir.magnitude<1f){
			float myAngle=Mathf.Abs(Vector3.Angle(PlayerController.Instance.transform.forward,relativeDir));
			if(myAngle<45f){
				return true;
			}
			else
				return false;
		}
		else
			return false;
	}
}