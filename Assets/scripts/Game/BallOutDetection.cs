using UnityEngine;
using System.Collections;

public class BallOutDetection : MonoBehaviour {

	public int teamSide=0;
 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.tag != "Ball")
			return;
    var ball = other.GetComponent<BallScript> ();
		if (teamSide == 0) {
			Debug.Log ("C'est la touche");
      GameScript gs = GetComponent<GameScript>();

//      gs.setLineOutSituation(Vector3.zero,(ball.lastTeamTouch==1)?2:1);
		}
    else if (teamSide == ball.lastTeamTouch) {
      Debug.Log ("C'est le corner !!");
    }
    else{
      Debug.Log ("C'est le 6m !");
    }


	}
}
