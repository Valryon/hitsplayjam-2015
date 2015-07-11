using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
  public const float BALL_DISTANCE_FROM_PLAYER = 2f;

  public int team = 1;
  public int number = 1;
  public PlayerDefinition definition;

	private Vector3 movement;
	private Rigidbody rbody;
  private Renderer render;

  private BallScript ball;
  private GameScript gameScript;

  private Vector3 ballDirection;

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

    ballDirection = new Vector3 (team == GameScript.TEAM1 ? -1 : 1, 0, 0); 
    BallRelativePosition = ballDirection * BALL_DISTANCE_FROM_PLAYER;
	}

	void Start () 
	{
	}

	void Update () 
	{
    // Clean
    movement = Vector3.zero;

    // Move
    if (IsSelected) 
    {
      float x = 0f;
      float z = 0f;

      // Team 1: arrows
      if (team == GameScript.TEAM1) 
      {
        x = Input.GetAxis ("Horizontal");
        if (x < -0.25f)
        {
          x = -1f;
        }
        else if (x > 0.25f)
        {
          x = 1f;
        }

        z = Input.GetAxis ("Vertical");
        if (z < -0.25f)
        {
          z = -1f;
        }
        else if (z > 0.25f)
        {
          z = 1f;
        }
      }
      // Team 2: ZQSD
      else if (team == GameScript.TEAM2) 
      {
        if (Input.GetKey (KeyCode.Q)) 
        {
          x = -1f; 
        } 
        else if (Input.GetKey (KeyCode.D)) 
        {
          x = 1f;
        }
        if (Input.GetKey (KeyCode.Z)) 
        {
          z = 1f; 
        }
        else if (Input.GetKey (KeyCode.S)) 
        {
          z = -1f;
        }
      }

      Vector3 direction =  new Vector3 (x, 0, z);
      movement = direction * definition.speed;

      // Change ball position
      if(direction.x != 0f || direction.z != 0f)
      {
        float ballX = 0f;
        float ballZ = 0f;

        if(Mathf.Abs(direction.x) > 0.2f)
        {
          ballX = 1f * Mathf.Sign(direction.x);
        }
        if(Mathf.Abs(direction.z) > 0.2f)
        {
          ballZ = 1f * Mathf.Sign(direction.z);
        }

        ballDirection = new Vector3(ballX, 0, ballZ);
      }
    }

    if (ball != null) 
    {
      Debug.Log(ballDirection);
      BallRelativePosition = ballDirection * BALL_DISTANCE_FROM_PLAYER;
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
        Debug.Log(this.name + " PREND LA BALLE");
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

  public Vector3 BallRelativePosition
  {
    get;
    set;
  }
}
