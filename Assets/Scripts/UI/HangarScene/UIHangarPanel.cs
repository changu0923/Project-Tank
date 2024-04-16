using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHangarPanel : MonoBehaviour
{
    public UITopBarPanel topBarPanel;
    public UIBottomBarPanel bottomBarPanel;
    public UIEquipmentPanel equipmentPanel;

    private void Awake()
    {
        UIManager.Instance.hangarPanel = this;
    }
}
