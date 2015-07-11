using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour 
{
  public int lastTeamTouch;
  public PlayerScript linkedPlayer;

  private Rigidbody rbody;
  private Vector3 startPosition;
	
  void Awake()
  {
    rbody = GetComponent<Rigidbody> ();

    startPosition = this.transform.position;
    IsPickable = true;
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


  void OnCollisionEnter(Collision other)
  {
    var col = other.collider;
    if (col.tag != "Player")
      return;
    var p = col.GetComponent<PlayerScript> ();
    lastTeamTouch = p.team;
    Debug.Log ("poc by ");
  }

  public bool IsPickable 
  {
    get;
    set;
  }

  public void setActive(bool active){
    var rb = this.GetComponent<Rigidbody> ();
    rb.isKinematic = !active;
    checkCollision = active;
  }

}
