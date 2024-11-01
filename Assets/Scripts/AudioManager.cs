using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")] 
    public AudioClip footstepsDirt;
    public AudioClip gemBreak;

    private void Awake()
    {
        // Singleton enforcement
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
