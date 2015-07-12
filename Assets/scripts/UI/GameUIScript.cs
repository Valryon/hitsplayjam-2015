using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIScript : MonoBehaviour 
{
  private static GameUIScript instance;

  [Header("Bindings")]
  public Animator animator;
  public Text score1;
  public Text score2;
  public Text timer;
  public Text player1;
  public Text player2;
  public GameObject butPanel;

	void Awake () 
  {
    instance = this;

    foreach (var goal in FindObjectsOfType<GoalScript>()) 
    {
      goal.OnGoal += Goal;
    }

    score1.text = "0";
    score2.text = "0";
    player1.text = "";
    player2.text = "";
	}

  private void Goal(GoalScript g)
  {
    animator.SetTrigger ("but" + Random.Range (1, 4));

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

  public static void SetPlayer(int team, PlayerScript p)
  {
    if (team == GameScript.TEAM1) {
      instance.player1.text = p.definition.shiityName;
    } else {
      instance.player2.text = p.definition.shiityName;
    }
  }


}
