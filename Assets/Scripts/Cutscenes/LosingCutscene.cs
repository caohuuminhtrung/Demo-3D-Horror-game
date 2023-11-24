using System.Collections;
using TMPro;
using UnityEngine;
public class LosingCutscene : MonoBehaviour
{
    private readonly int FadeInHash = Animator.StringToHash("Losing Fade In");
    private readonly int FadeOutHash = Animator.StringToHash("Losing Fade Out");
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] AudioSource soundSource;
    [SerializeField] Light enemyLight;
    private Animator animator;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Confined;
        animator = GetComponent<Animator>();
    }

    public void SetHintText(string hint)
    {
        hintText.text = hint;
    }

    public void PlayEndingSong()
    {
        soundSource.Play();
    }
    public void PlayEndingScene(){
        StartCoroutine(EndingScene());
    }
    
    public void PlayFlickerLight()
    {
        StartCoroutine(FlickerLight());
    }

    IEnumerator EndingScene(){
        animator.Play(FadeInHash);
        yield return new WaitForSeconds(2f);
        PlayFlickerLight();
        yield return new WaitForSeconds(8f);
        animator.Play(FadeOutHash);
        yield return new WaitForSeconds(2f);
        gameManager.MainMenu();
    }
    IEnumerator FlickerLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.89f));
            enemyLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.15f, 0.66f));
            enemyLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.3f, 1.89f));
            enemyLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.21f, 0.87f));
            enemyLight.enabled = true;
        }

    }
}
