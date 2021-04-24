using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private RuneExplosion runeExplosion;
    [SerializeField] private float aliveTime = 10f;

    private void Start()
    {
        Destroy(gameObject, aliveTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            Instantiate(runeExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
