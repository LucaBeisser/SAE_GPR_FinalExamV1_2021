using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float selfdestructTime = 10;
    [SerializeField] private Rune rune;

    private void Start()
    {
        _rigidbody.velocity = transform.up * -5f + transform.forward  * 10f;
        Destroy(gameObject, selfdestructTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(rune, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
