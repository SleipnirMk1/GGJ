using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayTransition : MonoBehaviour
{
    public GameObject fadeScreen;
    public TMP_Text dayText;

    public Animator fadeAnimator;

    public void TransitionDay()
    {
        StartCoroutine(Execute());
    }

    IEnumerator Execute()
    {
        dayText.text = DayTracker.Instance.GetDay().ToString();

        fadeAnimator.Play("Transition");

        yield return new WaitForSeconds(90f);
    }
}
