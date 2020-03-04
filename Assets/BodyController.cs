using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Vector2 direction;
    public float stride;
    public bool isTail = false;

    private Vector2 prevDirection;


    public void UpdatePosition()
    {
        transform.position += new Vector3(direction.x, direction.y, 0f) * stride;
        if ((direction != prevDirection) && isTail)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Atan2(-direction.y, -direction.x) * 180f / Mathf.PI));

        prevDirection = direction;
    }
}
