using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterController : MonoBehaviour
{

    //Defniendo variables
    [SerializeField] InputActionReference jump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;

    //Definiendo gravedad
    [SerializeField] private float fallMultiplier;

    //Mas variables y componentes
    private Rigidbody rb;
    private Animator animator;

    private bool isGrounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        jump.action.performed += OnJump;
        jump.action.Enable();
    }

    private void OnDisable()
    {
        jump.action.performed -= OnJump;
        jump.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (characterRespawn.isAlive)
        {
            //Para avanzar automaticamente
            rb.velocity = new Vector3(movementSpeed, rb.velocity.y, rb.velocity.z);

            //Para que acelere la caida
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(isGrounded && characterRespawn.isAlive)
        {
            characterJump();
        }
    }

    //Funcion para saltar
    private void characterJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        isGrounded = false;
        animator.SetBool("Jumping", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Para verificar desde que lado esta colisionando
            Vector3 normal = collision.contacts[0].normal;

            if (normal.y > 0.5f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                isGrounded = true;
                animator.SetBool("Jumping", false);
            }
        }
    }

}
