using UnityEngine;

public class SpaceGem : MonoBehaviour
{
    public static event System.Action OnSpaceGemDestroy = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DestroyGem();
        }
        audioManager.PlaySFX(audioManager.gemBreak); 
    }

    public void DestroyGem()
    {
        // Deactivate the gem object
        gameObject.SetActive(false);
        OnSpaceGemDestroy();

        Debug.Log("Gem destroyed! Exit is now accessible.");
    }

AudioManager audioManager;
private void Awake()
{
    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

}
}
