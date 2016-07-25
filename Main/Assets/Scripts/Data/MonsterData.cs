public class MonsterData{
	public string name;
	public int id;//abcd  a:scene level,b:npc type,cd:npc id
	/// <summary>
	/// The power of attack.
	/// </summary>
	public int attackPower;
	public int hp;
	public float speed;
	public float attackDelayTime;
	public MonsterData(){

	}
	public MonsterData(string inName,int inId,int inPower,int inHp,float inSpeed,float inAttackDelayTime){
		this.name = inName;
		this.id = inId;
		this.attackPower = inPower;
		this.hp = inHp;
		this.speed = inSpeed;
		this.attackDelayTime=inAttackDelayTime;
	}
}

