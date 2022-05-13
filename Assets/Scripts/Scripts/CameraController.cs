using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

  //Компонент камеры
  Camera cam;
  //Компонент трансформ
  Transform cameraTransform;

  //Объект главного героя
  public GameObject player;
  //Смещение камеры относительно персонажа по умолчанию 
  public Vector3 cammOffset;
  //Смещение точки относительно персонажа, в которую смотрит камера
  public Vector3 pivotPoint;

  //Смещение позиции камеры при движении курсора
  Vector3 offsetFromCursor;

  //Стартовая позиция курсора, при которой начали смещать камеру
  Vector3 startCursorPosition;
  //Вектор разницы между начальной позицией курсора и текущей
  Vector3 deltaCursorPos;

  float angleX;
  float angleY;

  // Use this for initialization
  void Start ()
  {
    cameraTransform = GetComponent<Transform>();
    cam = GetComponent<Camera>();
	}
	

	// Update is called once per frame
	void Update ()
  {
    /* if (Input.GetMouseButton(0))
     {
       angleX += //Input.GetAxis("Mouse X") * 10.0f;
       angleY += //Input.GetAxis("Mouse Y") * 10.0f;
     }*/
    //angleX += CameraLook.AngleX;
    //angleY += CameraLook.AngleY;
    //Debug.Log(angleX + " " + angleY);

    offsetFromCursor = -player.transform.forward;
    //offsetFromCursor = Quaternion.AngleAxis(angleX, Vector3.up ) * offsetFromCursor;
    offsetFromCursor = Quaternion.Euler( angleY, angleX, 0.0f ) * offsetFromCursor;

    //Debug.DrawRay(offsetFromCursor, player.transform.position);
    //offsetFromCursor = player.transform.forward + offsetFromCursor;
  }

  void LateUpdate()
  {
    //Позиция камеры
  //  cameraTransform.position = player.transform.position - player.transform.forward*cammOffset.z + pivotPoint * cammOffset.y + offsetFromCursor * cammOffset.z ;

    //Камера смотрит на точку вращения вокруг персонажа
    cameraTransform.LookAt(player.transform.position + pivotPoint);
  }
}
