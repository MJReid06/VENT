using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    public Transform PosA, PosB;
    public float Speed = 2f;

    private Vector3 targetPos;

    void Start()
    {
        targetPos = PosB.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, PosA.position) < 0.1f)
            targetPos = PosB.position;

        if (Vector3.Distance(transform.position, PosB.position) < 0.1f)
            targetPos = PosA.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}