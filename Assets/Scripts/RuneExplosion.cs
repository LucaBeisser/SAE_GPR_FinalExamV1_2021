using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneExplosion : MonoBehaviour
{
    [SerializeField] private float timeToDealDamage = 1f;
    [SerializeField] private float damageRadius = 1f;
    [SerializeField] private float damage = 1f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToDealDamage);
        DealDamage();
    }

    private void DealDamage()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, damageRadius);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }

        Destroy(gameObject, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
