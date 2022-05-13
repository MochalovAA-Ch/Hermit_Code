using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

  public static SceneLoader instance;

  private void Awake()
  {
    instance = this;
  }

  public  void LoadLevel( int levelNumber )
  {
    SceneManager.LoadScene( levelNumber, LoadSceneMode.Single );
  }

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
