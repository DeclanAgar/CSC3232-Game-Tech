using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset; // Set camera position to player plus offset vector
    }
}