using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBossBehavior: MonoBehaviour
{

  public bool isDefeated = false;
  private void Start()
  {
    
  }

  private void Update()
  {
    
  }

  public virtual void StartAction() { }
  public virtual void StopAction() { }
}
