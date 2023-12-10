using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public Animator animation;
    public TMP_Text dayText;

    public void EndGame()
    {
        dayText.text = DayTracker.Instance.GetDay().ToString();
        animation.Play("Fading");
    }
}
