using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Wall : MonoBehaviour
{
    public GameObject iceEffectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Ground")
        {
            
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Debug.Log(pos);
            Instantiate(iceEffectPrefab, pos, transform.rotation);
            Destroy(gameObject);
        }

    }
}
