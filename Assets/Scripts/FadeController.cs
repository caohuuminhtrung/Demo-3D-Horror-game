using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
  public Image blackScreen;
  public float fadeDuration = 1.0f;

  public void SetBlack() {
    blackScreen.color = Color.black;
    blackScreen.CrossFadeAlpha(0, 0, true);
  }

  public IEnumerator FadeIn()
  {
    blackScreen.CrossFadeAlpha(1, fadeDuration, false);
    yield return new WaitForSeconds(fadeDuration);
  }

  public IEnumerator FadeOut()
  {
    blackScreen.CrossFadeAlpha(0, fadeDuration, false);
    yield return new WaitForSeconds(fadeDuration);
  }
}
