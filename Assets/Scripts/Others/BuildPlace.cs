using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlace : MonoBehaviour
{
    [SerializeField] private Transform signBoard;
    [SerializeField] private Transform towerBuildLocation;
    [SerializeField] private Tower.TowerLocation neededTowerLocation;

    public Tower.TowerLocation NeededTowerLcoation => neededTowerLocation;
    public Transform TowerBuildLocation => towerBuildLocation;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dirToTarget = mainCamera.transform.position - signBoard.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Quaternion correctedRotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y + 270f, 0f);
        signBoard.rotation = Quaternion.Lerp(signBoard.rotation, correctedRotation, Time.deltaTime * 10f);
    }
}
