using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    private Collider2D beeCollider;

    // Start is called before the first frame update
    void Start()
    {
        beeCollider = GetComponent<Collider2D>();    
    }

    // MoveBee moves a bee towards a given direction scaled by time between frames
    public void MoveBee(Vector2 direction)
    {
        transform.up = direction;
        transform.position += (Vector3)direction * Time.deltaTime;
    }

    public void SetBeeCollider(Collider2D tempBeeCollider)
    {
        beeCollider = tempBeeCollider;
    }

    public Collider2D GetBeeCollider()
    {
        return beeCollider;
    }
}
