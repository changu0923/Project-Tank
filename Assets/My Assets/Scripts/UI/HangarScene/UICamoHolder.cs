using System.Collections;
using System.Collections.Generic;
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
    private bool isSettingCamo = false; // 중복 호출 방지를 위한 플래그

    private void OnEnable()
    {
        isCamoButtonOn = false;
        selectedTankData = DatabaseManager.Instance.SelectedTank;
        InitButton();
        InitToggle();
        SetCamo(selectedTankData.CamoSlot);
    }

    private void OnDisable()
    {
        camoButton.onClick.RemoveListener(OnButtonClicked);
        foreach (Toggle toggle in camoToggles)
        {
            toggle.onValueChanged.RemoveListener(OnToggleSelected);
        }
        selectedTankData = null;
    }

    private void InitButton()
    {
        camoButton.onClick.AddListener(OnButtonClicked);
    }

    private void InitToggle()
    {
        camoContentHolder.gameObject.SetActive(true);
        for (int i = 0; i < camoToggles.Count; i++)
        {
            if (i == selectedTankData.CamoSlot)
            {
                print("camoToggles : " + i + " is isON");
                camoToggles[i].isOn = true;
            }
            camoToggles[i].onValueChanged.AddListener(OnToggleSelected);
        }
        camoContentHolder.gameObject.SetActive(false);
    }

    private void OnButtonClicked()
    {
        isCamoButtonOn = !isCamoButtonOn;
        camoContentHolder.gameObject.SetActive(isCamoButtonOn);
    }

    private void OnToggleSelected(bool value)
    {
        if (isSettingCamo || !value) 
            return;

        int index = camoToggles.FindIndex(toggle => toggle.isOn);
        if (index >= 0)
        {
            SetCamo(index);
        }
    }

    private void SetCamo(int index)
    {
        if (isSettingCamo) 
            return;
        StartCoroutine(SetCamoCoroutine(index));
    }

    public void SetCamoOnUI(int index)
    {
        Image currentCamoImage = camoButton.GetComponent<Image>();
        Sprite targetSprite = UIManager.Instance.hangarPanel.ModelList.CamoSprites[index];
        currentCamoImage.sprite = targetSprite;
    }

    public void SetCamoOnVehicle(int index)
    {
        if (UIManager.Instance.hangarPanel.VehicleSpawnPoint.GetChild(0).TryGetComponent<TankStat>(out TankStat currentVehicle))
        {
            Material targetMaterial = UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index];
            currentVehicle.SetVehicleCamo(targetMaterial);
        }
    }

    private IEnumerator SetCamoCoroutine(int index)
    {
        isSettingCamo = true;
        UIManager.Instance.hangarPanel.loadingPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SetCamoOnUI(index);
        SetCamoOnVehicle(index);
        DatabaseManager.Instance.ChangeCamo(index);
        UIManager.Instance.hangarPanel.loadingPanel.gameObject.SetActive(false);
        isSettingCamo = false; // 설정 완료 상태로 플래그 해제
    }
}
