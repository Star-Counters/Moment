using UnityEngine;
using System.Collections;

public class MainCameraControl : MonoBehaviour {
	public float height;
	public float distance;
	Transform player;
	void Start(){
		player = GameObject.Find ("Player").transform;
	}
	void Update () {
		transform.position = new Vector3 (
			player.position.x-distance/Mathf.Sqrt(2), 
			player.position.y + height, 
			player.position.z - distance/Mathf.Sqrt(2));
		transform.rotation=Quaternion.Euler(new Vector3(Mathf.Atan2(height,distance)*180f/Mathf.PI,45,0));
	}
}
