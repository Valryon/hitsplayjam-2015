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
    ball = FindObjectOfType<BallScript> ();

    var players = FindObjectsOfType<PlayerScript> ();
    if (players.Length == 0) 
    {
      Debug.LogError("No players WTF");
    }

    player1Index = 0;
    team1 = players.Where (p => p.team == TEAM1).OrderBy(p => p.number).ToList ();
    SelectTeam(team1[player1Index], TEAM1);

    player2Index = 0;
    team2 = players.Where (p => p.team == TEAM2).OrderBy(p => p.number).ToList ();
    SelectTeam(team2[player2Index], TEAM2);
  }

  void Start () 
  {
	}
	
	void Update () 
  {
    InputHandleSelection ();
	}

  private void InputHandleSelection()
  {
    if (ball == null)
      return;

    HandleSelectionForPlayer (TEAM1);

    HandleSelectionForPlayer (TEAM2);
  }

  private void HandleSelectionForPlayer(int teamNumber)
  {
    List<PlayerScript> ps = null;

    // Team: change selected character
    if (teamNumber == TEAM1 && Input.GetKeyDown(PlayerInputsScheme.Player1Action1))
    {
      if(player1 == null || (player1 != null && player1.HasBall == false))
      {
        ps = team1;
      }
    }
    else if (teamNumber == TEAM2 && Input.GetKeyDown(PlayerInputsScheme.Player2Action1)) 
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
  