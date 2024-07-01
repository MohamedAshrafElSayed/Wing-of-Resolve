using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
   /* public float targetRadius = 100f;  // Set the target size you want
    public float duration = 5f;    // Set the duration of the size change

    private SphereCollider collider;
    private float initialRadius;
    private float elapsedTime = 0f;

    void Start()
    {
        collider = GetComponent<SphereCollider>();

        if (collider != null)
        {
            initialRadius = collider.radius;  
            StartCoroutine(IncreaseSizeOverTime());
            
        }
        else
        {
            Debug.LogError("Collider not found on the GameObject!");
        }
        
    }

    void updateCollider()
    {
        if (collider != null)
        {
            elapsedTime += Time.deltaTime;

            float newSize = Mathf.Lerp(initialRadius, targetRadius, elapsedTime / duration);

            collider.radius = newSize;

            if (elapsedTime >= duration)
            {
                elapsedTime = 0f;
            }
        }
    }


    IEnumerator IncreaseSizeOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float newRadius = Mathf.Lerp(initialRadius, targetRadius, elapsedTime / duration);

            collider.radius = newRadius;

            yield return null;
            
        }

        elapsedTime = 0f;
        collider.radius = 0;
    }


    private void OnTriggerEnter(Collider other)
    {
      
        if(other.tag == "Scanner")
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }*/
}
