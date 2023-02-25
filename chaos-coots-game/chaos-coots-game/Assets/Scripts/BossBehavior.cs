using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class BossBehavior : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject pelletPrefab;
    public GameObject electricPrefab;
    public List<GameObject> summonPrefabs;

    public float pelletSpread = 200;
    public int pelletAmount = 36;

    public SpriteModifier modifier;
    public int maxHealth;
    public int currentHealth;
    public float timer;
    public float cooldown = 3;
    public bool launchingAttack = false;

    public Animator animator;
    private AudioSource audioSource;
    public List<AudioClip> soundsEffects;

    private Slider healthBar;
    public bool isBarMoving = false;
    public float barTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DataManager.AddSoundEffect(audioSource);

        healthBar = CanvasReference.Instance.bossBar;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.value != currentHealth)
        {
            StartCoroutine(LerpSlider());
        }

        if (launchingAttack) return;
        timer += Time.deltaTime;
        if (timer < cooldown) return;
        timer = 0;

        if(currentHealth > 2*maxHealth / 3)
        {
            int random = Random.Range(0, 5);
            switch (random)
            {
                case 0: StartCoroutine(SpawnPellets(1)); break;
                case 1: StartCoroutine(SpawnLasers(2, 1.5f)); break;
                case 2: StartCoroutine(Summon(1)); break;
                case 3: StartCoroutine(OrderedAttack(2, true)); break;
            }
        } else if(currentHealth > maxHealth / 3)
        {

            int random = Random.Range(0, 5);
            switch (random)
            {
                case 0: StartCoroutine(SpawnPellets(2)); break;
                case 1: StartCoroutine(SpawnLasers(3, 1.33f)); break;
                case 2: StartCoroutine(Summon(2)); break;
                case 3: StartCoroutine(OrderedAttack(3, true)); break;
            }
        }
        else if (currentHealth > 0)
        {

            int random = Random.Range(0, 5);
            switch (random)
            {
                case 0: StartCoroutine(SpawnPellets(6)); break;
                case 1: StartCoroutine(SpawnLasers(4, 1.25f)); break;
                case 2: StartCoroutine(Summon(3)); break;
                case 3: StartCoroutine(OrderedAttack(3, false)); break;
            }
        }


    }

    public IEnumerator OrderedAttack(int amount, bool stall)
    {
        launchingAttack = true;
        animator.SetBool("Warning", true);
        if (stall)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        
        

        List<bool> orders = new List<bool>(); //false = spawns on the right, true = spawns on left
                                              //false = arrow points to the right, true = points to the left
        for(int i = 0; i < amount; i++)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);

            int pos = Random.Range(0, 2);
            if (pos == 0)
            {
                orders.Add(false);
                animator.SetBool("Left", true);
                //Debug.Log("Go Right");
            }
            else
            {
                orders.Add(true);
                animator.SetBool("Right", true);
                //Debug.Log("Go Left");
            }
            animator.SetBool("Warning", false);
            yield return new WaitForSeconds(0.85f);
        }
        animator.SetBool("Warning", true);
        animator.SetBool("Left", false);
        animator.SetBool("Right", false);

        if (!stall)
        {
            yield return new WaitForSeconds(1.1f);
            animator.SetBool("Warning", false);
            launchingAttack = false;
        }

        foreach (bool order in orders)
        {
            Vector3 spawnPosition = Vector3.zero;
            if (order)
            {
                spawnPosition = electricPrefab.transform.position;
            }
            else
            {
                spawnPosition = new Vector3(6.5f, electricPrefab.transform.position.y);
            }
            spawnPosition += transform.position;
            Instantiate(electricPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(1.1f);
        }

        if (stall)
        {
            launchingAttack = false;
            animator.SetBool("Warning", false);
        }

        
    }

    public IEnumerator SpawnLasers(int amount, float delay)
    {
        launchingAttack = true;
        animator.SetBool("Laser", true);
        for(int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(delay);
            int pos = Random.Range(0, 2);
            if (pos == 0)
            {
                laserPrefab.GetComponent<BossLaser>().spinMultipler = 135;
                laserPrefab.GetComponent<BossLaser>().startRotation = 135;
            }
            else
            {
                laserPrefab.GetComponent<BossLaser>().spinMultipler = -135;
                laserPrefab.GetComponent<BossLaser>().startRotation = 45;
            }

            GameObject laser = Instantiate(laserPrefab, transform);
            
            
        }
        yield return new WaitForSeconds(1f);
        animator.SetBool("Laser", false);
        launchingAttack = false;
    }
    
    public IEnumerator SpawnPellets(int waves)
    {
        launchingAttack = true;
        for(int z = 0; z < waves; z++)
        {
            float offset = pelletSpread / 2;
            float directionFacing = 185 + Random.Range(0, 10f);

            for (int i = 1; i <= pelletAmount; i++)
            {
                float section = (float)i / pelletAmount;
                float degree = section * pelletSpread - pelletSpread / 2 - offset + directionFacing;
                GameObject pelletClone = Instantiate(pelletPrefab, transform.position, Quaternion.Euler(0, 0, degree));
                pelletClone.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * 10, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(1f);
        }
        
        launchingAttack = false;
    }
    public IEnumerator Summon(int amount)
    {
        launchingAttack = true;
        animator.SetBool("Summon", true);
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            int randomIndex = Random.Range(0, summonPrefabs.Count);

            Vector3 randomPos = Vector3.zero;
            //spawn them near the edge;
            int axis = Random.Range(0, 3);
            if (axis == 0) randomPos = new Vector3(Random.Range(-12f, 12f), -12f);
            else if (axis == 1) randomPos = new Vector3(-12f, Random.Range(-4f, -12f));
            else if (axis == 2) randomPos = new Vector3(12f, Random.Range(-4f, -12f));

            Instantiate(summonPrefabs[randomIndex], randomPos+transform.position, transform.rotation);
            
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Summon", false);
        launchingAttack = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth > 2 * maxHealth / 3)
        {
            cooldown = 3;
            CanvasReference.Instance.bossBarColor.color = Color.cyan;
        }
        else if(currentHealth > maxHealth / 3)
        {
            cooldown = 2;
            CanvasReference.Instance.bossBarColor.color = Color.yellow;

        } else if(currentHealth > 0)
        {
            cooldown = 0;
            CanvasReference.Instance.bossBarColor.color = Color.red;
        }

        if (currentHealth <= 0)
        {
            if (maxHealth > 0)
            {
                maxHealth = 0;
                DropItems drops = GetComponent<DropItems>();
                if (drops != null)
                {
                    drops.SpawnDrops();
                }

                GetComponent<Rigidbody2D>().simulated = false;
                if (GetComponent<BoxCollider2D>()) { GetComponent<BoxCollider2D>().enabled = false; }
                //StartCoroutine(CrashSprite());
            }

        }

        if (modifier != null)
        {
            modifier.GlitchSprite();
        }

    }

    private IEnumerator LerpSlider()
    {
        isBarMoving = true;
        while (barTimer < 1)
        {
            barTimer += Time.deltaTime * 1;
            healthBar.value = Mathf.Lerp(healthBar.value, currentHealth, barTimer);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        healthBar.value = currentHealth;
        isBarMoving = false;
    }
}
