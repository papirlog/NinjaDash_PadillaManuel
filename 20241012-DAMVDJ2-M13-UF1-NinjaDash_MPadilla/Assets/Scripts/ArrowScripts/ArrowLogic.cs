using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour
{
    [SerializeField] private float arrowSpeed;

    Vector3 direction = Vector3.left;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * arrowSpeed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("parryBarrier"))
        {
            other.GetComponentInParent<characterController>().EnableExtraJump();
            Destroy(gameObject);
        }
    }
}
