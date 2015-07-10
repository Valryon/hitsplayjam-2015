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
        z = Input.GetAxis ("Vertical");
      }
      // Team 2: ZQSD
      else if(team == GameScript.TEAM2)
      {
        x = 0f; //Input.GetKeyDown(KeyCode.Q);
        z = 0f; //Input.GetAxis ("Vertical");
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
