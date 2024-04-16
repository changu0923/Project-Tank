using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public UITitlePanel titlePanel;

    private void Awake()
    {
        Instance = this;
    }
}
