using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelExit : MonoBehaviour
{
    public static LevelExit instance;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private string nextLevelName;
    private bool unlocked = false;

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

        SpaceGem.OnSpaceGemDestroy += UnlockExit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only log when the player collides with the "Vine" (X block)
        if (collision.gameObject.CompareTag("Player") && !unlocked)
        {
            Debug.Log("The exit is blocked! Destroy the space gem to unblock.");
        }

        if (collision.gameObject.CompareTag("Player") && unlocked) {
            LevelComplete();
        }
    }

    public void UnlockExit()
    {
        unlocked = true;
        sprite.sprite = openSprite;
        Debug.Log("The exit is now unlocked!");
    }

    private void LevelComplete()
    {
        SceneManager.LoadSceneAsync(nextLevelName);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SpaceGem.OnSpaceGemDestroy -= UnlockExit;
    }

}
