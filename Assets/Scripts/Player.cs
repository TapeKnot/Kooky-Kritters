using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform movePoint;
    [SerializeField] private Transform lookPoint;
    [SerializeField] private LayerMask wallCollider;
    [SerializeField] private LayerMask obstacleCollider;
    [SerializeField] private LayerMask centipedeCollider;
    [SerializeField] private bool critterMode = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject centipede;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            Vector3 horizontalOffset = new(moveHorizontal, 0f, 0f);
            float moveVertical = Input.GetAxisRaw("Vertical");
            Vector3 verticalOffset = new(0f, moveVertical, 0f);

            if (Mathf.Abs(moveHorizontal) == 1f && !Physics2D.OverlapCircle(movePoint.position + horizontalOffset, 0.2f, wallCollider))
            {
                // Want to prevent movePoint from going onto obstacle tiles UNLESS there is also a centipede tile there OR Critter Mode is on
                if (!Physics2D.OverlapCircle(movePoint.position + horizontalOffset, 0.2f, obstacleCollider))
                {
                    movePoint.position += horizontalOffset;
                }
                else if (Physics2D.OverlapCircle(movePoint.position + horizontalOffset, 0.2f, centipedeCollider) || critterMode)
                {
                    movePoint.position += horizontalOffset;
                }

                if (critterMode)
                {
                    int rotation;
                    if (moveHorizontal > 0f)
                    {
                        rotation = 270;
                    }
                    else
                    {
                        rotation = 90;
                    }

                    PlaceCentipede(movePoint.position, rotation);
                    movePoint.position -= horizontalOffset; // Do not move player
                }
            }
            else if (Mathf.Abs(moveVertical) == 1f && !Physics2D.OverlapCircle(movePoint.position + verticalOffset, 0.2f, wallCollider))
            {
                // Want to prevent movePoint from going onto obstacle tiles UNLESS there is also a centipede tile there OR Critter Mode is on
                if (!Physics2D.OverlapCircle(movePoint.position + verticalOffset, 0.2f, obstacleCollider))
                {
                    movePoint.position += verticalOffset;
                }
                else if (Physics2D.OverlapCircle(movePoint.position + verticalOffset, 0.2f, centipedeCollider) || critterMode)
                {
                    movePoint.position += verticalOffset;
                }

                if (critterMode)
                {
                    int rotation;
                    if (moveVertical > 0f)
                    {
                        rotation = 0;
                    }
                    else
                    {
                        rotation = 180;
                    }

                    PlaceCentipede(movePoint.position, rotation);
                    movePoint.position -= verticalOffset; // Do not move player
                }
            }

            if (Input.GetButtonDown("Critter Place Mode"))
            {
                ToggleCritterMode();
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

    // Place Centipede at given position and rotation, then toggle Critter Mode
    private void PlaceCentipede(Vector3 position, int rotation)
    {
        GameObject spawnedCritter = Instantiate(centipede, position, Quaternion.identity);
        spawnedCritter.transform.Rotate(0, 0, rotation);
        ToggleCritterMode();
    }
}
