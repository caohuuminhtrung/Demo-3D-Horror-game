using System.Collections;
using TMPro;
using UnityEngine;
public class LosingCutscene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] AudioSource soundSource;
    [SerializeField] Light enemyLight;


    public void SetHintText(string hint){
        hintText.text = hint;
    }

    public void PlayEndingSong(){
        soundSource.Play();
    }

    public void PlayFlickerLight(){
        StartCoroutine(flickerLight());
    }
    IEnumerator flickerLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3.2f));
            enemyLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.15f, 0.89f));
            enemyLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.4f, 2.7f));
            enemyLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.21f, 1.1f));
            enemyLight.enabled = true;
        }

    }
}
