using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clasa de baza folosita pentru "gloantele" turnurilor
public class TowerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Transform currentTarget;
    private float damage;
    private TowerSO.DamageType typeOfDAmage;

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 moveDir = currentTarget.position - transform.position;
        transform.Translate(moveSpeed * Time.deltaTime * moveDir.normalized, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable enemyToHit))
        {
            enemyToHit.TakeDamage(damage, typeOfDAmage);
            Destroy(gameObject);
        }
    }

    public void SeekEnemy(Transform targetToSeek, float damageToDeal, TowerSO.DamageType damageType)
    {
        currentTarget = targetToSeek;
        damage = damageToDeal;
        typeOfDAmage = damageType;
    }
}
