using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIScript : MonoBehaviour 
{
  private static GameUIScript instance;

  [Header("Bindings")]
  public Text score1;
  public Text score2;
  public Text timer;

	void Awake () 
  {
    instance = this;

    foreach (var goal in FindObjectsOfType<GoalScript>()) 
    {
      goal.OnGoal += Goal;
    }

    score1.text = "0";
    score2.text = "0";
	}

  private void Goal(GoalScript g)
  {
    if (g.team == GameScript.TEAM1) 
    {
      score1.text = g.score.ToString();
    } 
    else if (g.team == GameScript.TEAM2) 
    {
      score2.text = g.score.ToString();
    }
  }
	
	void Update () 
  {
	
	}

  public static void SetTimerValue(float t)
  {
    if (instance != null) 
    {
      instance.timer.text = t.ToString("00.00");
    }
  }
}
