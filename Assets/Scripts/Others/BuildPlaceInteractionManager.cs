using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlaceInteractionManager : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                BuildPlace buildPlace = hit.collider.GetComponent<BuildPlace>();

                if (buildPlace)
                {
                    UIManager.Instance.OpenUtilityPanel(buildPlace.gameObject,buildPlace.TowerBuildLocation, buildPlace.NeededTowerLcoation);
                }
                else
                {
                    UIManager.Instance.CloseUtilityPanel();
                }
            }
            else
            {
                UIManager.Instance.CloseUtilityPanel();
            }
        }
    }
}
