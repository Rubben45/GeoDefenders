using UnityEngine;


// In acest container se gasesc toate caracteristicile de baza a unui turn

[CreateAssetMenu(fileName = "TowerSO", menuName = "Tower")]
public class TowerSO : ScriptableObject
{
    [Header("Basic Settings")]
    [SerializeField, Tooltip("The prefab of the tower.")]
    private GameObject towerPrefab;
    public GameObject TowerPrefab => towerPrefab;

    [SerializeField, Tooltip("Damage dealt by the tower.")]
    private float attackDamage;
    public float AttackDamage => attackDamage;

    [SerializeField, Tooltip("Rate of attacks per second.")]
    private float attackSpeed;
    public float AttackSpeed => attackSpeed;

    [SerializeField, Tooltip("Tower's ammo capacity")]
    private float towerAmmo;
    public float TowerAmmo => towerAmmo;

    [SerializeField, Tooltip("Effective range of the tower.")]
    private float towerRange;
    public float TowerRange => towerRange;

    [SerializeField, Tooltip("Tower price to build or upgrade")]
    private int towerPrice;
    public int TowerPrice => towerPrice;

    [SerializeField, Tooltip("Type of damage dealt by the tower.")]
    private DamageType damageType;
    public DamageType TowerDamageType => damageType;

    [SerializeField, Tooltip("Tower type")]
    private TowerType towerType;
    public TowerType GetTowerType => towerType;

    [Header("Additional Components")]
    [SerializeField, Tooltip("Prefab of the cannon smoke visual effects.")]
    private GameObject cannonSmokeVFX;
    public GameObject CannonSmokeVFX => cannonSmokeVFX;

    [SerializeField, Tooltip("Prefab of the cannonball.")]
    private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    public enum DamageType
    {
        Physical,
        Magic
    }

    public enum TowerType
    {
        Cannon, 
        Temple,
        Mage
    }

}
