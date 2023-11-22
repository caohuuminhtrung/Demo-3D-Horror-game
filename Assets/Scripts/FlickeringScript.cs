using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringScript : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;

    bool isFlicker = false;

    void Start()
    {
        isFlicker = false;
        StartCoroutine(flickerLight());
    }
    void Update()
    {
        if (enemy.IsEnemyAtWindow())
        {
            isFlicker = true;
        }
        else
        {
            isFlicker = false;
        }
    }

    IEnumerator flickerLight()
    {
        // yield return new WaitForSeconds(0.1f);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.8f, 2.6f));
            if (!isFlicker)
            {
                transform.GetComponent<Light>().enabled = false;
            }
            if (isFlicker)
            {
                transform.GetComponent<Light>().enabled = false;
                yield return new WaitForSeconds(Random.Range(0.15f, 0.69f));
                transform.GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(Random.Range(0.4f, 2.2f));
                transform.GetComponent<Light>().enabled = false;
                yield return new WaitForSeconds(Random.Range(0.21f, 1.01f));
                transform.GetComponent<Light>().enabled = true;
            }

        }

    }
}
