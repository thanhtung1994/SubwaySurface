using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
	private Transform playerTranform;
	// Use this for initialization
	void Start () {
		playerTranform = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerTranform.position.z > transform.position.z - 650)
			gameObject.SetActive (false);
	}
}
