using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask[] moveableLayers;
    [SerializeField] private LayerMask[] unnmoveableLayers;

    public static Player instance;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool critterMode = false;
    private const int CENTIPEDE = 0;
    private const int WATERBUG = 1;
    private const int FLY = 2;

    [SerializeField] private int[] numCritters = new int[3];
    [SerializeField] private GameObject[] critters = new GameObject[3];
    private int activeCritterIdx;

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
                    
                    PlaceCritter(activeCritterIdx, offsetPos, rotation);
                    numCritters[activeCritterIdx]--;
                }
                else if (IsMoveableTile(offsetPos))
                {
                    movePoint = offsetPos;
                }
            }

            if (Input.GetButtonDown("PlaceCentipede") && numCritters[CENTIPEDE] > 0)
            {
                ToggleCritterMode(CENTIPEDE);
            }
            else if (Input.GetButtonDown("PlaceWaterBug") && numCritters[WATERBUG] > 0)
            {
                ToggleCritterMode(WATERBUG);
            }
            else if (Input.GetButtonDown("PlaceFly") && numCritters[FLY] > 0)
            {
                ToggleCritterMode(FLY);
            }
        }
    }

    private void ToggleCritterMode(int critter)
    {
        critterMode = !critterMode;
        activeCritterIdx = critter;

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
    private void PlaceCritter(int critterIdx, Vector3 position, int rotation)
    {
        GameObject spawnedCritter = Instantiate(critters[critterIdx], position, Quaternion.identity);
        spawnedCritter.transform.Rotate(0, 0, rotation);
        ToggleCritterMode(critterIdx);
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

        //return (!Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Water")) || Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("WaterBug"))) && (!Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole"))
          //  || Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Centipede")));
    }

    // Checks if position is in world bounds AKA on ground tile
    private bool IsInBounds(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Ground"));
    }

    // Checks if a ONE-TILE position is a valid critter placement
    // NOTE: Does not check for multi-tile placements (ahem, Centipede, ahem). How do we address that?
    private bool IsPlaceableTile(Vector3 position)
    {
        // All critters can be placed on ground, mud, covered holes, and covered water.
        // Critters CANNOT be placed out-of-bounds
        // Critters placed on holes or water will normally fall or drown, respectively.
        // The Water Bug can be placed on water.
        // The Centipede can be placed on holes.
        // The Fly can be placed on water or holes, it flies over everything.
        bool isGround = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Ground"));
        bool isMud = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Mud"));
        bool isHole = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole"));
        bool isWater = 
    }
}
