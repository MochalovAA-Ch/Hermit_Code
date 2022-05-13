using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour {

  Vector3[] vercticles;
  Vector3[] defaultVerticles;
  Mesh mesh;
	// Use this for initialization
	void Start () {

    mesh = GetComponent<MeshFilter>().mesh;
    //vercticles = mesh.vertices;
    defaultVerticles = mesh.vertices;
  }

  float changeMeshTimer = 3.0f;
  float currChangeMeshTime;


  IEnumerator tstWaves()
  {
    for (int i = 0; i < vercticles.Length; i++)
    {
      Vector3 verticle = defaultVerticles[i];
      verticle.z = defaultVerticles[i].z + Random.Range(-0.03f, 0.03f);
      vercticles[i] = verticle;
      mesh.vertices = vercticles;
      mesh.RecalculateNormals();
      yield return null;
    }
    StartCoroutine("tstWaves");
  }
  // Update is called once per frame
  void Update ()
  {
    if (vercticles == null)
    {
      vercticles = new Vector3[defaultVerticles.Length];
    }

    if( Input.GetKeyDown(KeyCode.P) )
    {
      StartCoroutine("tstWaves");
    }
    /*
    if( currChangeMeshTime < changeMeshTimer )
    {
      currChangeMeshTime += Time.deltaTime;
    }
    else
    {
      for (int i = 0; i < vercticles.Length; i++)
      {
        Vector3 verticle = defaultVerticles[i];
        verticle.z = defaultVerticles[i].z + Random.Range(-0.03f, 0.03f);
        vercticles[i] = verticle;
      }
      currChangeMeshTime = 0.0f;
      mesh.vertices = vercticles;
      mesh.RecalculateNormals();
    }*/

  }
}
