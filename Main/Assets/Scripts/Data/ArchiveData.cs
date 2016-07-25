using UnityEngine;
using System.Collections;

public class ArchiveData : GameData {
	public string playerName;
	public float musicVolume;
	public ArchiveData(){

	}
	public ArchiveData(string _playerName,float _volume){
		playerName = _playerName;
		musicVolume = _volume;
	}
}
