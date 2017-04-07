using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public GameObject tilePretabs;

	private Transform playerTransform;
	private float spawnZ = 0.0f;
	private float tileLength = 640f;
	private int amnTileOnScreen = 3;
	private List<GameObject> tilePools = new List<GameObject> ();
	// Use this for initialization
	private void Start ()
	{
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		for (int i = 0; i < amnTileOnScreen; i++) {
			SpawnTileGameObject ();
		}

	}
	
	// Update is called once per frame
	private void Update ()
	{
		if (playerTransform.position.z > (spawnZ - amnTileOnScreen * tileLength - 40)) {
			SpawnTileGameObject ();
		}
	}

	private void SpawnTileGameObject ()
	{
		GameObject tile = GetTile ();
		tile.transform.position = Vector3.forward * spawnZ;
		spawnZ += tileLength;
		tile.SetActive (true);
	}

	private GameObject GetTile ()
	{
		GameObject tileGo = tilePools.Find (x => !x.activeSelf);
		if (tileGo == null) {
			tileGo = Instantiate (tilePretabs, Vector3.zero, Quaternion.identity)as GameObject;
			tileGo.transform.SetParent (transform);
			tilePools.Add (tileGo);
		}
		RandomPertabIndex (tileGo);
		return tileGo;
	}

	private void RandomPertabIndex (GameObject tileGo)
	{
		Transform tfTrain = tileGo.transform.GetChild (0);
		foreach (Transform train in tfTrain) {
			if (!train.gameObject.activeSelf)
				train.gameObject.SetActive (true);
			if (Random.Range (0, 5) > 3) {
				train.gameObject.SetActive (false);
			}
		}
	}
}

