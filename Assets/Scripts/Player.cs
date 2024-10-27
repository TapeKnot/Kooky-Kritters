using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform movePoint;
    public LayerMask moveCollider;

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
            float moveVertical = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(moveHorizontal) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(moveHorizontal, 0f, 0f), 0.2f, moveCollider))
                {
                    movePoint.position += new Vector3(moveHorizontal, 0f, 0f);
                }
                
            }

            else if (Mathf.Abs(moveVertical) == 1f)
            {

                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, moveVertical, 0f), 0.2f, moveCollider))
                {
                    movePoint.position += new Vector3(0f, moveVertical, 0f);
                }
            }
        }
    }
}
