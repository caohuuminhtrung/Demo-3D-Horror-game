using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinningCutscene : MonoBehaviour
{
    private readonly int FadeInHash = Animator.StringToHash("Winning Fade In");
    private readonly int FadeOutHash = Animator.StringToHash("Winning Fade Out");
    [SerializeField] private GameManager gameManager;

    [SerializeField] AudioSource soundSource;
    [SerializeField] private TextMeshProUGUI timerText;
    private Animator animator;
    private float timer = 0f;
    private bool startCutScene = false;
    private float blinkInterval = 0.5f; 
    private float initialDelay = 0.2f;
    public void StartCutScene(){
        animator = GetComponent<Animator>();
        StartCoroutine(CutScene());
    }
    IEnumerator CutScene()
    {           
        animator.Play(FadeInHash);
        timerText.enabled = false;
        yield return new WaitForSeconds(initialDelay);
        Debug.Log(timer);
        if(!startCutScene){
            startCutScene = true;
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        timerText.enabled = true;
        timerText.text = "05:59";
        while (timer < 0.02f)
        {   
            timer += Time.deltaTime;
            yield return new WaitForSeconds(blinkInterval);
        }

        soundSource.Play();
        timerText.text = "06:00";
        while(timer < 0.08f){
            timer += Time.deltaTime;
            timerText.enabled = !timerText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        soundSource.Stop();
        timerText.enabled = true;
        yield return new WaitForSeconds(blinkInterval);
        animator.Play(FadeOutHash);
        yield return new WaitForSeconds(1f);
        gameManager.MainMenu();
    }
}
