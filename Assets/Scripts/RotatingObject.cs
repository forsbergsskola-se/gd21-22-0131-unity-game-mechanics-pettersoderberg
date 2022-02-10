using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.UIElements;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RotatingObject : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 rotationVector;
    [SerializeField] private float rotationSpeed;
    void Start()
    {
       rotationVector  = Vector3.zero;

    }
    void Update()
    {
        rotationVector = new Vector3(0, rotationSpeed*Time.deltaTime, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(rotationVector);
        gameObject.transform.rotation = gameObject.transform.rotation *= deltaRotation;

    }
}
