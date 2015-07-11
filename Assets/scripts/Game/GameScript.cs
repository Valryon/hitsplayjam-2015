using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameScript : MonoBehaviour 
{
  public const int TEAM1 = 1;
  public const int TEAM2 = 2;

  private  const float OFFSET_PLAYER = 1;

  public float time = 60f;

  public List<PlayerScript> team1;
  public List<PlayerScript> team2;

  public PlayerScript player1, player2;
  private int player1Index, player2Index;

  private BallScript ball;

  private float timeLeft;
  private bool paused;

  void Awake()
  {
    // Ball
    ball = FindObjectOfType<BallScript> ();

    // Players
    var players = FindObjectsOfType<PlayerScript> ();
    if (players.Length == 0) 
    {
      Debug.LogError("No players WTF");
    }

    foreach (var p in players) 
    {
      p.OnBallPick += (pickingPlayer) =>
      {
        SelectTeam(pickingPlayer, pickingPlayer.team);
      };
    }

    team1 = players.Where (p => p.team == TEAM1).OrderBy(p => p.number).ToList ();
    team2 = players.Where (p => p.team == TEAM2).OrderBy(p => p.number).ToList ();

    InputHandleSelection (true);

    // Goal
    foreach (GoalScript g in  FindObjectsOfType<GoalScript>()) 
    {
      g.OnGoal += GOAL;
    }
  }

  void Start () 
  {
    timeLeft = time;
	}
	
	void Update () 
  {
    if (paused == false) 
    {
      timeLeft -= Time.deltaTime;
      if(timeLeft <= 0f)
      {
        timeLeft = 0f;
        GameOver();
      }

      GameUIScript.SetTimerValue(timeLeft);

      InputHandleSelection (false);
    }
	}

  private void InputHandleSelection(bool forceSelection)
  {
    if (ball == null)
      return;

    HandleSelectionForPlayer (TEAM1, forceSelection);

    HandleSelectionForPlayer (TEAM2, forceSelection);
  }

  private void HandleSelectionForPlayer(int teamNumber, bool force)
  {
    List<PlayerScript> ps = null;

    // Team: change selected character
    if (teamNumber == TEAM1 && (force || Input.GetKeyDown(PlayerInputsScheme.Player1Action1)))
    {
      if(player1 == null || (player1 != null && player1.HasBall == false))
      {
        ps = team1;
      }
    }
    else if (teamNumber == TEAM2 && (force || Input.GetKeyDown(PlayerInputsScheme.Player2Action1)) )
    {
      if(player2 == null || (player2 != null && player2.HasBall == false))
      {
        ps = team2;
      }
    }
    
    // No input? Nothing to do
    if (ps == null)
      return;
    
    var closestPlayer = GetNearestPlayer (ps, ball.gameObject);
    
    int index = ps.IndexOf (closestPlayer);
    
    // Gogo
    SelectTeam(ps[index], teamNumber);
  }

  private void SelectTeam(PlayerScript p, int team)
  {
    if (team == TEAM1) 
    {
      if(player1 != null)
      {
        player1.IsSelected = false;
      }
      player1 = p;
      player1.IsSelected = true;
    } 
    else if (team == TEAM2) 
    {
      if(player2 != null)
      {
        player2.IsSelected = false;
      }
      player2 = p;
      player2.IsSelected = true;
    }
  }

  private void GOAL(GoalScript goalScript)
  {
    // Text, particles, juice
    BallCamera.FollowBall = false;
    CameraShaker.ShakeCamera (0.5f, 1f);

    foreach (var p in team1) 
    {
      p.BackToYourPlace();
    }
    foreach (var p in team2) 
    {
      p.BackToYourPlace();
    }

    // Wait
    StartCoroutine (Timer.Start (3f, () =>
    {
      // Reset everything
      ball.Reset();
      BallCamera.FollowBall = true;
    }));
  }

  private void GameOver()
  {

  }

  public PlayerScript GetNearestPlayer (List<PlayerScript> ps, GameObject from)
  {
    // Get nearest player from the ball
    float minDistance = float.MaxValue;
    PlayerScript closestPlayer = null;

    foreach (var p in ps) 
    {
      // Always allow change
      if (p == player1 || p == player2)
        continue;

      var distance = Vector3.Distance (p.transform.position, from.transform.position);
      if (distance < minDistance) 
      {
        minDistance = distance;
        closestPlayer = p;
      }
    }

    return closestPlayer;
  }

  public void setLineOutSituation(Vector3 position, int team)
  {
    StartCoroutine (_coLineOut(position, team));
  }

  private IEnumerator _coLineOut(Vector3 position, int team){
    var ballp = position;

    var gs = GameObject.FindObjectOfType<GameScript> ();
    List<PlayerScript> te, to;
    if (team == TEAM1) {
      te = team1;
      to = team2;
    } else {
      to = team1;
      te = team2;
    }
    var ste = te.OrderBy (t => Vector3.Distance (t.transform.position, ballp)).ToList();
    var sto = to.OrderBy (t => Vector3.Distance (t.transform.position, ballp)).ToList();
    var sign = Mathf.Sign (ballp.z);

    //send opponents close to the ball
    for (int i =0; i<2; i++) {
      PlayerScript o  = sto[i];
      ballp.y = o.transform.position.y;

      Vector3 goal  = ballp;
      Vector2 offset = Random.insideUnitCircle*Random.Range(3.5f,6f);
      goal.x  = goal.x + offset.x ;
      goal.z  = goal.z + Mathf.Abs(offset.y)*-sign;
      Debug.Log("player "+o.transform.position  +"ball "+ballp+"goal" +goal);
      StartCoroutine( Interpolators.Curve(Interpolators.EaseInOutCurve,o.transform.position,goal,1,step => {
        o.transform.position= step;
      },null));
    }
    //send player close to the ball
    {
      PlayerScript o = ste [0];
      ballp.y = o.transform.position.y;
      Vector3 goal = ballp+Vector3.forward* Mathf.Sign(ballp.z)* 2 * OFFSET_PLAYER;
      StartCoroutine (Interpolators.Curve (Interpolators.EaseInOutCurve, o.transform.position, goal, 1, step => {
        o.transform.position = step;
      }, null));
    }
    for (int i =1; i<3; i++) {
      PlayerScript o  = ste[i];
      ballp.y = o.transform.position.y;

      Vector3 goal  = ballp;
      Vector2 offset = Random.insideUnitCircle*Random.Range(3.5f,6f);
      goal.x  = offset.x;
      goal.z  = Mathf.Abs(offset.y)*-sign;
      Debug.Log("player "+o.transform.position  +"ball "+ballp+"goal" +goal);
      StartCoroutine( Interpolators.Curve(Interpolators.EaseInOutCurve,o.transform.position,goal,1,step => {
        o.transform.position= step;
      },null));
    }
    yield return new WaitForSeconds (1.1f);
    var ball = FindObjectOfType<BallScript> ();
    ball.transform.position = ballp;
    ball.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;

  }

  public void disableAllPLayer(){
    player1.IsSelected = false;
    player1 = null;
    player2.IsSelected = false;
    player2 = null;
    ball.linkedPlayer.ball = null;
    ball.linkedPlayer = null;
  }

}
  
