using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckActiveScript : MonoBehaviour {

	private void OnTriggerEnter(Collider coll){
			StartCoroutine (SetActive ());
	}
	private IEnumerator SetActive(){
		yield return new WaitForSeconds (3);
		transform.parent.gameObject.SetActive (false);
	}

}
