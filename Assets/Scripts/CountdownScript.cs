using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownScript : MonoBehaviour
{
    TextMeshProUGUI countdown;

    private void OnEnable()
    {
        countdown = GetComponent<TextMeshProUGUI>();

        countdown.text = "3";
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            countdown.text = (count - i).ToString();
            yield return new WaitForSeconds(1);
        }

        FindObjectOfType<GameManager>().CountDownComplete();
    }
}
