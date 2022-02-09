using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isJumping;
    private float playerInput;
   
    [SerializeField]
    private float walkSpeed;
    
    [SerializeField] 
    private float jumpForce;

    // [SerializeField]
    private bool isGrounded;

    [SerializeField] private float fastDashSpeedMultiplier;
    [SerializeField] private float slowDashSpeedMultiplier;

    [SerializeField] private Transform startTransform;

    [SerializeField] private Text deathCountText;

    [SerializeField] private int deathCount;

    public int DeathCount
    {
        get
        {
            return deathCount;
        }
        set
        {
            deathCountText.text = "Deathcunt: " + value;
        }
    }
    // Slow dash stuff
    [SerializeField] private GameObject slowDashTargetObject;
    private bool hasSlowDashTarget;
    [SerializeField] private bool isSlowDashEnabled;

    // Fast dash stuff
    private bool isFastDashing;
    [SerializeField] private bool isFastDashEnabled;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CancelSlowDash();
        DeathCount = 0;
    }

    void CancelSlowDash()
    {
        hasSlowDashTarget = false;
        slowDashTargetObject.SetActive(false);
    }

    void EnableSlowDash()
    {
        hasSlowDashTarget = true;
        slowDashTargetObject.SetActive(true);
    }

    void Update()
    {
        if (isSlowDashEnabled && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray clickRay = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(clickRay, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Ground"))
                {
                    slowDashTargetObject.transform.position = new Vector3(hitInfo.point.x, 0.5f, 0.0f);
                    EnableSlowDash();
                }
            }
        }

        if (hasSlowDashTarget && Vector3.Distance(transform.position, slowDashTargetObject.transform.position) < 0.4f)
        {
            CancelSlowDash();
        }

        playerInput = Input.GetAxis("Horizontal");
        isFastDashing = Input.GetKey(KeyCode.F);
        
        bool shouldCancelSlowDash = Mathf.Abs(playerInput) > 0.1f || isFastDashing;
        if (hasSlowDashTarget && shouldCancelSlowDash)
        {
            CancelSlowDash();
        }
        
        isJumping = Input.GetKeyDown(KeyCode.Space);
        if (isJumping && isGrounded)
        {
            rb.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        float movementSpeed = 0;

        if (hasSlowDashTarget)
        {
            movementSpeed = walkSpeed * slowDashSpeedMultiplier;
            Vector3 direction = (slowDashTargetObject.transform.position - transform.position).normalized;
            rb.velocity = direction * movementSpeed * Time.deltaTime;
        }
        else
        {
            movementSpeed = walkSpeed;            
            if (isFastDashEnabled && isFastDashing)
            {
                movementSpeed = walkSpeed * fastDashSpeedMultiplier;
            }
            
            Vector3 newVelocity = new Vector3(playerInput * movementSpeed * Time.deltaTime, rb.velocity.y, 0.0f);
            rb.velocity = newVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.position = startTransform.position;
            CancelSlowDash();
            deathCount++;
            DeathCount = deathCount;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }    
    }
    
}
