using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;

	public float followSpeed = 0.01f;

	Vector3 velocity;

	void Start () 
	{
		if (target == null)
			target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	void Update () 
	{
		//transform.position = Vector3.Lerp (transform.position, target.position, followSpeed);
		transform.position = Vector3.SmoothDamp (transform.position, target.position, ref velocity, followSpeed);
	}
}
