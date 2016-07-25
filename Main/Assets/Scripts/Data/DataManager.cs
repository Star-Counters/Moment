using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public sealed class DataManager:GameData{
	public static DataManager Instance{
		get{
			if(instance==null)
				instance=new DataManager();
			return instance;
		}
	}
	private static DataManager instance;
	public DataManager(){
		LoadUserData ();
		LoadMonsterData ();
		LoadNPCData();
		LoadBossData ();
		Messenger.AddListener<bool> (CombatEventType.Pause, PauseAllAI);
		Messenger.AddListener(CombatEventType.GameOver, OnGameOver);
	}
	#region private member parameters
	private List<MonsterAI> allAIList=new List<MonsterAI>();
	private Dictionary<int,MonsterData> monsterDataDict=new Dictionary<int, MonsterData>();
	private Dictionary<int,BossData> bossDataDict=new Dictionary<int,BossData>();
	private Dictionary<int,NPCData> npcDataDict=new Dictionary<int,NPCData>();
	private ArchiveData archiveData;
	//private List<MonsterAI> inSightMonsterList=new List<MonsterAI>();
	//private List<GameObject> inSightNPCList=new List<GameObject>();
//	private Dictionary<int,ItemData> itemDataDict=new Dictionary<int, ItemData>();
	PlayerData playerData;
	#endregion
	public void AddAIListItem(MonsterAI ai){
		allAIList.Add (ai);
	}
	public void RemoveAIListItem(MonsterAI ai){
		allAIList.Remove (ai);
	}
	public List<MonsterAI> GetAllAIList(){
		return allAIList;
	}
	/*public List<BaseAI> GetAllAIList(){
		return allAIList;
	}*/
	public void PauseAllAI(bool isPause){//,BaseAI aiToIgnore){
		if(isPause){
			//allAIList.Remove(aiToIgnore);
			foreach(BaseAI ai in allAIList){
				ai.SetTransition(Transition.WaitForOrder);
			}
		}
		else{
			foreach(BaseAI ai in allAIList){
				ai.SetTransition(Transition.WaitForOrder);
			}
			//allAIList.Add(aiToIgnore);
		}
	}
	public void OnGameOver(){
		Application.LoadLevel (Application.loadedLevel);
	}
	/*public void AddInSightNPC(GameObject go){
		inSightNPCList.Add(go);
	}
	public void RemoveInSightNPC(GameObject go){
		if(inSightNPCList.Contains(go)){
			inSightNPCList.Remove(go);
		}
		else{
			Debug.LogWarning(go.name+",the npc you want to remove is not in the list");
		}
	}
	public List<GameObject> GetAllInSightNPC(){
		return inSightNPCList;
	}*/
	/*
	public void AddInSightMonster(MonsterAI go){
		if(!inSightMonsterList.Contains(go)){
			Debug.Log("add,"+go.name);
			inSightMonsterList.Add(go);
		}
	}
	public void RemoveInSightMonster(MonsterAI go){
		if(inSightMonsterList.Contains(go)){
			Debug.Log("Remove"+go.name);
			inSightMonsterList.Remove(go);
			allAIList.Remove(go);
		}
		else{
			Debug.LogWarning(go.name+",the monster you want to remove is not in the list");
		}
	}

	public List<MonsterAI> GetAllInSightMonster(){
		return inSightMonsterList;
	}
	*/
	public MonsterData GetMonsterData(int id){
		if(monsterDataDict.ContainsKey(id)){
			return monsterDataDict[id];
		}
		else{
			Debug.LogError("The monster data of id "+id.ToString()+" is not valid");
			return null;
		}
	}
	private void LoadMonsterData(){
		monsterDataDict.Add(101,new MonsterData ("Beatle", 101, 1, 2, 1f,2f ));
	}
	public NPCData GetNPCData(int id){
		if(npcDataDict.ContainsKey(id)){
			return npcDataDict[id];
		}
		else{
			Debug.LogError("The NPC data of id "+id.ToString()+" is not valid");
			return null;
		}
	}
	private void LoadNPCData(){
		npcDataDict.Add(101,new NPCData ("Villager1", 101, "Villager", 1));
		npcDataDict.Add(701,new NPCData ("GuidePost",701,"GuidePost",1));
		npcDataDict.Add(801,new NPCData ("TreasureChest",801,"TreasureChest",1));
	}
	public PlayerData GetPlayerData(){
		return playerData;
	}
	private void LoadUserData(){
		playerData=new PlayerData("Melody",1001,3,8);
	}
	private void LoadPlayerData(){

	}
	private void LoadBossData(){

	}
	public void GetBossData(int id){

	}
	private void LoadSceneData(int sceneLevel){

	}
	/*
	public void LoadItemData(){
		itemDataDict.Add(701,new ItemData ("GuidePost",701,"GuidePost",1));
	}
	public ItemData GetItemData(int id){
		if(itemDataDict.ContainsKey(id)){
			return itemDataDict[id];
		}
		else{
			Debug.LogError("The Item Data of id "+id.ToString()+" is not valid");
			return null;
		}
	}*/
	#region events
	public void ModifyPlayerHP(int n){
		int temp=playerData.hp;
		temp=Mathf.Clamp(temp+n,0,playerData.maxHP);
		n=temp-playerData.hp;
		if(n==0)
			return;
		else{
			playerData.hp=temp;
			Messenger.Broadcast<int>(CombatEventType.ModifyPlayerHP,n);
		}
	}
	#endregion
}