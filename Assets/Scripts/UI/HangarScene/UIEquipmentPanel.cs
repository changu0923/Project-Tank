using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentPanel : MonoBehaviour
{
    [Header("Equipment Holder")]
    [SerializeField] Button equipSlot1Button;
    [SerializeField] Button equipSlot2Button;
    [SerializeField] Button equipSlot3Button;
    [SerializeField] ScrollRect equipSlot1Rect;
    [SerializeField] ScrollRect equipSlot2Rect;
    [SerializeField] ScrollRect equipSlot3Rect;
    [SerializeField] VerticalLayoutGroup equipSlot1Content;
    [SerializeField] VerticalLayoutGroup equipSlot2Content;
    [SerializeField] VerticalLayoutGroup equipSlot3Content;
    [SerializeField] List<Toggle> equipSlot1Toggles;
    [SerializeField] List<Toggle> equipSlot2Toggles;
    [SerializeField] List<Toggle> equipSlot3Toggles;
    private bool isEquipmentButtonOn;


    [Header("Camo Holder")]
    [SerializeField] Button camoButton;
    [SerializeField] ScrollRect camoContentHolder;
    [SerializeField] VerticalLayoutGroup camoContent;
    [SerializeField] List<Toggle> camoToggles = new List<Toggle>(); 
    private bool isCamoButtonOn;
    private void Awake()
    {
        camoButton.image.sprite = camoToggles[0].GetComponentInChildren<Image>().sprite;
        camoContentHolder.gameObject.SetActive(false);

        InitializeEquipmentHolder();

        camoButton.onClick.AddListener(OnCamoButtonClick);
        foreach (Toggle toggle in camoToggles) 
        {
            toggle.onValueChanged.AddListener(OnCamoToggleOn);
        }
    }
    #region EquipmentHolder

    private void InitializeEquipmentHolder()
    {
        equipSlot1Button.onClick.AddListener(EquipSlot1ButtonClick);
        equipSlot2Button.onClick.AddListener(EquipSlot2ButtonClick);
        equipSlot3Button.onClick.AddListener(EquipSlot3ButtonClick);

        foreach (Toggle toggle in equipSlot1Toggles)
        {
            toggle.onValueChanged.AddListener(OnEquipSlot1ToggleOn);
        }
        foreach (Toggle toggle in equipSlot2Toggles)
        {
            toggle.onValueChanged.AddListener(OnEquipSlot2ToggleOn);
        }
        foreach (Toggle toggle in equipSlot3Toggles)
        {
            toggle.onValueChanged.AddListener(OnEquipSlot3ToggleOn);
        }
    }

    private void EquipSlot1ButtonClick()
    {
        if (equipSlot1Rect.IsActive())
        {
            equipSlot1Rect.gameObject.SetActive(false);
            isEquipmentButtonOn = false;
        }
        else
        {
            equipSlot1Rect.gameObject.SetActive(true);
            isEquipmentButtonOn = true;
        }
        equipSlot2Rect.gameObject.SetActive(false);
        equipSlot3Rect.gameObject.SetActive(false);
    }
    private void EquipSlot2ButtonClick()
    {
        if (equipSlot2Rect.IsActive())
        {
            equipSlot2Rect.gameObject.SetActive(false);
            isEquipmentButtonOn = false;
        }
        else
        {
            equipSlot2Rect.gameObject.SetActive(true);
            isEquipmentButtonOn = true;
        }

        equipSlot1Rect.gameObject.SetActive(false);
        equipSlot3Rect.gameObject.SetActive(false);
    }
    private void EquipSlot3ButtonClick()
    {
        if (equipSlot3Rect.IsActive())
        {
            equipSlot3Rect.gameObject.SetActive(false);
            isEquipmentButtonOn = false;
        }
        else
        {
            equipSlot3Rect.gameObject.SetActive(true);
            isEquipmentButtonOn = true;
        }

        equipSlot1Rect.gameObject.SetActive(false);
        equipSlot2Rect.gameObject.SetActive(false);
    }

    private void OnEquipSlot1ToggleOn(bool bValue)
    {
        foreach(Toggle toggle in equipSlot1Toggles)
        {
            if(toggle.isOn)
            {
                // Change sprite
                Sprite newSprite = toggle.GetComponentInChildren<Image>().sprite;
                Image targetImage = equipSlot1Button.GetComponent<Image>();
                targetImage.sprite = newSprite;
            }
        }
    }
    private void OnEquipSlot2ToggleOn(bool bValue)
    {
        foreach (Toggle toggle in equipSlot2Toggles)
        {
            if (toggle.isOn)
            {
                Sprite newSprite = toggle.GetComponentInChildren<Image>().sprite;
                Image targetImage = equipSlot2Button.GetComponent<Image>();
                targetImage.sprite = newSprite;
            }
        }
    }
    private void OnEquipSlot3ToggleOn(bool bValue)
    {
        foreach (Toggle toggle in equipSlot3Toggles)
        {
            if (toggle.isOn)
            {
                Sprite newSprite = toggle.GetComponentInChildren<Image>().sprite;
                Image targetImage = equipSlot3Button.GetComponent<Image>();
                targetImage.sprite = newSprite;
            }
        }
    }

    #endregion


    #region CamoHolder
    private void OnCamoButtonClick()
    {
        isCamoButtonOn = !isCamoButtonOn;
        camoContentHolder.gameObject.SetActive(isCamoButtonOn);
    }

    private void OnCamoToggleOn(bool bValue)
    {
        int index = 0;
        foreach (Toggle toggle in camoToggles) 
        {
            if(toggle.isOn) 
            {
                Sprite newSprite = toggle.GetComponentInChildren<Image>().sprite;
                Image buttonImage = camoButton.GetComponent<Image>();
                buttonImage.sprite = newSprite;

                TankStat targetTank;
                bool result = UIManager.Instance.hangarPanel.VehicleSpawnPoint.GetChild(0).TryGetComponent<TankStat>(out targetTank);
                if(result)
                {
                    targetTank.SetVehicleCamo(UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index]);
                }
                DatabaseManager.Instance.ChangeCamo(index);
                break;
            }
            index++;
        }
    }
    #endregion


}

