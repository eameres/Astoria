using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEnemy : MonoBehaviour
{
    public int IHasProperty = 12;
    // Start is called before the first frame update
    void Start()
    {
        IHasProperty = 15;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(IHasProperty);
    }
}
