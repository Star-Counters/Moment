using UnityEngine;
using System.Collections;
public class MonsterAI:BaseAI
{
	public MonsterData monsterData;
	protected override void Start ()
	{
		base.Start ();
	}
	protected override void Awake ()
	{
		base.Awake ();
		DataManager.Instance.AddAIListItem (this);
	}
	protected override void InitData ()
	{
		monsterData=DataManager.Instance.GetMonsterData(id);
	}
	public void OnAttackBegin(){
//		Debug.Log("FALSE");
		animator.SetBool("Attack",false);
	}
	public void OnAttackEnd(){
		DataManager.Instance.ModifyPlayerHP(-(int)(monsterData.attackPower));
	}
	IEnumerator AttackDelay(){
		yield return new WaitForSeconds(monsterData.attackDelayTime);
		//fsm.CurrentStateID==StateID.AttackPlayer
	}
	public void OnHurt(int inHP){
		monsterData.hp-=inHP;
		Debug.Log(monsterData.hp);
		if(monsterData.hp<1){
			OnDie();
		}
	}
	public void OnDie(){
		fsm.PerformTransition (Transition.NoHealth);
	}
}