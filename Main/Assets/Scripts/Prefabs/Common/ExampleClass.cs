using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
	public float target = 270.0F;
	public float speed = 45.0F;
	void Update() {
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, target, speed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}
}