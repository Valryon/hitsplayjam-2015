using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
  public PlayerDefinition definition;
  public Vector3 ballDistance = new Vector3(2f, 0f, 1f);

  [Header("Team 1")]
  public SimpleAnimation defaultAnimationTeam1;
  public SimpleAnimation[] animationsTeam1;
  
  [Header("Team 2")]
  public SimpleAnimation defaultAnimationTeam2;
  public SimpleAnimation[] animationsTeam2;

  [Header("Affectation")]
  public int team = 1;
  public int number = 1;

  public event System.Action<PlayerScript> OnBallPick;
  public event System.Action<PlayerScript> OnShoot;

	private Vector3 movement;
	private Rigidbody rbody;

  public BallScript ball;

  private GameScript gameScript;
  private SimpleAnimator animator;
  private Vector3 ballDirection;
  public Vector3 startPosition;
  private float flip;
  private bool touchingBall;

  public bool initialized = false;

	void Awake()
	{
		rbody = GetComponent<Rigidbody> ();
    animator = GetComponentInChildren<SimpleAnimator> ();

    if (definition == null) 
    {
      Debug.LogError("Missing shiity definition, shitload of errors incoming");
    }

    gameScript = FindObjectOfType<GameScript> ();

    if (team == GameScript.TEAM1) {
      animator.defaultAnimation = defaultAnimationTeam1;
      animator.clips = animationsTeam1;
    } else {
      animator.defaultAnimation = defaultAnimationTeam2;
      animator.clips = animationsTeam2;
    }

    ballDirection = new Vector3 (team == GameScript.TEAM1 ? -1 : 1, 0, 0); 
    BallRelativePosition = Vector3.Scale(ballDirection, ballDistance);

    IsActive = true;


    flip = (team == GameScript.TEAM1 ? -1 : 1);
    this.transform.localScale = new Vector3 (this.transform.localScale.x * flip, this.transform.localScale.y, this.transform.localScale.z);
   
    this.gameObject.name = definition.name;
	}

	void Start () 
	{

	}

	void Update () 
	{
    // Clean
    movement = Vector3.zero;

    // Move
    if (IsActive)
    {
      if (IsSelected) 
      {
        float x = 0f;
        float z = 0f;
        bool pass = false;
        bool shoot = false;
        bool attack = false;

        // Team 1: arrows
        if (team == GameScript.TEAM1) {
          x = Input.GetAxis ("Horizontal");
          if (x < -0.25f) {
            x = -1f;
          } else if (x > 0.25f) {
            x = 1f;
          }

          z = Input.GetAxis ("Vertical");
          if (z < -0.25f) {
            z = -1f;
          } else if (z > 0.25f) {
            z = 1f;
          }

          pass = Input.GetKeyDown (PlayerInputsScheme.Player1Action1);
          shoot = Input.GetKeyDown (PlayerInputsScheme.Player1Action2);
          attack = Input.GetKeyDown (PlayerInputsScheme.Player1Action2);
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

          pass = Input.GetKeyDown (PlayerInputsScheme.Player2Action1);
          shoot = Input.GetKeyDown (PlayerInputsScheme.Player2Action2);
          attack = Input.GetKeyDown (PlayerInputsScheme.Player2Action2);
        }

        float speedPenalty = 1f;
        if (ball != null) {
          speedPenalty = 0.9f;
        }

        const float baseSpeed = 2f;

        Vector3 direction = new Vector3 (x, 0, z);
        movement = direction * baseSpeed * definition.speed * speedPenalty;

        // Change ball position
        const float deadZone = 0.5f;
        if (Mathf.Abs (direction.x) > deadZone || Mathf.Abs (direction.z) > deadZone) {
          // Putain c'est pourri ça marche mal
          ballDirection = new Vector3 (direction.x, 0, direction.z);
        }

        if (shoot)
          Shoot ();
        if (attack)
          Attack ();
        if (pass)
          Pass ();

      } // Selected
      else 
      {
        var gs  = GameObject.FindObjectOfType<GameScript>();
        if(gs.attacking !=0)
        {
          //Skynet prend le controle :) 
          if(definition.role == ROLE.Defense)
            Defending();
          if(definition.role == ROLE.Attack)
            Attacking();
          if(definition.role == ROLE.Keeper)
            Keeping();
        }

      }
    } // Active

    if (ball != null) 
    {
      BallRelativePosition = Vector3.Scale(ballDirection, ballDistance);
    }

    UpdateFlip ();
  }


  private void Defending()
  {
    var gs = GameObject.FindObjectOfType<GameScript> ();
    var b = gs.ball.transform.position;
    var dx = 0f;

    if (team == 2)
      dx = (transform.position.x > 0) ? 0 : Mathf.Sign(b.x-transform.position.x);
    else
      dx = (transform.position.x < 0) ? 0 : Mathf.Sign(b.x-transform.position.x);

    var side = (transform.position.z < 0) ? -1 : 1; 
    var dz = Mathf.Sign (b.z - transform.position.z);

    if (side == 1)
      dz = Mathf.Max (0, dz);
    else
      dz = Mathf.Min (0, dz);

    movement = definition.speed * new Vector3 (dx,0,dz);
  }

  private void UpdateFlip ()
  {
    float previousFlip = flip;

    // Auto flip
    if (movement.x > 0)
      flip = 1f;
    else
      if (movement.x < 0)
        flip = -1f;

    if (movement.magnitude > 0) 
    {
      animator.Play ("walk");
    }

    if (previousFlip != flip) 
    {
      this.transform.localScale = new Vector3 (Mathf.Abs (this.transform.localScale.x) * flip, this.transform.localScale.y, this.transform.localScale.z);
    }
  }

  private void Attacking()
  {
    var gs = GameObject.FindObjectOfType<GameScript> ();
    var b = gs.ball.transform.position;
    var dx = 0f;

    if (gs.attacking == team)
      dx = (transform.position.x < 0) ? 0 : Mathf.Sign(b.x-transform.position.x);
    else
      dx = (transform.position.x > 0) ? 0 : Mathf.Sign(b.x-transform.position.x);

    var side = (transform.position.z < 0) ? -1 : 1; 
    var dz = Random.Range(-1f,1f);

    if (side == 1)
      dz = Mathf.Max (0, dz);
    else
      dz = Mathf.Min (0, dz);

    movement = definition.speed * new Vector3 (dx,0,dz);
  }


  private void Keeping()
  {
    var gs = GameObject.FindObjectOfType<GameScript> ();
    var b = gs.ball.transform.position;
    var side = (team == 2) ? -1 : 1;
    var p = transform.position;
    var dx = 0;

    if (p.x != side * 32) {
      dx = side;
    }

    var dz = 0f;
    if (Mathf.Abs (p.z) < 5)
      dz = Mathf.Sign (b.z);
    else {
      dz = Mathf.Sign (p.z) * -1;
    }

    movement = definition.speed * new Vector3 (dx,0,dz);
  }


	void FixedUpdate()
	{
    rbody.velocity = new Vector3(movement.x, rbody.velocity.y, movement.z);
	}

  void OnTriggerEnter(Collider c)
  {
    HandleBallCollision (c);

  }

  void OnTriggerStay(Collider c)
  {
    HandleBallCollision (c);
  }

  void HandleBallCollision (Collider c)
  {
    if (this.ball != null)
      return;

    BallScript b = c.GetComponent<BallScript> ();

    // Touching the ball
    if (b != null) 
    {
      touchingBall = true;

      // Link?
      if (b.linkedPlayer == null && b.IsPickable) 
      {
        b.linkedPlayer = this;
        this.ball = b;

        // Reset ball direction
        ballDirection = new Vector3 (team == GameScript.TEAM1 ? -1 : 1, 0, 0);

        if (OnBallPick != null) 
        {
          OnBallPick (this);
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
    Vector3 shootDirection = new Vector3 (ballDirection.x, 0.25f * definition.lobForce, ballDirection.z);

    Shooting (shootDirection, 350f, false);

    ball = null;

    CameraShaker.ShakeCamera (0.2f, 0.3f);
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

    Shooting (shootDirection, 40f, false);

    CameraShaker.ShakeCamera (0.1f, 0.15f);
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
      direction.Normalize();

      Vector3 shootDirection = new Vector3 (direction.x, 0.15f, direction.z);

      BallRelativePosition = Vector3.Scale(Vector3.Normalize(shootDirection), ballDistance);
      ball.ApplyBallPosition();

      Shooting(shootDirection, 200f, true);

      ball = null;

      CameraShaker.ShakeCamera (0.1f, 0.15f);
    }
  }

  private void Shooting(Vector3 direction, float forceBase, bool pass)
  {
    Vector3 force = direction * forceBase * definition.shootForce;

    BallScript b = ball;
    if (b == null) 
    {
      b = FindObjectOfType<BallScript>();
    }
  
    b.Launch (force, pass);

    SoundsScript.Play ("balle", this.transform.position);

    if (OnShoot != null) OnShoot (this);
  }

  public void BackToYourPlace()
  {
    IsActive = false;

    StartCoroutine (Interpolators.Curve (Interpolators.EaseInOutCurve, this.transform.position, startPosition, 2f, 
     (s) => 
     {
      // Hack flip
      movement = Vector3.Normalize(s) * -1;
      UpdateFlip();
      movement = Vector2.zero;

      animator.Play("walk");

      this.transform.position = new Vector3(s.x, this.transform.position.y, s.z);
     }, 
    () => {
      IsActive = true;
    }));
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

  public Rigidbody Rigidbody
  {
    get
    {
      return rbody;
    }
  }
}
