using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    GameObject Player;
    public float minModifier = 7;
    public float maxModifier = 11;
    Vector3 _velocity = Vector2.zero;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");    
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Player.transform.position, ref  _velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
    }

}
