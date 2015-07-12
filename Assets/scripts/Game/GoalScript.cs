using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour 
{
  public int team = 0;
  public int score = 0;

  public System.Action<GoalScript> OnGoal;

  private Collider col;

	void Start () 
  {
    col = GetComponent<Collider> ();
	}
	
	void Update () 
  {
	
	}

  void OnTriggerEnter(Collider c)
  {
    BallScript ball = c.gameObject.GetComponent<BallScript> ();
    if (ball != null) 
    {
      GoalTeam = ball.lastTeamTouch;
      Goal ();
    }
  }

  private void Goal()
  {
    score++;

    col.enabled = false;

    if (OnGoal != null) 
    {
      OnGoal (this);
    }

    StartCoroutine(Timer.Start(5f, () => {
      col.enabled = true;
    }));
  }

  public int GoalTeam {
    get;
    private set;
  }
}
