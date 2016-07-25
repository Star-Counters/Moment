using UnityEngine;
using System.Collections;
public abstract class BaseToogle:MonoBehaviour{
	protected int id;
	public Animator animator;
	protected virtual void Awake(){
		animator=GetComponent<Animator>();
	}
	protected virtual void Start(){
		InitData();
	}
	protected abstract void InitData ();
}