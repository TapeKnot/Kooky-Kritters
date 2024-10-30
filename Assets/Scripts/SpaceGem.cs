using UnityEngine;

public class SpaceGem : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private LevelExit exit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

        Debug.Log("Gem destroyed! Exit is now accessible.");
        Debug.Log("Unlocking exit...");

        exit.UnlockExit();
    }

}
