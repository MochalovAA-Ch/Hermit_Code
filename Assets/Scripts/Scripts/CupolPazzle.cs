using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupolPazzle : MonoBehaviour {

  public Transform cameraPos;
  AudioSource audioSource;
  public AudioClip cupolOpened;
  Collider cupolCollider;
  public Material material;
  public uint cupolButtonsCount;
  uint cupolButtonsPressedCount;
  public ParticleSystem particlesCupola;

  // Use this for initialization
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    audioSource.clip = cupolOpened;
    cupolCollider = GetComponent<Collider>();
    material.color = new Color(material.color.r, material.color.g, material.color.b, 0.856f);
    //material = GetComponent<Material>();
  }

  private void OnEnable()
  {
    EventsManager.StartListening(EventsIds.CUPOL_BUTTON_BOX_ENTER, ButtonPressed);
    EventsManager.StartListening(EventsIds.CUPOL_BUTTON_BOX_EXIT, ButtonUnpressed);
  }

  private void OnDisable()
  {
    EventsManager.StopListening(EventsIds.CUPOL_BUTTON_BOX_ENTER, ButtonPressed);
    EventsManager.StopListening(EventsIds.CUPOL_BUTTON_BOX_EXIT, ButtonUnpressed);
    material.color = new Color(material.color.r, material.color.g, material.color.b, 0.856f);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.R))
    {
      material.color = new Color(material.color.r, material.color.g, material.color.b, 0.02f);
    }
  }

  void ButtonPressed()
  {
    cupolOpenTimer = 0.0f;
    cupolButtonsPressedCount++;
    audioSource.Play();
    materialColorAlpha = material.color.a * 0.5f;
    particlesCupola.Play();
    if (cupolButtonsPressedCount == cupolButtonsCount)
    {
      materialColorAlpha = 0.0f;
    }
    material.color = new Color(material.color.r, material.color.g, material.color.b, materialColorAlpha );
    ThirdPersonOrbitCam.instance.SetHintCameraState( cameraPos );
    CharacterControllerScript.instance.SetInactivePlayerState();
    StartCoroutine("showCupolOpenedAnim");
  }

  void ButtonUnpressed()
  {
    cupolButtonsPressedCount--;
    if (cupolButtonsPressedCount == 1)
    {
      material.color = new Color(material.color.r, material.color.g, material.color.b, 0.856f * 0.5f);
    }
    else
    {
      material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a * 2f);
    }
    cupolCollider.enabled = true;
  }

  float cupolOpenTimer = 0.0f;
  float materialColorAlpha;
  float matCurrAlpha;
  IEnumerator showCupolOpenedAnim()
  {
    while (cupolOpenTimer  < 2.5f  )
    {
      cupolOpenTimer += Time.deltaTime;
      yield return null;
    }

    ThirdPersonOrbitCam.instance.SetDefaultCameraState();
    CharacterControllerScript.instance.SetDefaulPlayertState();
    if (cupolButtonsPressedCount == cupolButtonsCount)
    {
      cupolCollider.enabled = false;
      Destroy(gameObject);
    }
  }
}
