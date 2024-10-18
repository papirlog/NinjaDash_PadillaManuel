using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterRespawn : MonoBehaviour
{
    static public bool isAlive = true;

    //Spawn o respawn point
    [SerializeField] private Transform respawnPoint;

    //Timer para respawnear
    [SerializeField] private float respawnTime;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Para verificar desde que lado esta colisionando
            Vector3 normal = collision.contacts[0].normal;

            if (normal.y < 0.5f)
            {
                StartCoroutine(HandleRespawn());
                isAlive = false;
            }
        }

        if (collision.gameObject.CompareTag("Spikes") && isAlive)
        {
            StartCoroutine(HandleRespawn());
            isAlive = false;
        }
    }


    //Funcion para que una vez el personaje muera, tarde cierto tiempo es spawnear
    private IEnumerator HandleRespawn()
    {
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(respawnTime);

        functionCharacterRespawn();
    }

    //Funcion para respawnear al personaje
    private void functionCharacterRespawn()
    {
        transform.position = respawnPoint.position;
        isAlive = true;
        rb.velocity = Vector3.zero;
    }
}
