using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AngleManager : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private StringBuilder sb = new StringBuilder(); 

    private void Awake()
    {
        debugText = GetComponent<TextMeshProUGUI>();
    }
 }
