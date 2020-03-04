using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject food;
    public Vector2 LowerLeft;
    public Vector2 UpperRight;
    public float multiple;
    // Start is called before the first frame update
    void Start()
    {
        SpawnFood();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFood()
    {
        float xpos = Random.Range(LowerLeft.x, UpperRight.x);
        float ypos = Random.Range(LowerLeft.y, UpperRight.y);
        xpos -= xpos % multiple;
        ypos -= ypos % multiple;
        Instantiate(food, new Vector2(xpos, ypos), Quaternion.identity, GetComponent<Transform>());
    }
}
