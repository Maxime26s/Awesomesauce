using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity += new Vector3(0, Random.Range(10f, 15f), 0);
        rb.AddTorque(new Vector3(Random.Range(1f, 5f), Random.Range(0.5f, 1.5f), Random.Range(1f, 5f)) * Random.Range(150, 200));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
