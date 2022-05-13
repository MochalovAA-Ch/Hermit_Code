using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundManager : MonoBehaviour
{
  public AudioSource bgAudioSource;
  public AudioSource effectsAudioSource;
  public static MainMenuSoundManager instance;
  public AudioClip btnClickSound;
  public AudioClip backgroundMusicSound;
  // Start is called before the first frame update
  void Start()
  {
    instance = this;
        
  }

  // Update is called once per frame
  void Update()
  {
    if (!GameSystem.isMusicEnabled)
    {
      bgAudioSource.Stop();
    }
    else
    {
      if( !bgAudioSource.isPlaying )
      {
        bgAudioSource.clip = backgroundMusicSound;
        bgAudioSource.Play();
      }
    }
  }

  public void ButtonClickSound()
  {
    if (!GameSystem.isSoundEnabled)
      return;
    effectsAudioSource.clip = btnClickSound;
    effectsAudioSource.Play();
  }
}
