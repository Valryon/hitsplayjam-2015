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
     
  }
}
