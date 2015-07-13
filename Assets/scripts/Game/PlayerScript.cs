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
  public bool IAActive = false;

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

    if (definition.team == GameScript.TEAM1) {
      animator.defaultAnimation = defaultAnimationTeam1;
      animator.clips = animationsTeam1;
    } else {
      animator.defaultAnimation = defaultAnimationTeam2;
      animator.clips = animationsTeam2;
    }

    ballDirection = new Vector3 (definition.team == GameScript.TEAM1 ? -1 : 1, 0, 0); 
    BallRelativePosition = Vector3.Scale(ballDirection, ballDistance);

    IsActive = true;


    flip = (definition.team == GameScript.TEAM1 ? -1 : 1);
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
        if (definition.team == GameScript.TEAM1) {
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
        else if (definition.team == GameScript.TEAM2) {
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

        //Skynet prend le controle :) 
        if(definition.role == ROLE.Defense){
          //Defending();
        }
        if(definition.role == ROLE.Attack){
          //Attacking();
        }
        if(definition.role == ROLE.Keeper){
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
  {/*
    var gs = GameObject.FindObjectOfType<GameScript> ();
    var b = gs.ball.transform.position;
    var dx = 0f;
    var p = transform.position;

    if (definition.team == 2)
      dx = (transform.position.x > 0) ? 0 : Mathf.Sign(b.x-transform.position.x);
    else
      dx = (transform.position.x < 0) ? 0 : Mathf.Sign(b.x-transform.position.x);
    var target = Vector3.zero;
    var side = Mathf.Sign (startPosition.z );
    var dz = Mathf.Sign (b.z - transform.position.z);

    var z = Mathf.Abs (transform.position.z);
    if(ball != null && ball.lastTeamTouch != definition.team){
      dz = 0;

    }
    {
      if (side <0){
        if(p.z<-19)
          dz = 1;
        else if(p.z>-1)
          dz = -1;
      }
      else{
        if(p.z>19)
          dz = -1;
        else if(p.z<1)
          dz = 1;
      }
    }
    if(startPosition.x>0){
      if(p.x<0 )
        dx =1;
      else if (p.x>30)
        dx = -1;
    }else {
      if(p.x>0)
        dx =-1;
      else if (p.x < -30)
        dx = 1;
    }

    target.z = b.z;
    if (side <0){
      if(p.z<-19)
        target.z = -19;
      else if(p.z>-1)
        target.z = -1;
    }
    else{
      if(p.z>19)
        target.z = 19;
      else if(p.z<1)
        target.z = 1;
    }

    dz = Mathf.Sign (p.z - target.z);
    if (target.z - p.z < definition.speed) {
      dz = 0;
    }
    */
//    var gs = GameObject.FindObjectOfType<GameScript> ();
//    var b = gs.ball.transform.position;
//    var dx = 0f;
//    var p = transform.position;
//    Vector3 tgt = Vector3.zero;
//
//    //equipe 1
//    if (definition.team == 1) {
//      var dist  = Vector3.Distance(b, transform.position);
//      if(dist >1){
//
//
//      }
//
//
//    }
//    

//    movement = definition.speed * new Vector3 (dx,0,0);
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
    var p = transform.position;
    if (gs.attacking == definition.team)
      dx = (transform.position.x < 0) ? 0 : Mathf.Sign(b.x-transform.position.x);
    else
      dx = (transform.position.x > 0) ? 0 : Mathf.Sign(b.x-transform.position.x);

   
    var side = Mathf.Sign (startPosition.z );
    var dz = Mathf.Sign (b.z - transform.position.z);
//    var z = Mathf.Abs (transform.position.z);
    if (side <0){
      if(p.z<-10)
        dz = 1;
      else if(p.z>-10)
        dz = -1;
    }
    else{
      if(p.z>10)
        dz = -1;
      else if(p.z<10)
        dz = 1;
    }
    if (definition.team == 1) {
      if(p.x>0){
        dx=-1;

      }
      if(p.x<-28)
        dx= 1;



    } else {
      if(p.x>0){
        dx=1;
        
      }
      if(p.x>28)
        dx= -1;
      

    }



   

    movement = definition.speed * new Vector3 (dx,0,dz);
  }


  private void Keeping()
  {
    var gs = GameObject.FindObjectOfType<GameScript> ();
    var b = gs.ball.transform.position;
    var side = (definition.team == 2) ? -1 : 1;
    var p = transform.position;
    var dx = 0;

    //31 ligne de but
    if (p.x != side * 31) {
      if(Mathf.Abs(p.x -side * 31) > definition.speed )
        dx = side;
    }

    var dz = 0f;

    if (Mathf.Abs (p.z) < 5)
      dz = Mathf.Sign (b.z- p.z);
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

    if (IsActive == false)
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
        ballDirection = new Vector3 (definition.team == GameScript.TEAM1 ? -1 : 1, 0, 0);

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
    Vector3 shootDirection = new Vector3 (ballDirection.x, 0.25f * definition.lobForceFactor, ballDirection.z);

    Shooting (shootDirection, definition.shootForce, false);

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

    Shooting (shootDirection, definition.attackForce, false);

    CameraShaker.ShakeCamera (0.1f, 0.15f);
  }

  private void Pass()
  {
    if (ball == null)
      return;

    PlayerScript nearest = gameScript.GetNearestPlayer ((definition.team == GameScript.TEAM1 ? gameScript.team1 : gameScript.team2), this.gameObject);

    if (nearest != null) 
    {
      // Small targeted shoot
      Vector3 direction = (nearest.transform.position - this.transform.position);
      direction.Normalize();

      Vector3 shootDirection = new Vector3 (direction.x, 0.15f, direction.z);

      BallRelativePosition = Vector3.Scale(Vector3.Normalize(shootDirection), ballDistance);
      ball.ApplyBallPosition();

      Shooting(shootDirection, definition.passForce, true);

      ball = null;

      CameraShaker.ShakeCamera (0.1f, 0.15f);
    }
  }

  private void Shooting(Vector3 direction, float force, bool pass)
  {
    Vector3 f = direction * force;

    BallScript b = ball;
    if (b == null) 
    {
      b = FindObjectOfType<BallScript>();
    }
  
    b.Launch (f, pass);

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
