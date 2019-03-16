using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraLifeController : MonoBehaviour
{

    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Color c = rend.material.color;
        c.a = 0f;
        rend.material.color = c;
        StartCoroutine("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {

        for (float f = .05f; f <= 1.0f; f += 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(5f);

        for (float f = 1f; f >= .0f; f -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(.05f);
        }

        Color x = rend.material.color;
        x.a = 0f;
        rend.material.color = x;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rocketPhysics.AddLife();
    }
}
