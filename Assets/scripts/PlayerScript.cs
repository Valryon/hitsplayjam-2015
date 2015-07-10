using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public float speed = 5f;
	public float shootForce = 1f;
	public float energy = 1f;

	private Vector2 movement;
	private Rigidbody rbody;

	void Awake()
	{
		rbody = GetComponent<Rigidbody> ();
	}

	void Start () 
	{
	
	}

	void Update () 
	{
		float x = Input.GetAxis ("Horizontal");
		float z = Input.GetAxis ("Vertical");

    Debug.Log (z);

		movement = new Vector3 (x, 0, z) * speed;
	}

	void FixedUpdate()
	{
		rbody.velocity = movement;
	}
}
