using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenApple : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "OtherColliders"){
            Destroy(gameObject);
        }
    }
}
