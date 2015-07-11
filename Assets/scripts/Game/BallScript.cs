using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour 
{
  public int lastTeamTouch;
  public PlayerScript linkedPlayer;
  public bool checkCollision ;

  public event System.Action BallReset; 
  public event System.Action OnShoot; 

  private Rigidbody rbody;
  private Vector3 startPosition;
	
  void Awake()
  {
    rbody = GetComponent<Rigidbody> ();

    IsPickable = true;
    startPosition = this.transform.position;

    this.setActive (true);
  }

  void Update()
  {
    if (linkedPlayer != null) 
    {
      this.transform.position = linkedPlayer.transform.position + linkedPlayer.BallRelativePosition;
    }
  }

  public void Reset()
  {
    if (linkedPlayer != null) 
    {
      linkedPlayer.BallLost();
      linkedPlayer = null;
    }

    this.transform.position = startPosition;
		lastTeamTouch = 0;

    rbody.velocity = Vector3.zero;
    rbody.angularVelocity = Vector3.zero;

    DisableFor (1.25f);

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

  public void Launch (Vector3 force)
  {
    rbody.velocity = Vector3.zero;
    rbody.angularVelocity = Vector3.zero;

    // Ball cannot be picked for few seconds
    DisableFor (0.5f);

    linkedPlayer = null;
    Rigidbody.AddForce (force, ForceMode.Force);

    if (OnShoot != null)
      OnShoot ();
  }

  public void DisableFor(float seconds)
  {
    IsPickable = false;
    StartCoroutine (Timer.Start (seconds, () => {
      IsPickable = true;
    }));
  }
  public void Picked(PlayerScript p ){
    this.linkedPlayer = p;
    lastTeamTouch = p.team;
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
