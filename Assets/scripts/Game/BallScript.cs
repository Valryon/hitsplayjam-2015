using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour 
{
  public int lastTeamTouch;
  public PlayerScript linkedPlayer;
  public bool checkCollision ;

  public event System.Action BallReset; 

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

    DisableFor (2f);

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
    // Ball cannot be picked for few seconds
    DisableFor (1f);

    linkedPlayer = null;
    Rigidbody.AddForce (force, ForceMode.Force);
  }

  public void DisableFor(float seconds)
  {
    IsPickable = false;
    StartCoroutine (Timer.Start (seconds, () => {
      IsPickable = true;
    }));
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
