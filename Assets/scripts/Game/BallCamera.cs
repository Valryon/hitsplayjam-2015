using UnityEngine;
using System.Collections;

public class BallCamera : MonoBehaviour 
{
  public static bool FollowBall = true;

  private BallScript ball;
  private float targetX;
  private float speed = 0.15f;

  void Start()
  {
    ball = FindObjectOfType<BallScript> ();

    targetX = this.transform.position.x;
  }

	void Update () 
  {
    if (ball == null || FollowBall == false)
      return;

    // Rect on screen. If ball is outside, follow it.
    Vector3 viewportCoords = Camera.main.WorldToViewportPoint (ball.transform.position);

    const float zoneSize = 0.2f;

    if (viewportCoords.x < (0.5f - zoneSize) || viewportCoords.x > (0.5f + zoneSize)) 
    {
      targetX = ball.transform.position.x;
    }

    float distance = targetX - this.transform.position.x;

    if (Mathf.Abs (distance) > 0.1f) 
    {
      float directionX = 0f;
      if (targetX > this.transform.position.x) {
        directionX = 1f;
      } else {
        directionX = -1f;
      }

      float currentX = this.transform.position.x + (directionX * speed);

      this.transform.position = new Vector3 (currentX, this.transform.position.y, this.transform.position.z);
    }
	}
}
