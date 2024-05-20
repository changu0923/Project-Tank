using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICamoHolder : MonoBehaviour
{
    [Header("Camo Holder")]
    [SerializeField] Button camoButton;
    [SerializeField] ScrollRect camoContentHolder;
    [SerializeField] VerticalLayoutGroup camoContent;
    [SerializeField] List<Toggle> camoToggles = new List<Toggle>();
    
    private bool isCamoButtonOn;
    private TankData selectedTankData;

    private void OnEnable()
    {
        isCamoButtonOn = false;
        selectedTankData = DatabaseManager.Instance.SelectedTank;
        SetCamo(selectedTankData.CamoSlot);
        InitButton();
        InitToggle();
    }

    private void InitButton()
    {
        camoButton.onClick.AddListener(OnButtonClicked);
    }

    private void InitToggle()
    {
        foreach (Toggle toggle in camoToggles) 
        {
            toggle.onValueChanged.AddListener(OnToggleSelected);
        }
    }

    private void OnButtonClicked()
    {
        isCamoButtonOn = !isCamoButtonOn;
        camoContentHolder.gameObject.SetActive(isCamoButtonOn);
    }

    private void OnToggleSelected(bool value)
    {
        Toggle selectedToggle;
        int index = 0;
        foreach (Toggle toggle in camoToggles)
        {        
            if(toggle.isOn)
            {
                selectedToggle = toggle;
                break;    
            }
            index++;
        }
        SetCamo(index);        
    }

    private void SetCamo(int index)
    {
        StartCoroutine(SetCamoCoroutine(index));
    }
    
    public void SetCamoOnUI(int index)
    {
        Image currentCamoImage = camoButton.GetComponent<Image>();
        Sprite targetSprite = UIManager.Instance.hangarPanel.ModelList.CamoSprites[index];
        currentCamoImage.sprite = targetSprite;
        camoToggles[index].isOn = true;
    }

    public void SetCamoOnVehicle(int index)
    {
        TankStat currentVehicle;
        if (UIManager.Instance.hangarPanel.VehicleSpawnPoint.GetChild(0).TryGetComponent<TankStat>(out currentVehicle))
        {            
            Material targetMaterial = UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index];
            currentVehicle.SetVehicleCamo(targetMaterial);
        }
    }

    IEnumerator SetCamoCoroutine(int index)
    {
        UIManager.Instance.hangarPanel.loadingPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SetCamoOnUI(index);
        SetCamoOnVehicle(index);
        DatabaseManager.Instance.ChangeCamo(index);
        UIManager.Instance.hangarPanel.loadingPanel.gameObject.SetActive(false); 
    }
}
    

