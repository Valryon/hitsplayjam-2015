using UnityEngine;
using System.Collections;

public enum ROLE
{
  Defense,
  Attack,
  Keeper  
};

public class PlayerDefinition : ScriptableObject 
{
  [Header("Affectation")]
  [Range(1,2)]
  public int team = 1;

  [Header("Stats")]
  public ROLE role = ROLE.Defense;
  public int speed = 1;
  public int shootForce = 350;
  public int passForce = 200;
  public float lobForceFactor = 1f;
  public int attackForce = 40;

  [Header("Menu")]
  public Sprite avatar;
  public Sprite avatarMenu;
  public Sprite avatarMenuNb;
  public string shiityName;
  public string biography;
  public AudioClip sound;
}
