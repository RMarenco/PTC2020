using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{
    Idle, Follow, Attack, Die
};

public enum EnemyType{
    Slime, Spike, Armadura
};
public class Enemies : MonoBehaviour
{
    public EnemyType theEnemy;
    
    public string RoomName;
    public bool playerInRoom;
    GameObject player;
    public EnemyState currentState = EnemyState.Idle;

    public float range;
    public float attackRange;
    public float speed;

    public bool dead = false;
    public Animator anim;

    private Vector3 dir;
    public Vida vida;

    [Header("Spike Settings")]
    public GameObject bulletPrefab;
    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject dropPrefab;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if(player){
            switch(currentState){
                case(EnemyState.Idle):
                    Idle();
                break;

                case(EnemyState.Follow):
                    Follow();
                break;

                case(EnemyState.Attack):
                    Attack();
                break;

                case(EnemyState.Die):
                    Die();
                break;
            }

            if(vida.currentHealth <= 0){
                currentState = EnemyState.Die;
            }

            dir =  player.transform.position;

            
            if(IsPlayerInRange(range) && currentState != EnemyState.Die){
                currentState = EnemyState.Follow;
            }else if(!IsPlayerInRange(range) && currentState != EnemyState.Die){
                currentState = EnemyState.Idle;
            }
            
            if(IsPlayerInAttackRange(attackRange) && currentState != EnemyState.Die && theEnemy != EnemyType.Slime){  
                currentState = EnemyState.Attack;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range); 
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }

    private bool IsPlayerInRange(float range){
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private bool IsPlayerInAttackRange(float attackRange){
        return Vector3.Distance(transform.position, player.transform.position) <= attackRange;
    }


    void Idle(){
        if(IsPlayerInRange(range)){
            currentState = EnemyState.Follow;
        }
        
    }

    void Follow(){
        Vector3 Dir = (dir - transform.position).normalized;    
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        anim.SetFloat("MoveX", Dir.x);
        anim.SetFloat("MoveY", Dir.y);
        if(theEnemy == EnemyType.Armadura){
            anim.SetBool("Walking", true);
        }
    }

    void Attack(){
        if(theEnemy == EnemyType.Armadura){
            
            if(IsPlayerInAttackRange(attackRange)){
                anim.SetBool("Attacking", true);
                anim.SetBool("Walking", false);
            }else if(!IsPlayerInAttackRange(attackRange)){
                anim.SetBool("Attacking", false);
            }
            

        }else if(theEnemy == EnemyType.Spike){

            Vector3 Dir = (dir - transform.position).normalized;
            anim.SetFloat("MoveX", Dir.x);
            anim.SetFloat("MoveY", Dir.y);

            if(timeBtwShots <= 0){
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }else{
                timeBtwShots -= Time.deltaTime;
            }
        }
    }


    void Die(){
        GameObject.Find(RoomName).GetComponent<Room>().enemiesInRoom.Remove(gameObject);
        anim.SetTrigger("muerto");
        speed = 0;
        Destroy(gameObject, 1); 
    }

    public void drop(){
        Instantiate(dropPrefab, transform.position + new Vector3(0, Random.Range(0, 0)), Quaternion.identity);
    }
}
