using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameScript : MonoBehaviour 
{
  public const int TEAM1 = 1;
  public const int TEAM2 = 2;

  private  const float OFFSET_PLAYER = 1;

  public List<PlayerScript> team1;
  public Color team1Color = Color.Lerp(Color.red, Color.blue, 0.5f);
  public List<PlayerScript> team2;
  public Color team2Color = Color.yellow;

  public PlayerScript player1, player2;
  private int player1Index, player2Index;

  private BallScript ball;

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
	}
	
	void Update () 
  {
    InputHandleSelection (false);
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
    
    // Get nearest player from the ball
    float minDistance = float.MaxValue;
    PlayerScript closestPlayer = null;
    
    foreach(var p in ps)
    {
      // Always allow change
      if(p == player1 || p == player2) continue;

      var distance = Vector3.Distance(p.transform.position, ball.transform.position);
      
      if(distance < minDistance)
      {
        minDistance = distance;
        closestPlayer = p;
      }
    }
    
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

    // Wait
    StartCoroutine (Timer.Start (3f, () =>
    {
      // Reset everything
      ball.Reset();
      BallCamera.FollowBall = true;
    }));
  }

  public Color GetTeamColor (int team)
  {
    if (team == TEAM1) 
    {
      return team1Color;
    }
    else 
    {
      return team2Color;
    }
  }

  public void setLineOutSituation(Vector3 position, int team){

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
    
    //send opponents close to the ball
    for (int i =0; i<2; i++) {
      PlayerScript o  = sto[i];
      Vector3 goal  = 2f*(o.transform.position+ position)/3f;
      StartCoroutine( Interpolators.Curve(Interpolators.EaseInOutCurve,o.transform.position,goal,1,step => {
        o.transform.position= step;
      },null));
    }
    //send player close to the ball
    {
      PlayerScript o = ste [0];
      Vector3 goal = ballp+Vector3.back* Mathf.Sign(ballp.z)* 2 * OFFSET_PLAYER;
      StartCoroutine (Interpolators.Curve (Interpolators.EaseInOutCurve, o.transform.position, goal, 1, step => {
        o.transform.position = step;
      }, null));
    }
    for (int i =1; i<3; i++) {
      PlayerScript o  = ste[i];
      Vector3 goal  = 2f*(o.transform.position+ position)/3f;
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
