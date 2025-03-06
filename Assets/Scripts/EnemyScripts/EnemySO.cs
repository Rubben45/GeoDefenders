using UnityEngine;

// In acest container se gasesc toate caracteristicile de baza a unui inamic

[CreateAssetMenu(fileName = "EnemyType", menuName = "Enemy / CreateEnemySO")]
public class EnemySO : ScriptableObject
{
    public GameObject EnemyPrefab => enemyPrefab;
    public float EnemyHP => enemyHP;
    public float EnemyMoveSpeed => enemyMoveSpeed;
    public int EnemyDamage => enemyDamage;
    public EnemyPhysicalResistance PhysicalResistance => physicalResistance;
    public EnemyMagicResistance MagicResistance => magicResistance;
    public EnemyType GetEnemyType => enemyType;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float enemyHP;
    [SerializeField] private float enemyMoveSpeed;
    [SerializeField] private int enemyDamage = 1;
    [SerializeField] private EnemyPhysicalResistance physicalResistance;
    [SerializeField] private EnemyMagicResistance magicResistance;
    [SerializeField] private EnemyType enemyType;

    public enum EnemyPhysicalResistance
    {
        None, 
        Low,
        Medium,
        High
    }
    public enum EnemyMagicResistance
    {
        None,
        Low,
        Medium, 
        High
    }
    public enum EnemyType
    {
        Simple,
        Mage,
        Tank
    }


}
