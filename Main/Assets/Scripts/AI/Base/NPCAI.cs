using UnityEngine;
using System.Collections;
public class NPCAI:BaseAI
{
	[HideInInspector]
	protected GameObject talkBubble;
    public string[] chatContents;
	public NPCData npcData;
	protected override void Start ()
	{
		base.Start ();
		talkBubble = Instantiate (Resources.Load ("UI/Items/TalkBubble")) as GameObject;
		PlayTalkBubble ();
		StartCoroutine(TalkBubbleFollowNPC());
	}
	protected override void Awake ()
	{
		base.Awake ();
	}
	protected override void InitData ()
	{
		npcData = DataManager.Instance.GetNPCData (id);
	}
	IEnumerator TalkBubbleFollowNPC(){
		while(true){
			Vector3 npcScreenPoint=Camera.main.WorldToScreenPoint(transform.position+Vector3.up*1.5f);
			talkBubble.transform.localPosition=new Vector2(npcScreenPoint.x-Screen.width/2f,npcScreenPoint.y-Screen.height/2f);
			yield return null;
		}
	}
	public void PlayTalkBubble(){
		if(talkBubble!=null){
			talkBubble.GetComponent<TweenScale>().enabled=true;
		}
	}
	public void StopTalkBubble(){
		if(talkBubble!=null){
			talkBubble.GetComponent<TweenScale>().enabled=false;
		}
	}
}