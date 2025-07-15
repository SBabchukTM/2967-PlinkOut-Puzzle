using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUpgradeController : MonoBehaviour
{
    [SerializeField] List<BuildingController> buildingControllers;

    public void SetBuildingLevel(int id, int level)
    {
        var buildingViews = buildingControllers[id].BuildingViews;

        foreach (var buildingView in buildingViews)
        {
            buildingView.gameObject.SetActive(false);
        }
        buildingViews[level].gameObject.SetActive(true);
    }
}