using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour 
{
  private Vector3 startPosition;

  void Awake()
  {
    startPosition = this.transform.position;
  }

  public void Reset()
  {
    this.transform.position = startPosition;
  }
}
