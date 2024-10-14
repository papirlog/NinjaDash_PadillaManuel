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

    //Spawn o respawn point
    [SerializeField] private Transform respawnPoint;

    //Timer para respawnear
    [SerializeField] private float respawnTime;

    //Definiendo gravedad
    [SerializeField] private float fallMultiplier;

    //Mas variables y componentes
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isAlive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        if (isAlive)
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
        if(isGrounded && isAlive)
        {
            characterJump();
        }
    }

    //Funcion para saltar
    private void characterJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            //Para verificar desde que lado esta colisionando
            Vector3 normal = collision.contacts[0].normal;

            if(normal.y > 0.5f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                isGrounded = true;
            }
            else
            {
                StartCoroutine(HandleRespawn());
            }
        }

        if (collision.gameObject.CompareTag("Spikes") && isAlive)
        {
            StartCoroutine(HandleRespawn());
        }
    }


    //Funcion para que una vez el personaje muera, tarde cierto tiempo es spawnear
    private IEnumerator HandleRespawn()
    {
        isAlive = false;
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(respawnTime);

        characterRespawn();
    }

    //Funcion para respawnear al personaje
    private void characterRespawn()
    {
        transform.position = respawnPoint.position;
        isAlive = true;
        isGrounded = true;
        rb.velocity = Vector3.zero;
    }
}
