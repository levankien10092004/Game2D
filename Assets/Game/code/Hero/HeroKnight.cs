using UnityEngine;
using System.Collections;

using UnityEditor;

public class HeroKnight : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] private LayerMask wallLayer;

    private Animator m_animator;
    private Rigidbody2D m_body2d;

    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;

    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    private float rollCooldown = 0.8f; // thời gian hồi lướt
    private float lastRollTime = -10f;

    [SerializeField] private HPNhanVat hp;
    [SerializeField] float mauht;
    [SerializeField] float mautd = 100;
    // Use this for initialization
    [SerializeField] private HPNhanVat mn;
    [SerializeField] float manaht;
    [SerializeField] float maanatd = 50;
    [SerializeField] float manaRegenRate = 2f;

    [SerializeField]private  float attackRange = 1.5f;          // độ dài vùng chém
    [SerializeField] private  float attackAngle = 90f;           // góc quét kiếm
    [SerializeField] private int  attackDamage = 10;
    [SerializeField] private LayerMask enemyLayers;

    private bool facingRight = true;          // hướng nhân vật
    private bool isAttacking = false;

    //menu lost
    [SerializeField] GameObject menu;

    //vat pham 
     private PlayerItems items;

    AudioManager audioManager;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        items = FindObjectOfType<PlayerItems>();

        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        attackDamage = PlayerPrefs.GetInt("PlayerDamage", 10);
        mautd = PlayerPrefs.GetInt("PlayerHealth", 100);
        maanatd = PlayerPrefs.GetInt("PlayerMana", 50);
        mau();
        mana();
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    void Update()
    {
        ManaRegen();
    
        m_timeSinceAttack += Time.deltaTime;

        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

       
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        float inputX = Input.GetAxis("Horizontal");

       
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
            facingRight = true;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
            facingRight = false;
        }

        // Move
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);


        //Attack
        if(Input.GetKeyDown(KeyBindings.Instance.AttackKey) && m_timeSinceAttack > 0.25f && !m_rolling&&manaht>=5)
        {
            audioManager.PlaySFX(audioManager.Attack);
            m_currentAttack++;
            

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            Attack();
          

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }


        // Roll
        else if (Input.GetKeyDown(KeyBindings.Instance.RollKey) && !m_rolling && Time.time >= lastRollTime + rollCooldown)
        {
            lastRollTime = Time.time;
            StartCoroutine(RollForward());
        }




        //Jump
        else if (Input.GetKeyDown(KeyBindings.Instance.JumpKey) && m_grounded && !m_rolling) 
        {
            audioManager.PlaySFX(audioManager.Jump);
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
        if (Input.GetKeyDown(KeyBindings.Instance.UseHPKey))
        {
            if (items.UseHPPotion())
            {
                HealHP(25); // hồi 30 máu
            }
        }
        if (Input.GetKeyDown(KeyBindings.Instance.UseManaKey))
        {
            if (items.UseManaPotion())
            {
                HealMana(20); // hồi 20 mana
            }
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
    public void mau()
    {
        mauht = mautd;
        hp.capNhatMau(mauht, mautd);
    }
    public void mana()
    {
        manaht = maanatd;
        mn.capNhatMau(manaht, maanatd);
    }

    public void TakeDamage(float mat)
    {
        mauht -= mat;
        hp.capNhatMau(mauht, mautd);
        audioManager.PlaySFX(audioManager.Hurt);
        m_animator.SetTrigger("Hurt");

        if (mauht <= 0)
        {
            mauht = 0;
            hp.capNhatMau(mauht, mautd);
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            m_body2d.velocity = Vector2.zero;
            this.enabled = false;
            GetComponent<PlayerCoins>()?.RestoreCoins();
            if (menu != null)
            {
                menu.SetActive(true);
                Time.timeScale = 0;
                audioManager.PlaySFX(audioManager.Lost);
            }
        }
    }
    public void Takemana(float mat)
    {
        manaht -= mat;
        if (manaht < 0)
            manaht = 0;
        mn.capNhatMau(manaht, maanatd);
       
    }
    public void HealHP(float amount)
    {
        mauht += amount;

        if (mauht > mautd)
            mauht = mautd; // không vượt quá máu tối đa

        hp.capNhatMau(mauht, mautd);
    }
    public void HealMana(float amount)
    {
        manaht += amount;

        if (manaht > maanatd)
            manaht = maanatd; // không vượt quá máu tối đa

        mn.capNhatMau(manaht, maanatd);
    }
    void ManaRegen()
    {
        if (manaht < maanatd)
        {
            manaht += manaRegenRate * Time.deltaTime;

            if (manaht > maanatd)
                manaht = maanatd;

            mn.capNhatMau((int)manaht, (int)maanatd);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            TakeDamage(2.5f);
        }
        if (collision.CompareTag("Heal"))
        {
            HealHP(25);
            Destroy(collision.gameObject); 
        }
    }
    //lướt
    IEnumerator RollForward()
    {
        m_rolling = true;
        m_animator.SetTrigger("Roll");

        float rollDuration = 0.3f;
        float rollDistance = 3f;
        float elapsed = 0f;

        Vector2 startPos = m_body2d.position;
        Vector2 targetPos = startPos + new Vector2(m_facingDirection * rollDistance, 0);

        while (elapsed < rollDuration)
        {
            elapsed += Time.deltaTime;
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, elapsed / rollDuration);

            // Kiểm tra va chạm trước khi di chuyển
            RaycastHit2D hit = Physics2D.Raycast(m_body2d.position, newPos - m_body2d.position, (newPos - m_body2d.position).magnitude, wallLayer);
            if (hit.collider != null)
            {
                // Dừng lướt khi gặp tường
                m_body2d.position = hit.point - new Vector2(m_facingDirection * 0.1f, 0);
                break;
            }

            m_body2d.MovePosition(newPos);
            yield return null;
        }

        m_rolling = false;
    }

    void Attack()
    {
        isAttacking = true;
        Takemana(5);
        // Lấy hướng tấn công
        Vector2 attackDir = facingRight ? Vector2.right : Vector2.left;

        // Tìm tất cả đối tượng trong phạm vi hình tròn
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Kiểm tra góc giữa nhân vật và kẻ địch
            Vector2 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(attackDir, dirToEnemy);

            if (angle < attackAngle / 2f)
            {
                enemy.GetComponent<Quai2>()?.TakeDamage(attackDamage);
                enemy.GetComponent<QuaithuongDC>()?.TakeDamage(attackDamage); 
                enemy.GetComponent<EnemyFly>()?.TakeDamage(attackDamage);
                enemy.GetComponent<EnemyThrower>()?.TakeDamage(attackDamage);  
                enemy.GetComponent<EnemyFlyAttack>()?.TakeDamage(attackDamage);
                enemy.GetComponent<EnemyAtaackrun>()?.TakeDamage(attackDamage);
                enemy.GetComponent<EnemyFlyThrower>()?.TakeDamage(attackDamage); 
                enemy.GetComponent<Boss1>()?.TakeDamage(attackDamage);
                enemy.GetComponent<Boss2>()?.TakeDamage(attackDamage);
                enemy.GetComponent<Boss3>()?.TakeDamage(attackDamage);
            } 
        } 

        isAttacking = false;
    }    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
