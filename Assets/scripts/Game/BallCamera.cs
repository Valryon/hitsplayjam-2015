using UnityEngine;
using System.Collections;

public class BallCamera : MonoBehaviour 
{
  public static bool FollowBall = true;

  private BallScript ball;
  private float targetX;
  private float speed = 0.07f;
  private float directionX;

  void Start()
  {
    ball = FindObjectOfType<BallScript> ();
    ball.BallReset += () =>  
    {
      this.transform.position = new Vector3(ball.transform.position.x, this.transform.position.y, this.transform.position.z);
    };

    targetX = this.transform.position.x;
  }

	void Update () 
  {
    if (ball == null || FollowBall == false)
      return;

    directionX -= 0.1f;
    if (directionX < 0f)
      directionX = 0f;

    // Rect on screen. If ball is outside, follow it.
    Vector3 viewportCoords = Camera.main.WorldToViewportPoint (ball.transform.position);

    const float zoneSize = 0.1f;
   
    if (viewportCoords.x < (0.5f - zoneSize)) 
    {
      directionX = -1f;
    } 
    else if (viewportCoords.x > (0.5f + zoneSize)) 
    {
      directionX = 1f;
    }

    float currentX = this.transform.position.x + (directionX * speed);

    this.transform.position = new Vector3 (currentX, this.transform.position.y, this.transform.position.z);
	}
}
