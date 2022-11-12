using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float speed;
    Rigidbody2D rb;
    bool facingRight = true;

    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;

    bool isTouchingFront;
    public Transform frontCheck;
    bool wallJumping;
    public float wallJumpTime;
    public float xWallForce;
    public float yWallForce;
    bool wallSliding;
    public float wallSlidingSpeed;

    Animator anim;
    public int fullHealth;
    public int health;
    public int lives; 
    public int level = 1;

    public GameObject gameoverPanel;

    public float timeBetweenAttacks;
    float nextAttackTime;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;

    public int damage;

    public SpriteRenderer weaponRenderer;

    public GameObject blood;
    public GameObject deathEffect;
    public GameObject pickupEffect;
    public GameObject swordSwingEffect;
    public GameObject dropEffect;

    AudioSource source;

    public AudioClip jumpSound;

    public Checkpoint checkPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        Debug.Log("Weapon " + StaticClass.weapon);

        if(StaticClass.weapon != null)
        {
            Weapon newWeapon = new Weapon();
            newWeapon.GFX = StaticClass.GFX;
            newWeapon.attackRange = StaticClass.attackRange;
            newWeapon.damage = StaticClass.damage;
            Equip(newWeapon);
        }

        gameObject.transform.SetPositionAndRotation(FindObjectOfType<Checkpoints>().GetComponent<Checkpoints>().checkpoints[0].transform.position, new Quaternion(0, 0, 0, 0));
    }
   
    private void Update()
    {

        if (Time.time > nextAttackTime)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                
                anim.SetTrigger("attack");
                nextAttackTime = Time.time + timeBetweenAttacks;
            }
        }


        float input = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(input * speed, rb.velocity.y);

        if (input > 0 && facingRight == false)
        {
            Flip();
        } else if (input < 0 && facingRight == true) {
            Flip();
        }

        if (input != 0)
        {
            anim.SetBool("isRunning", true);
        } else {
            anim.SetBool("isRunning", false);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        if (isGrounded == true)
        {
            anim.SetBool("isJumping", false);
        } else {
            anim.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            source.clip = jumpSound;
            source.Play();
        }


        if (isTouchingFront && !isGrounded && input != 0)
        {
            wallSliding = true;
        } else {
            wallSliding = false;
        }

        if (wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping)
        {
            rb.velocity = new Vector2(xWallForce * -input, yWallForce);
            source.clip = jumpSound;
            source.Play();
        }
        if(this.transform.position.y < -100)
        {

            //gameObject.transform.position.Set(32, 5,0);
            TakeDamage(1);
            gameObject.transform.SetPositionAndRotation(checkPoint.transform.position, new Quaternion(0,0,0,0));
            //Destroy(gameObject);
        }


    }

    void Flip() {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    void SetWallJumpingToFalse() {
        wallJumping = false;
    }
    
    public void Respawn()
    {
      if(lives > 0)
      {
        lives--;
        gameObject.transform.position = gameObject.GetComponent<Player>().checkPoint.transform.position;
        gameObject.GetComponent<Player>().health = fullHealth;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        /*GameObject respawnCharacter = gameObject;
        player.GetComponent<Player>().enabled = true;
        player.GetComponent<Player>().lives = lives;
        player.GetComponent<Player>().health = fullHealth;
        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<AudioSource>().enabled = true;
        player.GetComponent<CircleCollider2D>().enabled = true;
        Player.Instantiate(player, checkPoint.transform.position, Quaternion.identity);*/
        //Destroy(gameObject);
      }
      else
      {
      GameMaster gm = GameObject.FindObjectOfType<GameMaster>();
      gm.DestroyCharacter();
        StartCoroutine(GameOver("CharacterSelect"));
      }

      //GameObject newCharacter = Instantiate(respawnCharacter);
      //newCharacter.GetComponent<Player>().enabled = true;
    }

  IEnumerator GameOver(string sceneName)
  {
    gameoverPanel = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;
    gameoverPanel.SetActive(true);    
    yield return new WaitForSeconds(1f);
    Destroy(gameObject);
    SceneManager.LoadScene(sceneName);
  }
  public void TakeDamage(int damage) {
      FindObjectOfType<CameraShake>().Shake();
      health -= damage;
      print(health);
      if (health <= 0)
      {
          Instantiate(deathEffect, transform.position, Quaternion.identity);
          //Destroy(gameObject);
          Respawn();
            
      } else {
          Instantiate(blood, transform.position, Quaternion.identity);
      }
  }

  public void Attack() {

      Instantiate(swordSwingEffect, attackPoint.position, Quaternion.identity);
      FindObjectOfType<CameraShake>().Shake();
      Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
      foreach (Collider2D col in enemiesToDamage)
      {
          col.GetComponent<Enemy>().TakeDamage(damage);
      }

  }

  private void OnDrawGizmosSelected()
  {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(attackPoint.position, attackRange);
  }

  public void Equip(Weapon weapon) {
      damage = weapon.damage;
      attackRange = weapon.attackRange;
      weaponRenderer.sprite = weapon.GFX;
      Instantiate(pickupEffect, transform.position, Quaternion.identity);
    
    if (weapon.name != null)
    {

      StaticClass.damage = weapon.damage;
      StaticClass.attackRange = weapon.attackRange;
      StaticClass.GFX = weapon.GFX;

      Destroy(weapon.gameObject);
      Debug.Log("Deleting Wep" + StaticClass.weapon);
    }
      
  }

  public void Land() {
      Vector2 pos = new Vector2(groundCheck.position.x, groundCheck.position.y + 1);
      Instantiate(dropEffect, pos, Quaternion.identity);
  }

}
