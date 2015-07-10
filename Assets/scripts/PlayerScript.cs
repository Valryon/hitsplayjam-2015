using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

  public int team = 1;
	public float speed = 5f;
	public float shootForce = 1f;
	public float energy = 1f;

	private Vector3 movement;
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
    movement = Vector3.zero;

    if (IsSelected) 
    {
      float x = 0f;
      float z = 0f;

      // Team 1: arrows
      if(team == GameScript.TEAM1)
      {
        x = Input.GetAxis ("Horizontal");
        if( x < -0.25f) x = -1f;
        else if( x > 0.25f) x = 1f;

        z = Input.GetAxis ("Vertical");
        if( z < -0.25f) z = -1f;
        else if( z > 0.25f) z = 1f;
      }
      // Team 2: ZQSD
      else if(team == GameScript.TEAM2)
      {
        if(Input.GetKey(KeyCode.Q))
        {
          x = -1f; 
        }
        else if(Input.GetKey(KeyCode.D))
        {
          x = 1f;
        }
        if(Input.GetKey(KeyCode.Z))
        {
          z = 1f; 
        }
        else if(Input.GetKey(KeyCode.S))
        {
          z = -1f;
        }
      }

      movement = new Vector3 (x, 0, z) * speed;
    }
	}

	void FixedUpdate()
	{
		rbody.velocity = movement;
	}

  public bool IsSelected
  {
    get;
    set;
  }
}
