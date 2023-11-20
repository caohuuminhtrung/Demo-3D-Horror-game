using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LosingCutscene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Animator cameraBlinkAnimator;
    private readonly int BlinkHash = Animator.StringToHash("Blinking Camera");

    public void SetHintText(String hint){
        hintText.text = hint;
    }

    public void StartBlinkAnimation(){
        cameraBlinkAnimator.Play(BlinkHash);
    }
}
