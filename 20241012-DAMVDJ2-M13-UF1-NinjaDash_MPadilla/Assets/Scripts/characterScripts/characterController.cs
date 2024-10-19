using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterController : MonoBehaviour
{

    //Defniendo variables
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference parry;
    [SerializeField] private float jumpForce;
    [SerializeField] private float extraJumpForce;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float parryDuration;

    //Definiendo gravedad
    [SerializeField] private float fallMultiplier;

    //Mas variables y componentes
    [SerializeField] private GameObject parryBarrier;

    private Rigidbody rb;
    private Animator animator;

    private bool isGrounded = true;
    private bool hasExtraJump = false;
    private bool isParrying = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        jump.action.performed += OnJump;
        parry.action.performed += OnParry;

        jump.action.Enable();
        parry.action.Enable();
    }

    private void OnDisable()
    {
        jump.action.performed -= OnJump;
        parry.action.performed -= OnParry;

        jump.action.Disable();
        parry.action.Disable();
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

    //Entrar en el estado de parry
    private void OnParry(InputAction.CallbackContext context)
    {
        if (!isParrying && characterRespawn.isAlive)
        {
            isParrying = true;

            if (isGrounded)
            {
                animator.SetBool("groundParry", true);
            }
            else
            {
                animator.SetBool("airParry", true);
            }
        }

        parryBarrier.SetActive(true);
        

        StartCoroutine(parryTimerCount(parryDuration));
    }

    //Para desactivar el estado de parry
    private IEnumerator parryTimerCount(float parryDuration)
    {
        yield return new WaitForSeconds(parryDuration);
        isParrying = false;

        parryBarrier.SetActive(false);

        animator.SetBool("groundParry", false);
        animator.SetBool("airParry", false);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(isGrounded && characterRespawn.isAlive)
        {
            characterJump();
        }
        else if(!isGrounded && hasExtraJump)
        {
            PerformExtraJump();
        }
    }

    //Funcion para saltar
    private void characterJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        isGrounded = false;
        animator.SetBool("Jumping", true);
    }

    //Salto extra
    public void PerformExtraJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, extraJumpForce, rb.velocity.z);
        animator.SetBool("extraJumping", true);
        hasExtraJump = false;
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

    public void EnableExtraJump()
    {
        hasExtraJump = true;
    }

}
