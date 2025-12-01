using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 5f;
    public float reachDistance = 0.1f;

    private Transform target;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-create points if missing
        if (pointA == null)
        {
            GameObject pA = new GameObject("PointA");
            pA.transform.position = transform.position + Vector3.left * 2f;
            pointA = pA.transform;
        }

        if (pointB == null)
        {
            GameObject pB = new GameObject("PointB");
            pB.transform.position = transform.position + Vector3.right * 2f;
            pointB = pB.transform;
        }

        target = pointB; // Start moving toward pointB
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        // Move toward the target without overshooting
        Vector2 newPos = Vector2.MoveTowards(rb.position, target.position, speed * Time.deltaTime);
        rb.MovePosition(newPos);

        // Switch target if we have reached or passed the point
        if (Vector2.Distance(rb.position, target.position) <= reachDistance)
        {
            Flip();
            target = (target == pointA) ? pointB : pointA;
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null) Gizmos.DrawWireSphere(pointA.position, 0.3f);
        if (pointB != null) Gizmos.DrawWireSphere(pointB.position, 0.3f);
        if (pointA != null && pointB != null) Gizmos.DrawLine(pointA.position, pointB.position);
    }




    public int damage = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with is the player
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            // Deal damage or kill the player
            player.TakeDamage(damage);
        }
    }

    // OR, if using a trigger instead:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}



