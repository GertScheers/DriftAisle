using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PlayerScore : MonoBehaviour
{
    TextMeshProUGUI playerScore;

    void Start()
    {
        playerScore = GetComponent<TextMeshProUGUI>();
        playerScore.text = "Player Score: " + GameManager.Instance.Score;
    }
}
