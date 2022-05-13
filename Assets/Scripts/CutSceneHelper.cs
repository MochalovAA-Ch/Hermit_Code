using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneHelper : MonoBehaviour
{
  public TutorialManager tutoriaManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }


  private void OnDisable()
  {
    tutoriaManager.ShouldPlayHint = true;
  }

  // Update is called once per frame
  void Update()
    {
        
    }

}
