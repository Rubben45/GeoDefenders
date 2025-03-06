using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

// Se ocupa cu interactiunea dintre jucator si mediu inconjurator prin mouse ( probabil cel mai prost optimizat cod din acest proiect =)) ) 
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [HideInInspector] public Enemy CurrentSelectedEnemy;

    private Camera mainCamera;
    private IInteractable currentTower;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                IInteractable tower = hit.collider.GetComponent<IInteractable>();

                if (enemy)
                {
                    if (CurrentSelectedEnemy != enemy)
                    {
                        CurrentSelectedEnemy = enemy;
                        UIManager.Instance.ShowEnemyInfo(enemy.GetEnemyInfo(), enemy.GetCurrentHP());
                        SFXManager.Instance.PlaySoundEffect("Click");
                        enemy.EnemyDestryed += OnEnemyDestroyed;
                    }
                }
                else if (CurrentSelectedEnemy)
                {
                    CurrentSelectedEnemy = null;
                    UIManager.Instance.HideEnemyInfo();
                }

                if (tower != null)
                {
                    currentTower = tower;
                    UIManager.Instance.ShowTowerInfo(tower.GetTowerInfo(), tower.GetCurrentAmmo());
                    UIManager.Instance.ShowTowerUtilityInfo();
                    SFXManager.Instance.PlaySoundEffect("Click");
                    TowerUtilityManager.Instance.SetTowerUtilityPanel(tower.GetCurrentTower(), tower, tower.GetTowerInfo(), tower.GetUpgradeInfo());
                }
                else
                {
                    currentTower = null;
                    UIManager.Instance.HideTowerInfo();
                    UIManager.Instance.HideTowerUtilityInfo();
                }
            }
            else
            {
                UIManager.Instance.HideEnemyInfo();
                UIManager.Instance.HideTowerInfo();
                UIManager.Instance.HideTowerUtilityInfo();
            }
        }

        if (CurrentSelectedEnemy)
        {
            UIManager.Instance.ShowEnemyInfo(CurrentSelectedEnemy.GetEnemyInfo(), CurrentSelectedEnemy.GetCurrentHP());
        }

        if (currentTower != null)
        {
            TowerUtilityManager.Instance.SetTowerUtilityPanel(currentTower.GetCurrentTower(), currentTower, currentTower.GetTowerInfo(), currentTower.GetUpgradeInfo());
            UIManager.Instance.ShowTowerInfo(currentTower.GetTowerInfo(), currentTower.GetCurrentAmmo());
           // UIManager.Instance.ShowTowerUtilityInfo();
        }
    }

    private void OnEnemyDestroyed(Enemy enemy)
    {
        if (CurrentSelectedEnemy == enemy)
        {
            CurrentSelectedEnemy = null;
            UIManager.Instance.HideEnemyInfo();
        }
        enemy.EnemyDestryed -= OnEnemyDestroyed;
    }
}
