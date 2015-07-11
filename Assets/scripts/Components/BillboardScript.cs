using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour 
{
  public float xAngle = 270;

	void Update () 
  {
    Vector3 rot = Camera.main.transform.rotation.eulerAngles;
    rot.x = xAngle;
    rot.z = 0;
    transform.rotation = Quaternion.Euler(rot);
	}
}
