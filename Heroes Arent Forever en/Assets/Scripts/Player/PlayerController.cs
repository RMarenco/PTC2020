using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{	
    [Header("Character attributes:")]
	public float MOVEMENT_BASE_SPEED = 1.0f;
    public static float MOVEMENT_BASE_SPEED_STATIC;
	public float CROSSHAIR_DISTANCE = 1.0f;
	public float PROJECTILE_BASE_SPEED = 1.0f;
	public float AIMING_BASE_PENALTY = 1.0f;    
    public bool hit = false;
    public bool triggerTopWall = false;    
    public int specialPrefabPosible = 0;     

    //Aquí
    public int numOfHits;
    float lastClickedTime = 0;
    public float maxComboDelay = 0;   
    public bool canMove = true;
    public bool canAttack = true;
    public bool canShield = true;
    public escudo shieldObject; 
    public power poder;
    public GameObject gameOver;   
    //Aquí

	[Space]
	[Header("Character statistics:")]
	public Vector2 movementDirection;
	public Vector2 lookingDirection;
	public static float movementSpeed;
	public bool endOfAiming;
	public bool isAiming;		
	public bool shooting;

    public float healthInt;
    public static float playerHealth;
    public static float currentHealth = 0;
    public bool special;         
    public static string gem = "fire";
    public bool ableToShoot;

	[Space]
	[Header("References:")]
	public Rigidbody2D rb;
	public Animator animator;
	public GameObject crosshair;
	
    public CircleCollider2D feetHitbox;
    public BoxCollider2D chestHitbox;
    public GameObject weapon;
    public GameObject[] SpecialWeapons;
    public GameObject skillBar;
    public GameObject FireGemBox;
    public GameObject IceGemBox;
    public HealthBar healthBar;    

	[Space]
	[Header("Prefabs")]
    public GameObject InitialPower;
	public static GameObject projectilePrefab;
    public GameObject specialPrefab;    
    public VariableJoystick LeftvariableJoystick;
    public VariableJoystick RightvariableJoystick;
    public JoyButton normalAttackButton;
    public JoyButton SpecialAttackButton;
    

    // Start is called before the first frame update
    void Start()
    {        
        if(MOVEMENT_BASE_SPEED_STATIC == 0){
            MOVEMENT_BASE_SPEED_STATIC = MOVEMENT_BASE_SPEED;
            Debug.Log(MOVEMENT_BASE_SPEED_STATIC);
        }
        if(projectilePrefab == null){
            projectilePrefab = InitialPower;
        }
        if(currentHealth == 0){
            projectilePrefab = InitialPower;
            DamageEnemy.damage = 1.0f;
            Projectile.iceDamage = 1.0f;
            Projectile.fireDamage = 1.0f;   
            gem = "fire";
            MOVEMENT_BASE_SPEED_STATIC = MOVEMENT_BASE_SPEED;
            playerHealth = healthInt;
            currentHealth = playerHealth;            
        }   
        if(gameObject.name == "Rowena"){
            if(gem == "fire"){
                FireGemBox.SetActive(true);
            }else if(gem == "ice"){
                IceGemBox.SetActive(true);
            }
        }
        healthBar.SetMaxHealth(healthInt);  
        healthBar.SetHealth(currentHealth);             
        skillBar = GameObject.Find("EventManager");        
    }

    // Update is called once per frame
    void Update()
    {    	
        if(gameObject.name == "Rowena"){
            if(Time.timeScale == 1){
                ableToShoot = skillBar.GetComponent<SkillBarController>().GemIsCooldown;
                ProcessInputs();
                Move();
                Animate();
                Aim();
                Shoot();
                playerDeath();
                ActivateSpecial();
                if(crosshair.transform.localPosition.y < -1.8f || crosshair.transform.localPosition.y < 0 || (movementDirection.x < 0 || movementDirection.x > 0)){
                    triggerTopWall = false;                    
                }
                if(crosshair.transform.localPosition.y > 1.8f || crosshair.transform.localPosition.y > 0){
                    triggerTopWall = true;                    
                }
            }
        }else if(gameObject.name == "Asus"){
            if(Time.timeScale == 1){
                
                if(canShield){
                    playerShielding();
                }
                if (canAttack){
                    AsusAttack();   
                }
                if (canMove){
                    ProcessInputs();
                    MovementPlayer();  
                }
                if(power.currentPower >= poder.maxPower){
                    specialAttack();
                }
                die();
                
            }
        }
    }

    void ProcessInputs(){
    	lookingDirection = new Vector2(Input.GetAxis("Mouse X") + RightvariableJoystick.Horizontal, Input.GetAxis("Mouse Y") + RightvariableJoystick.Vertical);
    	//movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementDirection = new Vector2(LeftvariableJoystick.Horizontal + Input.GetAxis("Horizontal"), LeftvariableJoystick.Vertical + Input.GetAxis("Vertical"));
    	movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
    	movementDirection.Normalize();
    	lookingDirection.Normalize();        

        //#if UNITY_STANDALONE_WIN
            endOfAiming = Input.GetButtonUp("Fire1");
            isAiming = Input.GetButton("Fire1");
            special = Input.GetButtonUp("Fire2");       

            if(isAiming || shooting || skillBar.GetComponent<SkillBarController>().GemIsCooldown){
                movementSpeed *= AIMING_BASE_PENALTY;
            }

            if(endOfAiming && !shooting && skillBar.GetComponent<SkillBarController>().GemIsCooldown){          
                StartCoroutine(shootingTimer());
                shooting = true;                       
            }
        //#endif
    	
        //#if UNITY_WEBGL
            /*if((!isAiming && normalAttackButton.Pressed) && !skillBar.GetComponent<SkillBarController>().GemIsCooldown){
                movementSpeed = movementSpeed * AIMING_BASE_PENALTY;
                isAiming = true;
            }

            if((isAiming && !normalAttackButton.Pressed) && !shooting && !skillBar.GetComponent<SkillBarController>().GemIsCooldown){           
                isAiming = false;
                endOfAiming = true;
                StartCoroutine(shootingTimer());
                shooting = true;                       
            }else{
                endOfAiming = false;
            }*/
        //#endif
    }

    void Move(){
    	rb.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED_STATIC;        
        if(currentHealth == 0){
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator shootingTimer() {
    	animator.Play(0,0,0);
	    yield return new WaitForSecondsRealtime(1);	    	    
	    shooting = false;	 	   
	}	

    void Animate(){
    	if(movementDirection != Vector2.zero && movementSpeed != 0){
    		animator.SetFloat("Horizontal", movementDirection.x);
    		animator.SetFloat("Vertical", movementDirection.y);
    	}else if(lookingDirection != Vector2.zero && movementSpeed == 0){
    		animator.SetFloat("Horizontal", lookingDirection.x);
    		animator.SetFloat("Vertical", lookingDirection.y);
    	}
    	animator.SetFloat("Speed", movementSpeed);

    	if(isAiming && !skillBar.GetComponent<SkillBarController>().GemIsCooldown){    		
    		animator.SetFloat("AimingState", 0.5f);
    	}else if(shooting && skillBar.GetComponent<SkillBarController>().GemIsCooldown){    	        		
    		animator.SetFloat("AimingState", 1.0f);     		
    	}else{
    		animator.SetFloat("AimingState", 0.0f);
    	}
    }

    void Aim(){
    	if(movementDirection != Vector2.zero && movementSpeed != 0){
            crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE; 
            weapon.transform.localPosition = movementDirection * 0.4f; 
    	}else if(lookingDirection != Vector2.zero && movementSpeed == 0){
    		crosshair.transform.localPosition = lookingDirection * CROSSHAIR_DISTANCE;	
            weapon.transform.localPosition = lookingDirection * 0.4f; 
    	}        
    }

    void Shoot(){
    	Vector2 shootingDirection = crosshair.transform.localPosition;
    	shootingDirection.Normalize();

    	if(endOfAiming && !ableToShoot){     
            skillBar.GetComponent<SkillBarController>().GemIsCooldown = true;
            GameObject projectile = Instantiate(projectilePrefab, weapon.transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();            
            projectileScript.velocity = shootingDirection * PROJECTILE_BASE_SPEED;
            projectileScript.player = gameObject;
            projectileScript.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);                   	
    	}
    }

    void ActivateSpecial(){
        if(skillBar.GetComponent<SkillBarController>().specialIsCooldown){            
            if(specialPrefabPosible < 8){
                foreach(GameObject SpecialWeapon in SpecialWeapons){
                    GameObject fireRingActivated = Instantiate(specialPrefab, SpecialWeapon.transform.position , Quaternion.identity);
                    SpecialWeapon.transform.position.Normalize();
                    SpecialAttack FireRing = fireRingActivated.GetComponent<SpecialAttack>();            
                    FireRing.velocity = SpecialWeapon.transform.localPosition * PROJECTILE_BASE_SPEED;
                    FireRing.player = gameObject;
                    specialPrefabPosible++;
                }                                                    
            }                                
        }else{
            specialPrefabPosible = 0;
        }        
    }        

    public IEnumerator HitBoxOff(){
        hit = true;
        chestHitbox.enabled = false;
        feetHitbox.enabled = false;
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(3);
        chestHitbox.enabled = true;
        feetHitbox.enabled = true;
        hit = false;
    }

    void playerDeath(){
        if(currentHealth == 0){
            animator.SetTrigger("Dead");
            Destroy(gameObject, 2);
            gameOver.SetActive(true);
        }
    }



        /*--ASUS--*/

    void MovementPlayer(){
        //movimiento
        
        movementDirection.x = Input.GetAxisRaw("Horizontal") + LeftvariableJoystick.Horizontal;
        movementDirection.y = Input.GetAxisRaw("Vertical") +  + LeftvariableJoystick.Vertical;

        if(movementDirection != Vector2.zero){
            canAttack = false;
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);

        }else{
            
            canAttack = true;
        }
        animator.SetFloat("Magnitud", movementDirection.sqrMagnitude);
        rb.MovePosition(rb.position + movementDirection * MOVEMENT_BASE_SPEED_STATIC * Time.deltaTime);
    }

    void AsusAttack(string attackAxis = "Fire1"){
        //ataque
         if(Time.time - lastClickedTime > maxComboDelay){
             numOfHits = 0;
        }

        if(Input.GetButtonDown(attackAxis)){
                canMove = false;
                canShield = false;
                lastClickedTime = Time.time;
                numOfHits++;
            if(numOfHits == 1){
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack1", true);

            }
                numOfHits = Mathf.Clamp(numOfHits, 0, 3);
        }
    }

    public void return1(){
        if(numOfHits >=2){
            animator.SetBool("Attack2", true);
            animator.SetBool("Attack1", false);
        }else{
            animator.SetBool("Attack1", false);
            numOfHits = 0;
            canMove = true;
            canShield = true;
        }
    }

    public void return2(){
        if(numOfHits >=3){
            animator.SetBool("Attack3", true);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack1", false);
        }else{
            animator.SetBool("Attack2", false);
            numOfHits = 0;
            canMove = true;
            canShield = true;
        }
    }

    public void return3(){
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        numOfHits = 0;
        canMove = true;
        canShield = true;
        
    }


    void playerShielding(string shieldAxis = "Fire2"){        
        if(Input.GetButton(shieldAxis)){

            shieldObject.TakeDamage(); 
            animator.SetBool("Shielding", true);
            canMove = false;
            canAttack = false;

            if(shieldObject.currentShield < 1){
                StartCoroutine("shieldBreak");
            }

        }else if(shieldObject.currentShield < shieldObject.maxShield){

            shieldObject.recoverShield(1);
            animator.SetBool("Shielding", false);

        }
    }

    IEnumerator shieldBreak(){
        animator.SetBool("Break", true);
        canShield = false;
        canMove = false;
        yield return new WaitForSeconds(2f);
        animator.SetBool("Break", false);
        canMove = true;
        canShield = true;
    }

    void specialAttack(string specialAxis = "Fire3"){
        if(power.currentPower == 100){
            if (Input.GetButtonDown(specialAxis))
            {
                StartCoroutine("startAttack");   
            }
        }
    } 

    IEnumerator startAttack(){
        poder.DecressPower();
        animator.SetTrigger("SpecialAttack");
        canMove = false;
        yield return new WaitForSeconds(.7f);
        canMove = true;
    }
    void die(){
        if (currentHealth <= 0)
        {
            canMove = false;
            animator.SetTrigger("Dead");
            Destroy(gameObject, 1);    
            gameOver.SetActive(true);
        }
    }

}
