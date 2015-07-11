using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameScript : MonoBehaviour 
{
  public const int TEAM1 = 1;
  public const int TEAM2 = 2;

  public List<PlayerScript> team1;
  public List<PlayerScript> team2;

  private PlayerScript player1, player2;
  private int player1Index, player2Index;

  void Awake()
  {
    var players = FindObjectsOfType<PlayerScript> ();

    player1Index = 0;
    team1 = players.Where (p => p.team == TEAM1).ToList ();
    SelectTeam(team1[player1Index], TEAM1);

    player2Index = 0;
    team2 = players.Where (p => p.team == TEAM2).ToList ();
    SelectTeam(team2[player2Index], TEAM2);
  }

  void Start () 
  {
    setLineOutSituation (Vector3.zero, 0);
	}
	
	void Update () 
  {
	  // Team: change selected character
    if (Input.GetKeyDown(KeyCode.RightShift)) 
    {
      player1Index++;
      if(player1Index >= team1.Count)
      {
        player1Index = 0;
      }
      SelectTeam(team1[player1Index], TEAM1);
    }
    if (Input.GetKeyDown(KeyCode.E)) 
    {
      player2Index++;
      if(player2Index >= team2.Count)
      {
        player2Index = 0;
      }
      SelectTeam(team2[player2Index], TEAM2);
    }
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

  public void setLineOutSituation(Vector3 position, int team){
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



  }

  private IEnumerator _coLineOut(Vector3 position, int team){
    return null;
  }
}
