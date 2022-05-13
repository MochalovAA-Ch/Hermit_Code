using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

  public InputInfo Info;

  public bool hitBtnPressed;
  public bool jumpBtnPressed;
  public bool homyBtnPressed;
  public bool rolloverBtnPressed;

  public bool homyAimBtnPressed;
  public bool interactBtnPressed;

  // Use this for initialization
  void Start () {
    Info = new InputInfo();
  }

  // Update is called once per frame
  void Update()
  {
    //Info.Clear();
    ClearValues();
    if (true/*!CameraSettings.instance.withJoystick*/)
    {
      if (Input.GetKeyDown(KeyCode.Space))
        Info.jumpInput = true;

      if (Input.GetKeyDown(KeyCode.F))
        Info.hitInput = true;

      if (Input.GetKeyDown(KeyCode.G))
        Info.homyLaunchInput = true;

      if( Input.GetKeyDown( KeyCode.U ) )
      {
        Info.homyAimInput = true;
      }

      if( Input.GetKeyDown( KeyCode.V) )
      {
        Info.rolloverInput = true;
      }
    }
  }

  void ClearValues()
  {
    if( !jumpBtnPressed )
    {
      Info.jumpInput = false;
    }

    if( !hitBtnPressed )
    {
      Info.hitInput = false;
    }

    if (!homyBtnPressed)
    {
      Info.homyLaunchInput = false;
    }

    if( !rolloverBtnPressed  )
    {
      Info.rolloverInput = false;
    }

    if( !homyAimBtnPressed )
    {
      Info.homyAimInput = false;
    }

    if( !interactBtnPressed )
    {
      Info.interactInput = false;
    }

    jumpBtnPressed = false;
    hitBtnPressed = false;
    homyBtnPressed = false;
    rolloverBtnPressed = false;
    homyAimBtnPressed = false;
    interactBtnPressed = false;
  }
}
public struct InputInfo
{
  public bool jumpInput;
  public bool hitInput;
  public bool homyLaunchInput;
  public bool rolloverInput;
  public bool homyAimInput;
  public bool interactInput;
}
