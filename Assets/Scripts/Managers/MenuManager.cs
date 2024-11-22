using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        Vector3 bounds = levelGround.cellBounds.center;
        cam.transform.position = new Vector3(bounds.x, bounds.y, -10);
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadSceneAsync(nextLevel);
        }
        if (Input.GetButtonDown("Exit"))
        {
            Application.Quit();
        }
    }
}
