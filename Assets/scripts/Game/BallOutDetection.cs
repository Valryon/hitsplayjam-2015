using UnityEngine;
using System.Collections;

public class BallOutDetection : MonoBehaviour
{

  public int teamSide = 0;

  void OnTriggerEnter(Collider other)
  {
    if (other.tag != "Ball")
    {
      return;
    }

    var ball = other.GetComponent<BallScript>();
    if (!ball.checkCollision)
    {
      return;
    }

    if (teamSide == 0)
    {
      Debug.Log("C'est la touche");
      Vector3 g = ball.transform.position;

      GameScript gs = FindObjectOfType<GameScript>();
      ball.setActive(false);

      gs.setLineOutSituation(g, (ball.lastTeamTouch == 1) ? 2 : 1);
    }
    else if (teamSide == ball.lastTeamTouch)
    {
      Debug.Log("C'est le corner !!");
    }
    else
    {
      Debug.Log("C'est le 6m !");
    }


  }
}
