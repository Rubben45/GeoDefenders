using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damageAmount, TowerSO.DamageType damageType);
}

public interface IInteractable
{
    public float CurrentAmmoAmount { get; }
    public GameObject ammoGameObject { get; }
    public TowerSO GetTowerInfo();
    public TowerSO GetUpgradeInfo();
    public float GetCurrentAmmo();
    public Tower GetCurrentTower();
}