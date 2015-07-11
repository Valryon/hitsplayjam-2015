using UnityEngine;
using System.Collections;

public enum ROLE{
  Defense,
  Attack,
  Keeper  
};

public class PlayerDefinition : ScriptableObject 
{
  [Header("Menu")]
  public Sprite avatar;
  public string shiityName;
  public string biography;

  [Header("Team 1")]
  public SimpleAnimation defaultAnimationTeam1;
  public SimpleAnimation[] animationsTeam1;

  [Header("Team 2")]
  public SimpleAnimation defaultAnimationTeam2;
  public SimpleAnimation[] animationsTeam2;

  [Header("Stats")]

  public bool isGoalKeeper = false;
  public ROLE role;

  public float scaleX = 1f;
  public float scaleY = 1f;

  [Range(1,5)]
  public int speed = 1;

  [Range(1,5)]
  public int shootForce = 1;

  [Range(1,5)]
  public int lobForce = 1;

  public string specialScriptName;
}
