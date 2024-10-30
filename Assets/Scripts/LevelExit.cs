using UnityEngine;


public class LevelExit : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private GameObject blockade;
    private bool isExitAccessible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isExitAccessible = false;
    }
    public void UnlockExit()
    {
        isExitAccessible = true;
        blockade.SetActive(false);
        Debug.Log("The exit is now unlocked!");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        // Only log when the player collides with the "Vine" (X block)
        if (!isExitAccessible &&
            collision.gameObject.name == "Vine" &&
            collision.transform.root.gameObject == player)
        {
            Debug.Log("The exit is blocked! Destroy the space gem to unblock.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Collided with: " + collision.name);
        // Check if the object entering the trigger is the player
        if (collision.transform.root.gameObject == player && isExitAccessible) {
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        Debug.Log("Congratulations! You've completed the level.");
    }

}
