using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rocketPhysics : MonoBehaviour
{
    public float magnitude;
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    public GameObject extraLifePrefab;
    public GameObject roidPrefab;
    public static GameObject s;

    public GameObject boom;
    public List<GameObject> enemylist;
    public List<GameObject> roidList;
    public int score;
    public int level = 1;

    int roidCount = 2;

    bool reSpawning = false;

    GameObject[] markers;
    int lives = 4;
    bool shielded = true;

    // testing setters and getters 

    private int myIntField = 0;

    public int MyInt
    {
        // This is your getter.
        // it uses the accessibility of the property (public)
        get
        {
            return myIntField;
        }
        // this is your setter
        // Note: you can specify different accessibility
        // for your getter and setter.
        set
        {
            // The input of the setter is always called "value"
            // and is of the same type as your property definition
            myIntField = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (s == null)
            s = this.gameObject;
        else {
            Debug.LogError("tried to reinitialize rocket singleton!");
            return;
        }

        GameObject.Find("gameOver").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("RestartButton").transform.localScale = new Vector3(0, 0, 0);

        markers = GameObject.FindGameObjectsWithTag("playerMarker"); // create the array
        Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
        halo.enabled = true;
        shielded = true;
        Invoke("ShieldsOff", 2f);

        Invoke("SpawnEnemy",3f); 

        Invoke("SpawnExtralife", Random.Range(10f, 15f));

        for (int i = 0; i < roidCount; i++)
            SpawnRoid();

        roidCount *= 2;

        StartCoroutine("LevelScale");

    }

    IEnumerator LevelScale()
    {
        GameObject.Find("Level").GetComponent<Text>().text = "Level : " + level;

        for (float i = 0f; i <= 1.0f; i += .1f)
        {
            GameObject.Find("Level").transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(1f);

        for (float i = 1.0f; i >= 0f; i -= .1f)
        {
            GameObject.Find("Level").transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(.05f);
        }
        level++;
        GameObject.Find("Level").transform.localScale = new Vector3(0f,0f,0f);
        yield return new WaitForSeconds(.05f);
    }

    public void IncrementScore() { 
        score += 100;
        GameObject.Find("playerScore").GetComponent<Text>().text = "Score : " + score;

        if (roidList.Count == 0)
        {
            StartCoroutine("LevelScale");

            DestroyAllEnemies();

            for (int i = 0; i < roidCount; i++)
                SpawnRoid();

            if (roidCount > 8)
                roidCount = 2;

            roidCount *= 2;
        }
    }

    void SpawnRoid()
    {
        Vector3 playerPos = transform.position;
        Vector3 enemyPos;

        do
        {
            enemyPos = new Vector3(Random.Range(-8f, 8f), Random.Range(-4.9f, 4.9f), 0);
        } while (Vector3.Distance(playerPos, enemyPos) < 4f);

        GameObject roid = Instantiate(roidPrefab, enemyPos, Quaternion.identity);

        roidList.Add(roid);
    }

    void SpawnEnemy()
    {
        Vector3 playerPos = transform.position;
        Vector3 enemyPos;

        do {
            enemyPos = new Vector3(Random.Range(-8f, 8f), Random.Range(-4.9f, 4.9f), 0);
            } while (Vector3.Distance(playerPos, enemyPos) < 4f);

        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);

        enemylist.Add(enemy);

        Invoke("SpawnEnemy", 6.0f);
    }

    void DestroyAllEnemies()
    {
        while(enemylist.Count > 0)
        {
            GameObject tEnemy = enemylist[0];
            enemylist.RemoveAt(0);
            Destroy(tEnemy);
        }
    }

    void ReSpawnPlayer()
    {
        transform.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), 0);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        shielded = reSpawning = false;
    }

    void SpawnExtralife()
    {
        GameObject extra = Instantiate(extraLifePrefab, new Vector3(Random.Range(-8f, 8f), Random.Range(-4.9f, 4.9f), 0), Quaternion.identity);
        Destroy(extra, 7.1f);
        Invoke("SpawnExtralife", Random.Range(10f,15f));
    }

    void ShieldsOff() {
        Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
        halo.enabled = false;
        shielded = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((lives <= 0) || reSpawning)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
            halo.enabled = true;
            shielded = true;
            Invoke("ShieldsOff", 1.5f);
        }

        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * magnitude);
            GetComponent<ParticleSystem>().Play();
            if (!GetComponents<AudioSource>()[1].isPlaying)
                GetComponents<AudioSource>()[1].Play();
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (transform.InverseTransformDirection(GetComponent<Rigidbody2D>().velocity).y > 0)
                GetComponent<Rigidbody2D>().AddForce(transform.up * -magnitude);
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().AddTorque(2.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddTorque(-2.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform foo;
            GameObject[] bArray;

            bArray = GameObject.FindGameObjectsWithTag("bullet");

            if (bArray.Length > 5)
                return;

            GameObject bullet = Instantiate(bulletPrefab);

            foo = transform;
            bullet.transform.position = foo.position;
            bullet.transform.rotation = foo.rotation;
            Vector3 vel = bullet.GetComponent<Rigidbody2D>().velocity;
            bullet.GetComponent<Rigidbody2D>().AddForce(vel + (foo.up * magnitude * 40.0f));
            Destroy(bullet, 2.0f);
        }
    }
    private void FixedUpdate()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "ScreenEdge")
        {
            if (collision.gameObject.tag == "extraLife")
            {
                GetComponent<AudioSource>().Play();
                if (lives < 3)
                {
                    markers[lives].SetActive(true);
                    GameObject.Find("bonus").GetComponent<Text>().text = "";
                }

                lives += 1;

                if (lives > 3)
                    GameObject.Find("bonus").GetComponent<Text>().text = "+" + (lives - 3);
            }
            else if ((lives > 0) && !shielded)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;

                boom.transform.position  = transform.position;
                boom.GetComponent<ParticleSystem>().Play();
                boom.GetComponent<AudioSource>().Play();

                lives -= 1;

                if (lives < 4)
                    GameObject.Find("bonus").GetComponent<Text>().text = "";
                else 
                    GameObject.Find("bonus").GetComponent<Text>().text = "+" + (lives - 3);

                if (lives < 3)
                    markers[lives].SetActive(false);

                //Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
                //halo.enabled = true;

                if (lives > 0)
                {
                    reSpawning = shielded = true;
                    Invoke("ReSpawnPlayer", 3f);
                }
                else
                {
                    GameObject.Find("gameOver").transform .localScale = new Vector3(1, 1, 1);
                    GameObject.Find("RestartButton").transform.localScale = new Vector3(1, 1, 1);
                }
                DestroyAllEnemies();
            }
            if (collision.gameObject.tag == "roid")
                roidList.Remove(collision.gameObject);

            Destroy(collision.gameObject);

        }
    }

    IEnumerator Shields()
    {
        yield return new WaitForSeconds(.05f);
    }
}
