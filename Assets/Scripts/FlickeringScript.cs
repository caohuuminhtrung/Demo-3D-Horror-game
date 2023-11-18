using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringScript : MonoBehaviour
{
    [SerializeField] private EnemyController enemy;

    bool isFlicker;

    void Start()
    {
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
            yield return new WaitForSeconds(Random.Range(1f, 3.2f));
            if (!isFlicker)
            {
                transform.GetComponent<Light>().enabled = false;
            }
            if (isFlicker)
            {
                transform.GetComponent<Light>().enabled = false;
                yield return new WaitForSeconds(Random.Range(0.15f, 0.89f));
                transform.GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(Random.Range(0.4f, 2.7f));
                transform.GetComponent<Light>().enabled = false;
                yield return new WaitForSeconds(Random.Range(0.21f, 1.1f));
                transform.GetComponent<Light>().enabled = true;
            }

        }

    }
}
