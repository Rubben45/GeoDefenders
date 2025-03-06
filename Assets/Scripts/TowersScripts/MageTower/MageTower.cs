using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Clasa speciala pentru "Magician". Aici sunt toate caracteristicile turunului "Magician", cum ar fi atacul, reload-ul si toate componentele necesare
public class MageTower : Tower, IInteractable
{
    [SerializeField] private TowerSO currentTowerSO;
    [SerializeField] private TowerSO upgradedTowerSO;

    [SerializeField] private Transform trianglePivot;
    [SerializeField] private Transform laserStartPoint;
    [SerializeField] private GameObject pyramid;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private float chargeTime = 3f; // Time to charge before firing
    [SerializeField] private Color normalColor = Color.yellow; // Color during charging
    [SerializeField] private Color readyColor = Color.red; // Color when ready to fire
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float laserDuration = 0.5f;
    [SerializeField] private GameObject ammo;
    public GameObject ammoGameObject => ammo;

    public float CurrentAmmoAmount => currentAmmoAmount;

    private List<Enemy> enemiesInRange = new();
    private Transform target;
    private float chargeCountdown = 0f;
    private float currentAmmoAmount;

    private Camera mainCamera;

    private void Start()
    {
        currentAmmoAmount = currentTowerSO.TowerAmmo;
        chargeCountdown = chargeTime;
        laserLine.enabled = false;
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            UpdateTarget();
            if (target != null)
            {
                if (chargeCountdown <= 0f)
                {
                    Shoot();
                    chargeCountdown = chargeTime;
                }
                else
                {
                    ChargeLaser();
                    chargeCountdown -= Time.deltaTime;
                }
            }
        }
        else
        {
            laserLine.enabled = false;
            chargeCountdown = chargeTime;
        }

        if (CurrentAmmoAmount < 3)
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
    private void ChargeLaser()
    {
        Renderer renderer = pyramid.GetComponent<Renderer>();

        renderer.material.color = normalColor;
        trianglePivot.Rotate(Time.deltaTime * turnSpeed * Vector3.up, Space.Self);

        if (currentAmmoAmount == 0) return;



        float lerpValue = (chargeTime - chargeCountdown) / chargeTime;
        Color currentColor = Color.Lerp(normalColor, readyColor, lerpValue);
        

        if (renderer != null)
        {
            renderer.material.color = currentColor;
        }
        else
        {
            Debug.Log("Renderer is null on pyramid");
        }
    }

  
    private void Shoot()
    {
        if (target == null) return;
        if (currentAmmoAmount == 0) return;

        StartCoroutine(AnimateLaser(target));

        StartCoroutine(DisableLaserAfterDelay(laserDuration));
        target.GetComponent<IDamageable>()?.TakeDamage(currentTowerSO.AttackDamage, currentTowerSO.TowerDamageType);

        SFXManager.Instance.PlaySoundEffect("Mage");
        currentAmmoAmount--;
        currentAmmoAmount = Mathf.Clamp(currentAmmoAmount, 0, currentTowerSO.TowerAmmo);
    }
    private IEnumerator DisableLaserAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        laserLine.enabled = false;
    }

    private IEnumerator AnimateLaser(Transform target)
    {
        laserLine.enabled = true;
        Vector3 startPoint = laserStartPoint.position;
        Vector3 endPoint = target.position;

        // Animate laser growing
        float elapsedTime = 0f;
        while (elapsedTime < laserDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / laserDuration;
            laserLine.SetPosition(0, startPoint);
            laserLine.SetPosition(1, Vector3.Lerp(startPoint, endPoint, t));
            yield return null;
        }

        // Ensure the laser reaches the end point
        laserLine.SetPosition(1, endPoint);

        yield return new WaitForSeconds(0.1f); // Keep the laser visible for a short duration

        // Animate laser retracting
        elapsedTime = 0f;
        while (elapsedTime < laserDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / laserDuration;
            laserLine.SetPosition(0, startPoint);
            laserLine.SetPosition(1, Vector3.Lerp(endPoint, startPoint, t));
            yield return null;
        }

        laserLine.enabled = false;
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

    private void EliminateDestryedEnemy(Enemy enemyToEliminate)
    {
        if (enemiesInRange.Contains(enemyToEliminate))
        {
            enemiesInRange.Remove(enemyToEliminate);
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
