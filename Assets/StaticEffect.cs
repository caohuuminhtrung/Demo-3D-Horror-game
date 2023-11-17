using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class StaticEffect : MonoBehaviour
{
    [SerializeField] private Animation jumpScareAnimation;
    [SerializeField] private AnalogGlitch analogGlitch;

    private float initialJitter = 0.3f;
    private float initialVJump = 0;
    private float initialShake = 0;
    private float initialColorDrift = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float ratio = 35f;
        analogGlitch.scanLineJitter = Mathf.Lerp(initialJitter, 0.3f, ratio);
        analogGlitch.verticalJump = Mathf.Lerp(initialVJump, 0, ratio);
        analogGlitch.horizontalShake = Mathf.Lerp(initialShake, 0, ratio);
        analogGlitch.colorDrift = Mathf.Lerp(initialColorDrift, 0.1f, ratio);
    }
}
