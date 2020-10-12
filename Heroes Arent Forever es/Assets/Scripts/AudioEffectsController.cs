using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectsController : MonoBehaviour
{	
	public AudioSource powerUp;
    public AudioSource restoreHealth;
    // Start is called before the first frame update
    void Start()
    {
        powerUp = GetComponent<AudioSource>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PowerUp(){            
    	powerUp.Play();
    }
    public void RestoreHealth(){
        restoreHealth.Play();
    }
}
