using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Player player;
    [SerializeField] private Tilemap levelGround;

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
