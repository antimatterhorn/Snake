using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bodyPrefab;
    public GameObject tailPrefab;
    public float speed = 0.1f;
    public float stride = 0.2f;
    public GameObject gameController;

    private List<GameObject> bodies;
    private Rigidbody2D rb;
    private Vector2 prevDirection;
    private Vector2 direction;
    private float updateTime = 0f;

    private bool justAte = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.left;
        prevDirection = direction;

        bodies = new List<GameObject>();
        Vector3 startBody = new Vector3(transform.position.x + stride, transform.position.y, transform.position.z);
        bodies.Insert(0, Instantiate(tailPrefab, startBody, Quaternion.identity, gameController.transform));
        bodies[0].GetComponent<BodyController>().direction = direction;
        bodies[0].GetComponent<BodyController>().isTail = true;
    }

    void ArrowKeyMovement()
    {

        if ((Input.GetKey("up")) && (prevDirection != Vector2.down))
        {
            direction = Vector2.up;
        }
        else if ((Input.GetKey("down")) && (prevDirection != Vector2.up))
        {
            direction = Vector2.down;
        }
        else if ((Input.GetKey("left")) && (prevDirection != Vector2.right))
        {
            direction = Vector2.left;
        }
        else if ((Input.GetKey("right")) && (prevDirection != Vector2.left))
        {
            direction = Vector2.right;
        }

    }

    // Update is called once per frame
    void Update()
    {
        ArrowKeyMovement();
        if (Time.time > updateTime)
        {
            Vector3 oldPos = transform.position;
            transform.position += new Vector3(direction.x,direction.y,0f)*stride;
            if (direction != prevDirection)
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Atan2(-direction.y, -direction.x)*180f/Mathf.PI));
            updateTime = Time.time + 1f / speed;

            if (justAte)
            {
                bodies.Insert(0, Instantiate(bodyPrefab, oldPos, Quaternion.identity, gameController.transform));
                bodies[0].GetComponent<BodyController>().direction = direction;
                justAte = false;
                speed += 0.5f;
            }
            else if (bodies.Count > 1)
            {
                for (int i = 0; i < bodies.Count; i++)
                {
                    BodyController thisBody = bodies[i].GetComponent<BodyController>();
                    thisBody.UpdatePosition();
                }

                for (int i = bodies.Count-1; i >0; i--)
                {
                    BodyController thisBody = bodies[i].GetComponent<BodyController>();
                    BodyController headBody = bodies[i - 1].GetComponent<BodyController>();
                    thisBody.direction = headBody.direction;
                }
                bodies[0].GetComponent<BodyController>().direction = direction;
            }
            else
            {
                BodyController thisBody = bodies[0].GetComponent<BodyController>();
                thisBody.UpdatePosition();
                thisBody.direction = direction;
                
            }



            prevDirection = direction;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary") || collision.CompareTag("Body"))
        {
            Debug.Log("Killed");
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Food"))
        {
            FoodSpawner spawner = collision.GetComponentInParent<FoodSpawner>();
            spawner.SpawnFood();
            Destroy(collision.gameObject);
            justAte = true;
        }
    }
}
