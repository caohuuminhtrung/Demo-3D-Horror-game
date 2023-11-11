using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShaking : MonoBehaviour
{
    bool isTrembled = false;

    void Start()
    {
        StartCoroutine(Tremble());
    }
    IEnumerator Tremble()
    {
        isTrembled = true;
        while (isTrembled == true)
        {

            yield return new WaitForSeconds(Random.Range(2f, 4f));
            if (Random.Range(0, 8) == 0)
            {
                // Debug.Log("Start shaking");
                for (int i = 0; i < 20; i++)
                {
                    transform.localPosition += new Vector3(Random.Range(transform.localPosition.x - 0.08f, transform.localPosition.x + 0.05f), 0, 0);
                    yield return new WaitForSeconds(Random.Range(0f, 0.03f));
                    transform.localPosition = new Vector3(0, 0, 0);
                    yield return new WaitForSeconds(0.01f);
                    transform.localPosition -= new Vector3(Random.Range(transform.localPosition.x - 0.07f, transform.localPosition.x + 0.03f), 0, 0);
                    yield return new WaitForSeconds(Random.Range(0f, 0.02f));
                    transform.localPosition = new Vector3(0, 0, 0);
                    yield return new WaitForSeconds(0.01f);

                }
                transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
