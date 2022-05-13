using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsAnimator : MonoBehaviour {

  public static SceneObjectsAnimator instance;
  public float rotationSpeed = 1.0f;
  public List<Transform> coinsList;

  int coinsListLength = 0;
  int coinIndex = 0;
	// Use this for initialization
	void Start () {
    instance = this;
    //GameSystem.coinsOnLevel = 0;
    //GameSystem.coinsOnLevel = coinsList.Count;
    GameObject[] coinsGO = GameObject.FindGameObjectsWithTag( "Coin" );
    for ( int i = 0; i < coinsGO.Length; i++ )
    {
      coinsList.Add(coinsGO[i].transform);
    }
    //coinsListLength = coinsList.Count;
  }
	
	// Update is called once per frame
	void Update ()
  {
    for( int i = 0; i < coinsList.Count; i++ )
    {
      if( coinsList[i] == null )
        continue;
      coinsList[i].Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
    }
	}
}
