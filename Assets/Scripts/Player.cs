using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask[] moveableLayers;
    [SerializeField] private LayerMask[] unnmoveableLayers;

    public static Player instance;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool critterMode = false;
    [SerializeField] private enum Critter {Centipede, WaterBug, Fly};
    [SerializeField] private Critter active;

    [SerializeField] private int numCentipedes = 3;
    [SerializeField] private int numWaterBugs = 3;
    [SerializeField] private int numFlies = 3;
    [SerializeField] private GameObject centipede;
    [SerializeField] private GameObject waterBug;
    [SerializeField] private GameObject fly;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3 movePoint;
    

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

        movePoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint) == 0f)
        {
            Vector3 offset = new();
            offset.x += Input.GetAxisRaw("Horizontal");
            if (offset.magnitude == 0)
            {
                offset.y += Input.GetAxisRaw("Vertical");
            }

            Vector3 offsetPos = movePoint + offset;

            if (offset.magnitude == 1f && IsMoveableTile(offsetPos))
            {
                movePoint = offsetPos;
            }



            {
                if (critterMode)
                {
                    int rotation = 0;
                    if (offset.x > 0)
                    {
                        rotation = 270;
                    }
                    else if (offset.x < 0)
                    {
                        rotation = 90;
                    }
                    else if (offset.y < 0)
                    {
                        rotation = 180;
                    }
                    
                    switch (active) {
                        case Critter.Centipede:
                            PlaceCritter(centipede, offsetPos, rotation);
                    }
                    PlaceCritter(activeCritter, offsetPos, rotation);
                    numCritters[activeCritterIdx]--;
                }
                else if (IsMoveableTile(offsetPos))
                {
                    movePoint = offsetPos;
                }
            }

            if (Input.GetButtonDown("PlaceCentipede") && numCentipedes > 0)
            {
                ToggleCritterMode();
                active = Critter.Centipede;
            }
            else if (Input.GetButtonDown("PlaceWaterBug") && numWaterBugs > 0)
            {
                ToggleCritterMode();
                active = Critter.WaterBug;
            }
            else if (Input.GetButtonDown("PlaceFly") && numFlies > 0)
            {
                ToggleCritterMode();
                active = Critter.Fly;
            }
        }
    }

    private void ToggleCritterMode()
    {
        critterMode = !critterMode;

        if (critterMode)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.cyan;
        }
    }

    // Place Critter at given position and rotation, then toggle Critter Mode
    private void PlaceCritter(Critter critter, Vector3 position, int rotation)
    {
        GameObject spawnedCritter = null;
        switch (critter)
        {
            case Critter.Centipede:
                spawnedCritter = Instantiate(centipede, position, Quaternion.identity);
                break;
            case Critter.WaterBug:
                spawnedCritter = Instantiate(waterBug, position, Quaternion.identity);
                break;
            case Critter.Fly:
                spawnedCritter = Instantiate(fly, position, Quaternion.identity);
                break;
        }
        spawnedCritter.transform.Rotate(0, 0, rotation);
        ToggleCritterMode();
    }

    // Checks if position is on a moveable tile
    private bool IsMoveableTile(Vector3 position)
    {
        // For movement:
        // Player can move on ground, mud, holes (if centipede is on top), and water (if water bug is on top)
        // Player CANNOT move on out-of-bounds, holes, and water
        // Centipedes CANNOT be placed on water or out-of-bounds
        // Water Bugs CANNOT be placed on holes or out-of-bounds
        // Both can be placed on mud to negate its effects
        bool isGround = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Ground"));
        bool isMud = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Mud"));
        bool isCoveredHole = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Centipede"));
        bool isCoveredWater = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Water")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("WaterBug"));
        return isGround || isMud || isCoveredHole || isCoveredWater;
        // But wait, this means that as long as a ground tile is there, anything is moveable...make a note that each grid space should have at most 1 tile layer normally.
    }

    // Checks if a ONE-TILE position is a valid critter placement
    // NOTE: Does not check for multi-tile placements (ahem, Centipede, ahem). How do we address that?
    private bool IsPlaceableTile(Vector3 position, GameObject critter)
    {
        // All critters can be placed on ground, mud, covered holes, and covered water.
        // Critters CANNOT be placed out-of-bounds
        // Critters placed on holes or water will normally fall or drown, respectively.
        // The Water Bug can be placed on water.
        // The Centipede can be placed on holes.
        // The Fly can be placed on water or holes, it flies over everything.
        bool isGround = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Ground"));
        bool isMud = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Mud"));
        bool isCoveredHole = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Centipede"));
        bool isCoveredWater = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Water")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("WaterBug"));

        bool isHole = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole"));
        bool isWater = 1;

        bool canTryPlace = isGround || isMud || isCoveredHole || isCoveredWater;

        // Critter-specific cases
        if ()

        return 
    }
}
