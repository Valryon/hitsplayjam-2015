using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour 
{
  public int lastTeamTouch;
  public PlayerScript linkedPlayer;
  public bool checkCollision ;

  private Rigidbody rbody;
  private Vector3 startPosition;
	
  void Awake()
  {
    this.setActive (true);

    rbody = GetComponent<Rigidbody> ();

    IsPickable = true;

    startPosition = this.transform.position;
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
    this.transform.position = startPosition;
		lastTeamTouch = 0;

    rbody.velocity = Vector3.zero;
  }


  public void setActive(bool active){
    var rb = this.GetComponent<Rigidbody> ();
    rb.isKinematic = !active;
    checkCollision = active;
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

}
