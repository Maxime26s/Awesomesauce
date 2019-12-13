using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("Player movement")]
    public CharacterController controller;
    public bool canMove = true;
    public float speed = 10f, gravity = -9.81f, jumpHeight = 3f, groundDistance = 0.4f;
    public Transform cameraT;
    [Header("Player rotation")]
    public float desiredRotationSpeed = 1f;
    public float allowPlayerRotation = 0.1f;
    [Header("Player jump")]
    public bool isGrounded;
    public LayerMask groundMask;
    public Transform groundCheck;
    public Vector3 velocity;
    [Header("Player dash")]
    public bool isDashing = false;
    public float dashSpeed = 20f;
    public Vector3 dashForward;
    [Header("Player attack")]
    public bool isAttacking = false;
    public GameObject weapon;
    public IEnumerator comboCoroutine;
    public int combo = 0;
    public List<GameObject> entityHit;
    [Header("Player interact")]
    public bool isInteracting;
    public GameObject interactable;
    public InteractCollider interactScript;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        entityHit = new List<GameObject>();
        interactScript = GetComponentInChildren<InteractCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region Jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        #endregion
        #region Dash
        if (Input.GetKey(KeyCode.LeftShift)&&canMove)
        {
            IEnumerator Dash()
            {
                canMove = false;
                dashForward = transform.forward;
                isDashing = true;
                yield return new WaitForSeconds(0.5f);
                isDashing = false;
                transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
                canMove = true;
            }
            StartCoroutine(Dash());
        }
        if (isDashing)
        {
            transform.Rotate(720*Time.deltaTime, 0, 0);
            controller.Move(dashForward * dashSpeed * Time.deltaTime);
        }
        #endregion
        #region Attack
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            IEnumerator Combo()
                {
                    combo++;
                    yield return new WaitForSeconds(1.5f);
                    combo = 0;
                }
            IEnumerator Attack()
            {
                canMove = false;
                isAttacking = true;
                Vector3 forward = cameraT.forward;
                forward.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), desiredRotationSpeed);
                weapon.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                weapon.SetActive(false);
                isAttacking = false;
                entityHit.Clear();
                if (combo < 5)
                {
                    comboCoroutine = Combo();
                    StartCoroutine(comboCoroutine);
                }
                else
                    combo = 0;
                canMove = true;
            }
            if (combo > 0)
                StopCoroutine(comboCoroutine);
            StartCoroutine(Attack());
        }
        if (isAttacking)
        {
            //doyourthing
        }
        #endregion
        #region Interact
        if (interactable != null&& !isInteracting && Input.GetKey(KeyCode.F))
        {
            IEnumerator Interact(float time, bool ableToMove){
                isInteracting = true;
                canMove = ableToMove;
                yield return new WaitForSeconds(time);
                canMove = true;
                isInteracting = false;
            }
            if(interactable.tag == "Drop")
            {
                isInteracting = true;
                StartCoroutine(Interact(0.2f, true));
                interactable.SetActive(false);
                Destroy(interactable);
                interactable = null;
                Debug.Log("destroyed");
                interactScript.DisableText();
                //add to inventory
            }
        }
        #endregion
        #region Movement
        if (canMove)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (new Vector2(x, z).sqrMagnitude > allowPlayerRotation)
            {
                Vector3 forward = cameraT.forward;
                Vector3 right = cameraT.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                Vector3 move = forward * z + right * x;
                controller.Move(move * speed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), desiredRotationSpeed);
            }
        }
        #endregion
    }
}
