using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    private List<Rigidbody2D> EnemyRbs;

    [Header("Character attributes:")]
	public float MOVEMENT_BASE_SPEED = 1.0f;	

	[Space]
	[Header("Character statistics:")]
	public Vector2 movementDirection;
	public float enemyHealth = 3.0f;
	public float movementSpeed;
	public float mindistaceToPlayer = 1.0f;
    public float maxdistaceToPlayer = 5.0f;
    public float repelRange = 0.5f;
    public float damageReduction;

	[Space]
	[Header("References:")]
	private Rigidbody2D rb;
	public Animator animator;
	private Transform target;
    public SpriteRenderer sprite;
    public string RoomName;           

    public bool playerInRoom;

    void Awake()
    {        
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        if(EnemyRbs == null){
            EnemyRbs = new List<Rigidbody2D>();            
        }
        EnemyRbs.Add(rb);
    }

    void OnDestroy(){        
        EnemyRbs.Remove(rb);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRoom && target){
            if(enemyHealth <= 0){
                GameObject.Find(RoomName).GetComponent<Room>().enemiesInRoom.Remove(gameObject);
            	Destroy(gameObject);
            }
            if(Vector2.Distance(transform.position, target.position) > mindistaceToPlayer){
            	transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * MOVEMENT_BASE_SPEED * Time.deltaTime);
            	Vector2 v = target.position - transform.position;            
                movementDirection = new Vector2(v.x, v.y);	
                movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
            	movementDirection.Normalize();
            }else{
                movementDirection = new Vector2(0, 0);  
                movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
                movementDirection.Normalize();
            }        
            if(movementDirection != Vector2.zero){
            	animator.SetFloat("Horizontal", movementDirection.x);
        		animator.SetFloat("Vertical", movementDirection.y);            
            }            
        }else{
            movementSpeed = 0;
        }      
        animator.SetFloat("Speed", movementSpeed);
    }

    void FixedUpdate(){
        Vector2 repelForce = Vector2.zero;
        foreach(Rigidbody2D enemy in EnemyRbs){
            if(enemy == rb){
                continue;
            }

            if(Vector2.Distance(enemy.position, rb.position) <= repelRange){
                Vector2 repelDir = (rb.position - enemy.position).normalized;
                repelForce += repelDir;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.name == "OtherColliders"){
            movementSpeed = 0;
            Debug.Log("Player Hit");
        }       
    }
}
