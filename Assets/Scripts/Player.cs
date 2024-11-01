using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask[] moveableLayers;
    [SerializeField] private LayerMask[] unnmoveableLayers;

    public static Player instance;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool critterMode = false;

    private bool sliding = false;
    private Vector3 slideVector;

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

            if (sliding && IsMoveableTile(movePoint + slideVector))
            {
                movePoint += slideVector;
                // TODO: Play slide audio
            }
            else if (offset.magnitude == 1f && critterMode && IsPlaceableTile(offsetPos))
            {
                PlaceCritter(active, offsetPos, offset);
            }
            else if (offset.magnitude == 1f && IsMoveableTile(offsetPos))
            {
                if (sliding) slideVector = offset;
                movePoint = offsetPos;
                // TODO: Play footstep audio
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
    private void PlaceCritter(Critter critter, Vector3 position, Vector3 offset)
    {
        GameObject spawnedCritter = null;
        switch (critter)
        {
            case Critter.Centipede:
                spawnedCritter = Instantiate(centipede, position, Quaternion.identity);
                // TODO: Play Centipede spawn audio
                numCentipedes--;
                break;
            case Critter.WaterBug:
                spawnedCritter = Instantiate(waterBug, position, Quaternion.identity);
                // TODO: Play Water Bug spawn audio
                numWaterBugs--;
                break;
            case Critter.Fly:
                spawnedCritter = Instantiate(fly, position, Quaternion.identity);
                // TODO: Play Fly spawn audio
                numFlies--;
                break;
        }

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
        spawnedCritter.transform.Rotate(0, 0, rotation);

        ToggleCritterMode();
    }

    // Checks if position is on a moveable tile
    private bool IsMoveableTile(Vector3 position)
    {
        // Player can move on ground, mud, holes (if centipede is on top), and water (if water bug is on top)
        // In other words, player can move anywhere where a centipede or water bug is.
        // Player CANNOT move on out-of-bounds, holes, and water
        bool isGround = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Ground"));
        bool isMud = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Mud"));
        bool isCoveredMud = isMud && (Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Centipede")) || Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("WaterBug")));
        bool isCoveredHole = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Centipede"));
        bool isCoveredWater = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Water")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("WaterBug"));

        if (isMud && !isCoveredMud)
        {
            sliding = true;
            moveSpeed = 7f;
        }
        else
        {
            sliding = false;
            moveSpeed = 5f;
        }

        return isGround || isMud || isCoveredHole || isCoveredWater;
        // As long as a ground tile is there, anything is moveable...make a note that each grid space should have at most 1 tile layer normally.
    }

    // Checks if a ONE-TILE position is a valid critter placement
    // NOTE: Does not check for multi-tile placements (ahem, Centipede, ahem). Needs to be addressed.
    private bool IsPlaceableTile(Vector3 position)
    {
        // Cases:
        // 1. Ground (placeable)
        // 2. Mud (placeable)
        // 3. Out-of-bounds (not placeable)
        // 4. Hole (only Centipede or Fly placeable)
        // 5. Water (only Water Bug or Fly placeable)
        // 6. Covered hole (placeable)
        // 7. Covered water (placeable)
        bool isGround = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Ground"));
        bool isMud = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Mud"));
        bool isHole = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole"));
        bool isWater = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Water"));
        bool isCoveredHole = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Hole")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Centipede"));
        bool isCoveredWater = Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("Water")) && Physics2D.OverlapCircle(position, 0.2f, LayerMask.GetMask("WaterBug"));

        bool isMoveable = isGround || isMud || isCoveredHole || isCoveredWater;

        bool holePlaceableCritter = isHole && (active == Critter.Centipede || active == Critter.Fly);
        bool waterPlaceableCritter = isWater && (active == Critter.WaterBug || active == Critter.Fly);

        return isMoveable || holePlaceableCritter || waterPlaceableCritter;
    }
}
