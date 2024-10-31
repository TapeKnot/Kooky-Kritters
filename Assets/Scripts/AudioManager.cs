using UnityEngine;

public class AudioManager : MonoBehaviour
{
  [Header("---------- Audio Source ----------")]
[SerializeField] AudioSource SFXSource;

[Header("---------- Audio Clip ----------")] 
public AudioClip footstepsDirt;
public AudioClip gemBreak;


 public void PlaySFX(AudioClip clip)
 {
    SFXSource.PlayOneShot(clip);
 }






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
