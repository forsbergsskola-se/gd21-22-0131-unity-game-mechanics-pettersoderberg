using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingWall : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 currentTargetPosition;

    [SerializeField]
    private Vector3 deltaMovement;
    [SerializeField]
    private float movementSpeed;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        endPosition = startPosition + deltaMovement;
        currentTargetPosition = endPosition;
    }

    void Update()
    {
        endPosition = startPosition + deltaMovement;

        float distance = Vector3.Distance(transform.position, currentTargetPosition);

        if (distance < 0.4f)
        {
            if (currentTargetPosition == endPosition)
            {
                rb.MovePosition(endPosition);
                currentTargetPosition = startPosition;
            }
            else if (currentTargetPosition == startPosition)
            {
                rb.MovePosition(startPosition);
                currentTargetPosition = endPosition;
            }
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, currentTargetPosition, movementSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }
}
