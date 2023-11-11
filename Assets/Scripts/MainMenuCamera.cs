using System.Collections;
using System.Collections.Generic;
using Kino;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animation jumpScareAnimation;
    [SerializeField] private AnalogGlitch analogGlitch;
    private bool animationTriggered = false;

    private float timeToTriggerAnimation;
    private float initialJitter = 0.3f;
    private float initialVJump = 0;
    private float initialShake = 0;
    private float initialColorDrift = 0.1f;

    void Update()
    {
        Debug.Log(audioSource.time);
        if (audioSource.time > 20 && audioSource.time < 38)
        {
            float elapsedTime  = audioSource.time - 20;
            float duration = 18;
            float ratio = elapsedTime / duration;
            analogGlitch.scanLineJitter = Mathf.Lerp(initialJitter, 0.5f, ratio);
        }
        else if (audioSource.time > 60 && audioSource.time < 92)
        {
            float elapsedTime  = audioSource.time - 60;
            float duration = 32;
            float ratio = elapsedTime / duration;
            analogGlitch.scanLineJitter = Mathf.Lerp(initialJitter, 0.5f, ratio);
            analogGlitch.verticalJump = Mathf.Lerp(initialVJump, 0.2f, ratio);
            analogGlitch.horizontalShake = Mathf.Lerp(initialShake, 0.1f, ratio);
            analogGlitch.colorDrift = Mathf.Lerp(initialColorDrift, 0.2f, ratio);
        }
        else if (audioSource.time > 60 && audioSource.time < 90f){
            float elapsedTime  = audioSource.time - 60;
            float duration = 32;
            float ratio = elapsedTime / duration;
            analogGlitch.scanLineJitter = Mathf.Lerp(initialJitter, 0.5f, ratio);
            analogGlitch.verticalJump = Mathf.Lerp(initialVJump, 0.2f, ratio);
            analogGlitch.horizontalShake = Mathf.Lerp(initialShake, 0.1f, ratio);
            analogGlitch.colorDrift = Mathf.Lerp(initialColorDrift, 0.2f, ratio);
        }
        else if (audioSource.time > 94f && audioSource.time < 100){
            analogGlitch.scanLineJitter = 0.7f;
            analogGlitch.verticalJump = 0.3f;
            analogGlitch.horizontalShake = 0.2f;
            analogGlitch.colorDrift = 0.75f;
        }
        else if(audioSource.time > 100 && audioSource.time < 103.5f){
            float elapsedTime = 0.001f;
            float ratio = elapsedTime / 5;
            analogGlitch.verticalJump = Mathf.Lerp(initialVJump, 0f, ratio);
            analogGlitch.horizontalShake = Mathf.Lerp(initialShake, 0, ratio);
            analogGlitch.colorDrift = Mathf.Lerp(initialColorDrift, 0.1f, ratio);   
        }
        else if(analogGlitch.scanLineJitter > 0.3f || analogGlitch.verticalJump > 0 
                    || analogGlitch.horizontalShake > 0 || analogGlitch.colorDrift > 0.1f){
            float elapsedTime = 0.001f;
            float ratio = elapsedTime / 5;
            analogGlitch.scanLineJitter = Mathf.Lerp(initialJitter, 0.3f, ratio);
            analogGlitch.verticalJump = Mathf.Lerp(initialVJump, 0, ratio);
            analogGlitch.horizontalShake = Mathf.Lerp(initialShake, 0, ratio);
            analogGlitch.colorDrift = Mathf.Lerp(initialColorDrift, 0.1f, ratio);        
        }
    }


}
