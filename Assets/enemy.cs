using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Color c = rend.material.color;
        c.a = 0f;
        rend.material.color = c;

        player = rocketPhysics.s.gameObject;

        Invoke("fireBullets", 4.0f);
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
    void fireBullets()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        Transform foo;
        int level = rocketPhysics.level;

        foo = transform;
        bullet.transform.position = foo.position;
        bullet.transform.rotation = foo.rotation;
        Vector3 vel = bullet.GetComponent<Rigidbody2D>().velocity;
        bullet.GetComponent<Rigidbody2D>().AddForce(vel + (foo.up * 10 * 20.0f * (1+(level/4))));
        Destroy(bullet, 2.0f);
        Invoke("fireBullets", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 npos = player.transform.position - transform.position;
        npos += transform.up;
        npos /= 2.0f;

        transform.up = npos.normalized;

        if (Random.value < .5f)
            GetComponent<Rigidbody2D>().AddForce(transform.up * 5.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided: "+ collision.gameObject.tag);

        if (collision.gameObject.tag == "bullet")
        {
            player.GetComponent<rocketPhysics>().IncrementScore();

            GameObject xploder = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(xploder, 6f);

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
