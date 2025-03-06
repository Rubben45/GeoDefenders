using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Clasa speciala pentru "Tun". Aici sunt toate caracteristicile turunului "Tun", cum ar fi atacul, reload-ul si toate componentele necesare
public class CannonTower : Tower, IInteractable
{
    [SerializeField] private TowerSO currentTowerSO;
    [SerializeField] private TowerSO upgradedTowerSO;

    [Header("TowerMovingParts")]
    [SerializeField] private Transform rotatoryPart;
    [SerializeField] private Transform cannon;
    [SerializeField] private Transform cannonBallSpawningLocation;
    [SerializeField] private Animator cannonAnimator;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private GameObject ammo;
    public GameObject ammoGameObject => ammo;

    public float CurrentAmmoAmount => currentAmmoAmount;

    private List<Enemy> enemiesInRange = new();
    private Transform target;
    private float fireCountdown = 0f;
    private float currentAmmoAmount;
    private Camera mainCamera;

    private void Start()
    {
        currentAmmoAmount = currentTowerSO.TowerAmmo;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            UpdateTarget();
            LockOnTarget();

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / currentTowerSO.AttackSpeed;
            }

            fireCountdown -= Time.deltaTime;
        }

        if (CurrentAmmoAmount < 5)
        {
            ammoGameObject.SetActive(true);
            ammoGameObject.GetComponent<Image>().color = Color.red;
            ammoGameObject.transform.LookAt(mainCamera.transform);
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

    private void LockOnTarget()
    {
        if (target == null) return;

        // Horizontal rotation (for the rotatory part)
        Vector3 dirToTarget = target.position - rotatoryPart.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Quaternion correctedRotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y + 270, 0f);
        rotatoryPart.rotation = Quaternion.Lerp(rotatoryPart.rotation, correctedRotation, Time.deltaTime * turnSpeed);

        // Debugging lines to visualize aiming
        Debug.DrawLine(rotatoryPart.position, target.position, Color.red); // Line to target
        Debug.DrawLine(cannon.position, cannon.position + cannon.forward * 5, Color.blue); // Cannon's forward vector
    }
    private void Shoot()
    {
        if (target == null) return;
        if (currentAmmoAmount == 0) return;

        Instantiate(currentTowerSO.CannonSmokeVFX, cannonBallSpawningLocation.position, Quaternion.identity);
        cannonAnimator.SetTrigger("Shoot");
        GameObject currentCannonBall = Instantiate(currentTowerSO.ProjectilePrefab, cannonBallSpawningLocation.position, Quaternion.identity);
        TowerBullet cannonBall = currentCannonBall.GetComponent<TowerBullet>();

        if (cannonBall != null)
        {
            cannonBall.SeekEnemy(target, currentTowerSO.AttackDamage, currentTowerSO.TowerDamageType);
        }

        SFXManager.Instance.PlaySoundEffect("Cannon");
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

    public override void ReloadAmmo()
    {
        currentAmmoAmount = currentTowerSO.TowerAmmo;
    }

}
