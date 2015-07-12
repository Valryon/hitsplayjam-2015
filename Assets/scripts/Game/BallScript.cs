using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour 
{
  public int lastTeamTouch;
  public PlayerScript linkedPlayer;
  public bool checkCollision ;

  public event System.Action BallReset; 
  public event System.Action<bool, bool> OnShoot; 

  private Rigidbody rbody;
  private Collider collidr;
  private Vector3 startPosition;
  private float rotationSpeed;
	
  void Awake()
  {
    rbody = GetComponent<Rigidbody> ();
    collidr = GetComponent<Collider> ();

    IsPickable = true;
    startPosition = this.transform.position;

    this.setActive (true);
  }

  void Update()
  {
    if (linkedPlayer != null) {
      Vector3 v = linkedPlayer.Rigidbody.velocity;

      if(v.magnitude > 0f)
      {
        rotationSpeed += (0.1f * Mathf.Sign(v.x));
        rotationSpeed = Mathf.Clamp(rotationSpeed, -1f, 1f);
      }
      else
      {
        rotationSpeed = 0f;
      }

      ApplyBallPosition (); 

      this.transform.Rotate(Vector3.one * rotationSpeed * 10f);
    } 
    else 
    {
      rotationSpeed = 0f;
    }
  }

  public void ApplyBallPosition ()
  {
    this.transform.position = new Vector3 (linkedPlayer.transform.position.x + linkedPlayer.BallRelativePosition.x, this.transform.position.y, linkedPlayer.transform.position.z + linkedPlayer.BallRelativePosition.z);
  }

  public void Reset()
  {
    if (linkedPlayer != null) 
    {
      linkedPlayer.BallLost();
      linkedPlayer = null;
    }
    DisableFor (3f);

    this.transform.position = startPosition;
		lastTeamTouch = 0;

    rbody.velocity = Vector3.zero;
    rbody.angularVelocity = Vector3.zero;

    if (BallReset != null)
      BallReset ();
  }

  void OnCollisionEnter(Collision other)
  {
    var col = other.collider;
    if (col.tag != "Player")
      return;
    var p = col.GetComponent<PlayerScript> ();
    lastTeamTouch = p.team;
    Debug.Log ("poc by ");
  }

  public void setActive(bool active)
  {
    rbody.isKinematic = !active;
    checkCollision = active;
  }

  public void Launch (Vector3 force, bool pass)
  {
    rbody.velocity = Vector3.zero;
    rbody.angularVelocity = Vector3.zero;

    DisableFor (0.5f);

    linkedPlayer = null;
    Rigidbody.AddForce (force, ForceMode.Force);

    if (OnShoot != null)
      OnShoot (!pass, pass);
  }

  public void DisableFor(float seconds)
  {
    IsPickable = false;
    StartCoroutine (Timer.Start (seconds, () => {
      IsPickable = true;
    }));
  }

  public void Picked(PlayerScript p )
  {
    this.linkedPlayer = p;
    lastTeamTouch = p.team;
    var gs = GameObject.FindObjectOfType<GameScript> ();
    gs.attacking = p.team;

  }

  public bool IsPickable 
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
