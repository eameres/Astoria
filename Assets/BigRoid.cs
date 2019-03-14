using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRoid : MonoBehaviour
{
    public GameObject player;
    public GameObject explosionPrefab;

    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        player = rocketPhysics.s.gameObject;

        transform.localScale = new Vector3 ( Random.Range(.5f, 1.0f), Random.Range(.5f, 1.0f), 1f);

        rend = GetComponent<SpriteRenderer>();

        rend.flipX = Random.value > .5f;
        rend.flipY = Random.value > .5f;

        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * Random.Range(-5f, 5f), ForceMode2D.Impulse);
        gameObject.GetComponent<Rigidbody2D>().AddTorque (Random.Range(-5f, 5f));

        Color c = rend.material.color;
        c.a = 0f;
        rend.material.color = c;

        StartCoroutine("FadeIn");
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
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

        if (newPos.x > 10f)
            newPos.x = -10f;
        else if (newPos.x < -10f)
            newPos.x = 10f;

        if (newPos.y > 6f)
            newPos.y = -6f;
        else if (newPos.y < -6f)
            newPos.y = 6f;

        transform.position = newPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided: " + collision.gameObject.tag);

        if (collision.gameObject.tag == "bullet")
        {
            GameObject xploder = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(xploder, 6f);

            Destroy(collision.gameObject);

            player.GetComponent<rocketPhysics>().roidList.Remove(gameObject);
            Destroy(gameObject);
    
            player.GetComponent<rocketPhysics>().IncrementScore();
        }
    }
}
