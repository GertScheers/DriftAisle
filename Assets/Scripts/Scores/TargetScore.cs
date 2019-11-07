using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TargetScore : MonoBehaviour
{
    TextMeshProUGUI targetScore;

    void Start()
    {
        targetScore = GetComponent<TextMeshProUGUI>();
        targetScore.text = "Target Score: " + GameManager.Instance.TargetScore;
    }
}
