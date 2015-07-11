using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
  public const float BALL_DISTANCE_FROM_PLAYER = 1.25f;

  public int team = 1;
  public int number = 1;
  public PlayerDefinition definition;

  public event System.Action<PlayerScript> OnBallPick;

	private Vector3 movement;
	private Rigidbody rbody;
  private Renderer render;

  public BallScript ball;

  private GameScript gameScript;

  private Vector3 ballDirection;
  private Vector3 startPosition;

  private bool touchingBall;

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

    IsActive = true;
    startPosition = this.transform.position;
	}

	void Start () 
	{
	}

	void Update () 
	{
    // Clean
    movement = Vector3.zero;

    // Move
    if (IsSelected && IsActive) 
    {
      float x = 0f;
      float z = 0f;
      bool pass = false;
      bool shoot = false;
      bool attack = false;

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

        pass = Input.GetKeyDown(PlayerInputsScheme.Player1Action1);
        shoot = Input.GetKeyDown(PlayerInputsScheme.Player1Action2);
        attack = Input.GetKeyDown(PlayerInputsScheme.Player1Action2);
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

        pass = Input.GetKeyDown(PlayerInputsScheme.Player2Action1);
        shoot = Input.GetKeyDown(PlayerInputsScheme.Player2Action2);
        attack = Input.GetKeyDown(PlayerInputsScheme.Player2Action2);
      }

      Vector3 direction =  new Vector3 (x, 0, z);
      movement = direction * definition.speed;

      // Change ball position
      const float deadZone = 0.5f;
      if(Mathf.Abs(direction.x) > deadZone || Mathf.Abs(direction.z) > deadZone)
      {
        // Putain c'est pourri ça marche mal
        ballDirection = new Vector3(direction.x, 0, direction.z);
      }

      if(shoot) Shoot();
      if(attack) Attack();
      if(pass) Pass();
    }

    if (ball != null) 
    {
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
      touchingBall = true;

      // Link?
      if(b.linkedPlayer == null && b.IsPickable)
      {
        b.linkedPlayer = this;
        this.ball = b;

        if(OnBallPick != null)
        {
          OnBallPick(this);
        }
      }
    }
  }

  void OnTriggerExit(Collider c)
  {
    BallScript b = c.GetComponent<BallScript> ();

    if (b != null) 
    {
      touchingBall = false;
    }
  
  }

  private void Shoot()
  {
    if (ball == null)
      return;

    // Shooooot
    Vector3 shootDirection = new Vector3 (ballDirection.x, 0.15f, ballDirection.z);

    Shooting (shootDirection, 1000f);

    ball = null;
  }

  public void BallLost ()
  {
    ball = null;
  }

  private void Attack()
  {
    if (ball != null)
      return;

    if (touchingBall == false)
      return;

    Vector3 shootDirection = new Vector3 (ballDirection.x, 0.15f, ballDirection.z);

    Shooting (shootDirection, 100f);
  }

  private void Pass()
  {
    if (ball == null)
      return;

    PlayerScript nearest = gameScript.GetNearestPlayer ((team == GameScript.TEAM1 ? gameScript.team1 : gameScript.team2), this.gameObject);

    if (nearest != null) 
    {
      // Small targeted shoot
      Vector3 direction = (nearest.transform.position - this.transform.position);

      Vector3 shootDirection = new Vector3 (direction.x, 0.15f, direction.z);

      Shooting(shootDirection, 50f);

      ball = null;
    }
  }

  private void Shooting(Vector3 direction, float forceBase)
  {
    Vector3 force = direction * forceBase * definition.shootForce;

    BallScript b = ball;
    if (b == null) 
    {
      b = FindObjectOfType<BallScript>();
    }
  
    b.Launch (force);
  }

  public void BackToYourPlace()
  {
    StartCoroutine (Interpolators.Curve (Interpolators.EaseInOutCurve, this.transform.position, startPosition, 2f, 
     (s) => 
     {
      this.transform.position = s;
     }, null));
  }

  public bool IsActive 
  {
    get;
    set;
  }

  public bool HasBall 
  {
    get
    {
      return (ball != null);
    }
  }

  /// <summary>
  /// The football player is currently selected and played by the player
  /// </summary>
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
