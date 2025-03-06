using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;

// Clasa de baza a unui inamic care difera in functie de EnemySO, container-ul care contine informatia efectiva
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private GameObject enemyHealthHolder;
    [SerializeField] private GameObject deadParticles;


    public delegate void OnEnemyDestroyed(Enemy enemyDestryed);
    public delegate void OnHealthChanged(Enemy enemy, float newHealth);

    public event OnEnemyDestroyed EnemyDestryed;
    public event OnHealthChanged HealthChanged;
    public List<Transform> TargetLocations { get; set; } = new();

    private NavMeshAgent agent;
    private EnemySO currentEnemyData;
    private Camera mainCamera;
    private int currentTargetIndex = 0;
    private float currentEnemyHealthPoints;
    private float maxEnemyHealthPoints;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    public void EnemySetUp(EnemySO enemyData)
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy data is null when setting up enemy.");
            return;
        }


        agent.speed = enemyData.EnemyMoveSpeed;
        agent.stoppingDistance = 0.1f;
        agent.avoidancePriority = Random.Range(1, 100);
        currentEnemyHealthPoints = enemyData.EnemyHP;
        maxEnemyHealthPoints = enemyData.EnemyHP;
        currentEnemyData = enemyData;
        UpdateUI();


        MoveToNextTarget();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f || agent.remainingDistance < 0.5f)
            {
                MoveToNextTarget();
            }
        }

        enemyHealthHolder.transform.LookAt(mainCamera.transform);
    }

    private void MoveToNextTarget()
    {
        if (currentTargetIndex < TargetLocations.Count)
        {
            agent.SetDestination(TargetLocations[currentTargetIndex].position);
            currentTargetIndex++;
        }
        else
        {
            EndPath(); 
        }
    }

    private void EndPath()
    {
        EconomyManager.Instance.LoseHealth(currentEnemyData.EnemyDamage);
        Destroy(gameObject);
    }

    public void TakeDamage(float damageAmount, TowerSO.DamageType damageType)
    {
        float effectiveDamage = damageAmount;

        switch (damageType)
        {
            case TowerSO.DamageType.Physical:
                effectiveDamage *= GetPhysicalDamageMultiplier(currentEnemyData.PhysicalResistance);
                break;
            case TowerSO.DamageType.Magic:
                effectiveDamage *= GetMagicDamageMultiplier(currentEnemyData.MagicResistance);
                break;
        }

        currentEnemyHealthPoints -= effectiveDamage;

        if (currentEnemyHealthPoints <= 0f)
        {
            Die();
        }
        else
        {
            HealthChanged?.Invoke(this, currentEnemyHealthPoints);
        }

        UpdateUI();
    }

    private void Die()
    {
        GameObject currentDeadParticles = Instantiate(deadParticles, transform.position, Quaternion.identity);
        currentDeadParticles.GetComponent<ParticleSystem>().Play();

        GiveMoney();
        EnemyDestryed?.Invoke(this);
        Destroy(gameObject);
    }

    private void GiveMoney()
    {
        switch (currentEnemyData.GetEnemyType)
        {
            case EnemySO.EnemyType.Simple:
                int coinsToAddSimple = Random.Range(10, 20);
                EconomyManager.Instance.AddCoins(coinsToAddSimple);
                break;

            case EnemySO.EnemyType.Mage:
                int coinsToAddMage = Random.Range(20, 30);
                EconomyManager.Instance.AddCoins(coinsToAddMage);
                break;

            case EnemySO.EnemyType.Tank:
                int coinsToAddTank = Random.Range(35, 50);
                EconomyManager.Instance.AddCoins(coinsToAddTank);
                break;

            default: Debug.Log("What enemy are you?");
                break;
        }
    }
    private float GetPhysicalDamageMultiplier(EnemySO.EnemyPhysicalResistance resistance)
    {
        switch (resistance)
        {
            case EnemySO.EnemyPhysicalResistance.Low: return 0.85f; // 15% physical damage reduction
            case EnemySO.EnemyPhysicalResistance.Medium: return 0.60f; // 40% physical damage reduction
            case EnemySO.EnemyPhysicalResistance.High: return 0.25f; // 75% physical damage reduction
            default: return 1.0f;
        }
    }

    private float GetMagicDamageMultiplier(EnemySO.EnemyMagicResistance resistance)
    {
        switch (resistance)
        {
            case EnemySO.EnemyMagicResistance.Low: return 0.85f; // 15% magical damage reduction
            case EnemySO.EnemyMagicResistance.Medium: return 0.60f; // 40% magical damage reduction
            case EnemySO.EnemyMagicResistance.High: return 0.25f; // 75% magical damage reduction
            default: return 1.0f;
        }
    }

    private void UpdateUI()
    {
        enemyHealthBar.fillAmount = currentEnemyHealthPoints / maxEnemyHealthPoints;
    }

    public EnemySO GetEnemyInfo()
    {
        return currentEnemyData;
    }
    public float GetCurrentHP()
    {
        return currentEnemyHealthPoints;
    }
}
