using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour 
{
  private Vector3 startPosition;
	public int lastTeamTouch;

  void Awake()
  {
    startPosition = this.transform.position;
	
  }

  public void Reset()
  {
    this.transform.position = startPosition;
		lastTeamTouch = 0;
  }


  void OnCollisionEnter(Collision other){
    var col = other.collider;
    if (col.tag != "Player")
      return;
    var p = col.GetComponent<PlayerScript> ();
    lastTeamTouch = p.team;
    Debug.Log ("poc by ");

  }
}
