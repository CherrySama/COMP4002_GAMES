using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] float speed;

    int i = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rideable>().isRide)
        {
            speed = 3.0f;
        }
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position,
                                                     points[i].position,
                                                     speed * Time.deltaTime);
    }
}
