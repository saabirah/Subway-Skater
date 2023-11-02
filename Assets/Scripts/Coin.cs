using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;

   
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();     
    }

    private void OnEnable()
    {
        anim.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.GetCoin();
            anim.SetTrigger("Collected");
            // Destroy(gameObject,1.0f);
            //Destroy(transform.parent.gameObject, 1.5f);//2
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
