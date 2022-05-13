using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTiling : MonoBehaviour {

  public Material streamWaterMaterial;
  public float offsetSpeed;
  public Vector2 defaultTextureOffset;

	// Use this for initialization
	void Start ()
  {

  }
	
	// Update is called once per frame
	void Update ()
  {
  /*  if ( streamWaterMaterial.mainTextureOffset.y + offsetSpeed*Time.deltaTime >= 100.0f )
    {
      Debug.Log( "Больше 100" );
      streamWaterMaterial.SetTextureOffset( "_MainTex", new Vector2( defaultTextureOffset.x, defaultTextureOffset.y ) );
    }
    else*/
  //  {
      streamWaterMaterial.SetTextureOffset("_MainTex", new Vector2(defaultTextureOffset.x, streamWaterMaterial.mainTextureOffset.y + offsetSpeed*Time.deltaTime) );
   // }
	}
}
