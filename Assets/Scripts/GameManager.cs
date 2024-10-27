using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {/*
        float camXPos = gridManager.width / 2 - 0.5f;
        float camYPos = gridManager.height / 2 - 0.5f;
        cam.transform.position = new Vector3(camXPos, camYPos, -10);
    */}

    // Update is called once per frame
    void Update()
    {
        
    }
}
