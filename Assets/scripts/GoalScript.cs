using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour 
{
  public int team = 0;
  public int score = 0;

	void Start () 
  {
	
	}
	
	void Update () 
  {
	
	}

  void OnTriggerEnter(Collider c)
  {
    BallScript ball = c.gameObject.GetComponent<BallScript> ();
    if (ball != null) 
    {
      Goal ();

      // Reset ball
      ball.Reset();
    }
  }

  private void Goal()
  {
    score++;
  }
}
