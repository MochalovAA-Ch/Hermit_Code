using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrelEfectScript : MonoBehaviour {

  public ParticleSystem partsSys;
  public int partsCount;
  ParticleSystem.Particle[] parts;
  public GameObject coin;

  public float liveTime;
  float currLifeTime;
  public List<GameObject> coins;
	// Use this for initialization
	void Start () {
    //particleSystem = GetComponent<ParticleSystem>();
    //coin = Resources.Load("coin5") as GameObject;
  }

  private void OnEnable()
  {
    if( parts == null  )
      parts = new ParticleSystem.Particle[partsCount];
    partsSys.Play();
  }

  // Update is called once per frame
  void Update ()
  {
		if( currLifeTime < liveTime )
    {
      currLifeTime += Time.deltaTime;
      int particlesCount = partsSys.GetParticles(parts);
    }
    else
    {
      float angle = 360.0f / 5;
      float currAngle = 0.0f;
      float radius = 1.0f;
      currLifeTime = 0.0f;
      int count = partsSys.GetParticles(parts);
      for (int i = 0; i < 5; i++)
      {
        float x = radius * Mathf.Sin(currAngle * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(currAngle * Mathf.Deg2Rad);
        GameObject tmpCoin = Instantiate(coin, transform.position + parts[i].position + Vector3.up, Quaternion.Euler( 0.0f, parts[i].rotation, 0.0f ) );
        currAngle += angle;
        SceneObjectsAnimator.instance.coinsList.Add(tmpCoin.transform);
      }
      this.gameObject.SetActive(false);
    }
	}
}
