using UnityEngine;

public class SpaceGem : MonoBehaviour
{
    public static event System.Action OnSpaceGemDestroy = delegate { };

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DestroyGem();
        }
    }

    public void DestroyGem()
    {
        // Deactivate the gem object
        gameObject.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.instance.gemBreak);
        OnSpaceGemDestroy();

        Debug.Log("Gem destroyed! Exit is now accessible.");
    }
}
