using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manager care se ocupa cu construirea si upgradarea turnurilor
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void BuildTower(TowerSO towerToBuild, Transform buildLocation, Tower.TowerLocation towerLocation, GameObject oldBuild)
    {
        if (towerToBuild.TowerPrice <= EconomyManager.Instance.CurrentCoins)
        {
            switch (towerLocation)
            {
                case Tower.TowerLocation.Up:
                    BuildUp(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Up);
                    break;

                case Tower.TowerLocation.Down:
                    BuildDown(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Down);
                    break;

                case Tower.TowerLocation.Left:
                    BuildOnLeftPart(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Left);
                    break;

                case Tower.TowerLocation.Right:
                    BuildOnRightPart(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Right);
                    break;

                default:
                    Debug.Log("What position are you looking at??");
                    break;
            }

        }

    }

    public Tower UpgradeTower(TowerSO towerToBuild, Transform buildLocation, Tower.TowerLocation towerLocation, GameObject oldBuild)
    {
        GameObject newTower = null;
        switch (towerLocation)
        {
            case Tower.TowerLocation.Up:
                newTower = BuildUp(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Up);
                break;

            case Tower.TowerLocation.Down:
                newTower = BuildDown(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Down);
                break;

            case Tower.TowerLocation.Left:
                newTower = BuildOnLeftPart(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Left);
                break;

            case Tower.TowerLocation.Right:
                newTower = BuildOnRightPart(towerToBuild, buildLocation, oldBuild, Tower.TowerLocation.Right);
                break;

            default:
                Debug.Log("What position are you looking at??");
                break;
        }

        return newTower?.GetComponent<Tower>();
    }

    private GameObject BuildDown(TowerSO towerToBuild, Transform buildLocation, GameObject oldPlace, Tower.TowerLocation towerLocation)
    {
        GameObject spawnedTower;
        if (towerToBuild.GetTowerType == TowerSO.TowerType.Temple)
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.Euler(new Vector3(0f, 270f, 0f)));
        }
        else
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.Euler(new Vector3(0f, 180f, 0f)));
        }
        spawnedTower.GetComponent<Tower>().CurrentTowerLocation = towerLocation;

        Destroy(oldPlace);
        EconomyManager.Instance.RemoveCoins(towerToBuild.TowerPrice);
        return spawnedTower;
    }

    private GameObject BuildUp(TowerSO towerToBuild, Transform buildLocation, GameObject oldPlace, Tower.TowerLocation towerLocation)
    {
        GameObject spawnedTower;
        if (towerToBuild.GetTowerType == TowerSO.TowerType.Temple)
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.Euler(new Vector3(0f, 90f, 0f)));
        }
        else
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.identity);
        }
        spawnedTower.GetComponent<Tower>().CurrentTowerLocation = towerLocation;

        Destroy(oldPlace);
        EconomyManager.Instance.RemoveCoins(towerToBuild.TowerPrice);
        return spawnedTower;
    }

    private GameObject BuildOnLeftPart(TowerSO towerToBuild, Transform buildLocation, GameObject oldPlace, Tower.TowerLocation towerLocation)
    {
        GameObject spawnedTower;
        if (towerToBuild.GetTowerType == TowerSO.TowerType.Temple)
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.Euler(new Vector3(0f, 180f, 0f)));
        }
        else
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.Euler(new Vector3(0f, 90f, 0f)));
        }
        spawnedTower.GetComponent<Tower>().CurrentTowerLocation = towerLocation;

        Destroy(oldPlace);
        EconomyManager.Instance.RemoveCoins(towerToBuild.TowerPrice);
        return spawnedTower;
    }

    private GameObject BuildOnRightPart(TowerSO towerToBuild, Transform buildLocation, GameObject oldPlace, Tower.TowerLocation towerLocation)
    {
        GameObject spawnedTower;
        if (towerToBuild.GetTowerType == TowerSO.TowerType.Temple)
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.identity);
        }
        else
        {
            spawnedTower = Instantiate(towerToBuild.TowerPrefab, buildLocation.position, Quaternion.Euler(new Vector3(0f, 270f, 0f)));
        }
        spawnedTower.GetComponent<Tower>().CurrentTowerLocation = towerLocation;

        Destroy(oldPlace);
        EconomyManager.Instance.RemoveCoins(towerToBuild.TowerPrice);
        return spawnedTower;
    }

}
