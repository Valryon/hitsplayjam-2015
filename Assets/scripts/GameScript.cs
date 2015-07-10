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

  void Awake()
  {
    var players = FindObjectsOfType<PlayerScript> ();

    team1 = players.Where (p => p.team == TEAM1).ToList ();
    SelectTeam(team1[0], TEAM1);

    team2 = players.Where (p => p.team == TEAM2).ToList ();
    SelectTeam(team2[0], TEAM2);
  }

  void Start () 
  {
	
	}
	
	void Update () 
  {
	
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
}
