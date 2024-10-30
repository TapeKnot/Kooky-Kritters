using UnityEngine;

public class SpaceGem : MonoBehaviour
{
    public GameObject player;
    public GameObject vineBlock;
    [SerializeField] private LevelExit levelExit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (vineBlock != null)
        {
            vineBlock.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Collided with: " + collision.name);
        // Check if the object entering the trigger is the player
        if (collision.transform.root.gameObject == player)
        {
            DestroyGem();
        }
    }

    public void DestroyGem()
    {
        // Deactivate the gem object
        gameObject.SetActive(false);

        // Remove the vine block (X)
        if (vineBlock != null)
        {
            vineBlock.SetActive(false);
            Debug.Log("Gem destroyed! Exit is now accessible.");
        }

        if (levelExit != null)
        {   
            Debug.Log("Unlocking exit...");
            levelExit.UnlockExit();
        }
    }

}
