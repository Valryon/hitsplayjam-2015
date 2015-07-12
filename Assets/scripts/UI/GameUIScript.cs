using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIScript : MonoBehaviour 
{
  private static GameUIScript instance;

  public Animator animator;

  [Header("Bindings: HUD")]
  public Text score1;
  public Text score2;
  public Text timer;
  public Text player1;
  public Text player2;

  [Header("Bindings: But")]
  public GameObject butPanel;
  public Image butImage;

  [Header("Bindings: Game Over")]
  public GameObject gameOverPanel;

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

    instance.gameOverPanel.SetActive (false);
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

  public void RestartGame()
  {
    Application.LoadLevel ("TitleScreen");
  }

  public void ExitGame()
  {
    Application.Quit ();
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

    instance.butImage.sprite = p.definition.avatar;
  }

  public static void GameOver()
  {
    instance.gameOverPanel.SetActive (true);
  }


}
