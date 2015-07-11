using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
  public int team = 1;
  public int number = 1;
  public PlayerDefinition definition;
  public Transform ballPosition;

	private Vector3 movement;
	private Rigidbody rbody;
  private Renderer render;

  private BallScript ball;
  private GameScript gameScript;

	void Awake()
	{
		rbody = GetComponent<Rigidbody> ();
    render = GetComponentInChildren<Renderer> ();

    if (definition == null) 
    {
      Debug.LogError("Missing shiity definition, shitload of errors incoming");
    }

    gameScript = FindObjectOfType<GameScript> ();

    // Apply sprite and stuff
    this.render.material.color = gameScript.GetTeamColor(team);
    this.render.material.mainTexture = definition.sprite;
	}

	void Start () 
	{
	}

	void Update () 
	{
    movement = Vector3.zero;

    if (IsSelected) {
      float x = 0f;
      float z = 0f;

      // Team 1: arrows
      if (team == GameScript.TEAM1) {
        x = Input.GetAxis ("Horizontal");
        if (x < -0.25f)
          x = -1f;
        else if (x > 0.25f)
          x = 1f;

        z = Input.GetAxis ("Vertical");
        if (z < -0.25f)
          z = -1f;
        else if (z > 0.25f)
          z = 1f;
      }
      // Team 2: ZQSD
      else if (team == GameScript.TEAM2) {
        if (Input.GetKey (KeyCode.Q)) {
          x = -1f; 
        } else if (Input.GetKey (KeyCode.D)) {
          x = 1f;
        }
        if (Input.GetKey (KeyCode.Z)) {
          z = 1f; 
        } else if (Input.GetKey (KeyCode.S)) {
          z = -1f;
        }
      }

      movement = new Vector3 (x, 0, z) * definition.speed;
    }
  }

	void FixedUpdate()
	{
		rbody.velocity = movement;
	}

  void OnTriggerEnter(Collider c)
  {
    BallScript b = c.GetComponent<BallScript> ();

    // Touching the ball
    if (b != null) 
    {
      // Link?
      if(b.linkedPlayer == null && b.IsPickable)
      {
        b.linkedPlayer = this;
        this.ball = b;
      }
    }
  }

  public bool IsSelected
  {
    get;
    set;
  }
}
