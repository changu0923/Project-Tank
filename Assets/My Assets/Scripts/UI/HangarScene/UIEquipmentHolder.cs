using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentHolder : MonoBehaviour
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
}
