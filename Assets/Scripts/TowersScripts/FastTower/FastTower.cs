using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Clasa speciala pentru "Templu". Aici sunt toate caracteristicile turunului "Templu", cum ar fi atacul, reload-ul si toate componentele necesare
public class FastTower : Tower, IInteractable
{
    [SerializeField] private TowerSO currentTowerSO;
    [SerializeField] private TowerSO upgradedTowerSO;

    [SerializeField] private Transform[] ammoSpawnLocations;

    [SerializeField] private GameObject ammo;
    public GameObject ammoGameObject => ammo;
    public float CurrentAmmoAmount => currentAmmoAmount;

    private List<Enemy> enemiesInRange = new();
    private Transform target;
    private float fireCountdown = 0f;
    private float currentAmmoAmount;


    private void Start()
    {
        currentAmmoAmount = currentTowerSO.TowerAmmo;
    }

    private void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            UpdateTarget();

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / currentTowerSO.AttackSpeed;
            }

            fireCountdown -= Time.deltaTime;
        }

        if (CurrentAmmoAmount < 10)
        {
            ammoGameObject.SetActive(true);
            ammoGameObject.GetComponent<Image>().color = Color.red;
            ammoGameObject.transform.LookAt(Camera.main.transform);
        }
        else
        {
            ammoGameObject.SetActive(false);
        }
    }

    // Aceasta functie se ocupa cu vizarea inamicilor si este specifica fiecarui turn
    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;
        foreach (Enemy enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= currentTowerSO.TowerRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Shoot()
    {
        if (target == null) return;
        if (currentAmmoAmount == 0) return;

        int randomSpawnLocation = Random.Range(0, 1);
        Transform currentSpawnLocation = ammoSpawnLocations[randomSpawnLocation];

        GameObject currentShiriken = Instantiate(currentTowerSO.ProjectilePrefab, currentSpawnLocation.position, Quaternion.identity);
        TowerBullet shiriken = currentShiriken.GetComponent<TowerBullet>();

        if (shiriken != null)
        {
            shiriken.SeekEnemy(target, currentTowerSO.AttackDamage, currentTowerSO.TowerDamageType);
        }

        SFXManager.Instance.PlaySoundEffect("Temple");
        currentAmmoAmount--;
        currentAmmoAmount = Mathf.Clamp(currentAmmoAmount, 0, currentTowerSO.TowerAmmo);
    }

    private void EliminateDestryedEnemy(Enemy enemyToEliminate)
    {
        if (enemiesInRange.Contains(enemyToEliminate))
        {
            enemiesInRange.Remove(enemyToEliminate);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemiesInRange.Add(enemy);
                enemy.EnemyDestryed += EliminateDestryedEnemy;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.EnemyDestryed -= EliminateDestryedEnemy;
                enemiesInRange.Remove(enemy);
            }
        }
    }

    public override void ReloadAmmo()
    {
        currentAmmoAmount = currentTowerSO.TowerAmmo;
    }

    public TowerSO GetTowerInfo()
    {
        return currentTowerSO;
    }
    public TowerSO GetUpgradeInfo()
    {
        return upgradedTowerSO;
    }

    public float GetCurrentAmmo()
    {
        return currentAmmoAmount;
    }

    public Tower GetCurrentTower()
    {
        return this;
    }
}
