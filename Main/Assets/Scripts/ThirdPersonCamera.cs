using UnityEngine;
[RequireComponent( typeof( Camera ) )]
public class ThirdPersonCamera : MonoBehaviour
{
	// Attribute ------------------------------------------------------------------------
	Transform	follow;
	public		float		m_fDistance			= 7.0f;
	public		float		m_fXSpeed			= 350.0f;
	public		float		m_fYMinLimit		= 0.1f;
	public		float		m_fYMaxLimit		= 89.9f;
	private		float		m_fXRot				= 0.0f;
	public		float		m_fYRot				= 0.0f;
	public float speed=5.0f;
	void Start(){
		follow=GameObject.Find("Player").transform;
	}
	void LateUpdate(){
		if (Input.GetMouseButton(0))
		{
			m_fXRot += Input.GetAxis("Mouse X") * m_fXSpeed;
			m_fYRot -= Input.GetAxis("Mouse Y") * m_fXSpeed;
		}
		m_fYRot = Mathf.Clamp(m_fYRot,m_fYMinLimit, m_fYMaxLimit);
		Vector3 negDistance= new Vector3(0.0f, 0.0f, -m_fDistance);
		transform.rotation = Quaternion.Euler(m_fYRot, m_fXRot, 0);
		transform.position = transform.rotation * negDistance + follow.position;
	}
}