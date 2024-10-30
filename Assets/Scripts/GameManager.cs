using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Transform cam;
    [SerializeField] private Tilemap levelGround;

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
        BoundsInt groundBounds = levelGround.cellBounds;
        float xCam = (groundBounds.xMax - groundBounds.xMin) / 2;
        float yCam = (groundBounds.yMax - groundBounds.yMin) / 2;
        cam.transform.position = new Vector3(xCam, yCam, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
