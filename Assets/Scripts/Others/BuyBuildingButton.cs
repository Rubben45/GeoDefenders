using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class BuyBuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TowerSO towerToBuildWithThisButton;
    [SerializeField] private GameObject currentBuuldingPanel;

    private Button currentBuyBuildButton;
    public Transform PlaceToBuildTower { get; set; }
    public GameObject OldBuild { get; set; }
    public Tower.TowerLocation CurrentTowerLocation { get; set; }


    private void Start()
    {
        currentBuyBuildButton = GetComponent<Button>();
        if (currentBuyBuildButton == null)
        {
            Debug.LogError("Button component is missing!");
        }
        else
        {
            currentBuyBuildButton.onClick.AddListener(BuildTower);
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowBuildingInfoPanel(currentBuuldingPanel);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideBuildingInfoPanel(currentBuuldingPanel);
    }

    public void BuildTower()
    {
        if (BuildingManager.Instance != null)
        {
            UIManager.Instance.HideBuildingInfoPanel(currentBuuldingPanel);
            BuildingManager.Instance.BuildTower(towerToBuildWithThisButton, PlaceToBuildTower, CurrentTowerLocation, OldBuild);
        }
        else
        {
            Debug.LogError("BuildingManager instance is null!");
        }
        PlaceToBuildTower = null;
    }
}
