using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
[RequireComponent(typeof(BehaviorTree))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class AIController : MonoBehaviour {
	Animator animator;
	BehaviorTree behaviorTree;
	public string eventName;
	public object eventArgs;
	Dictionary<string,System.Action> delegateDictionary;
	void Start(){
		animator = GetComponent<Animator> ();
		behaviorTree = GetComponent<BehaviorTree> ();
		delegateDictionary = new Dictionary<string, System.Action> ();
	}
	public void OnAttackBegin(){
		//		Debug.Log("FALSE");
		animator.SetBool("Attack",false);
		behaviorTree.SendEvent ("OnAttackBegin");
	}
	public void OnAttackEnd(){
		//Debug.Log ("AnimatorEventHandler.OnAttackEnd");
		behaviorTree.SendEvent ("OnAttackEnd");
		//DataManager.Instance.ModifyPlayerHP(-(int)(monsterData.attackPower));
	}
	public void RegisterEvent(string eventName,System.Action method){
		//eventTable[eventType] = (Callback) Delegate.Combine((Callback) eventTable[eventType], handler);
		delegateDictionary [eventName] = System.Action.Combine (delegateDictionary [eventName], method) as System.Action;
	}
	public void UnRegisterEvent(string eventName,System.Action method){
		if (delegateDictionary.ContainsKey(eventName))
		{
			delegateDictionary[eventName] = System.Delegate.Remove(delegateDictionary[eventName] as System.Action, method) as System.Action;
		}
		delegateDictionary [eventName] = System.Action.Combine (delegateDictionary [eventName], method) as System.Action;
	}
	public void SendEvent(string eventName){
		System.Action action;
		if (delegateDictionary.TryGetValue (eventName, out action)) {
			action();
		}
	}
}
