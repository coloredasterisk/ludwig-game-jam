using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject pelletPrefab;
    public GameObject electricPrefab;
    public List<GameObject> summonPrefabs;
    public GameObject crashableClone;

    public float pelletSpread = 200;
    public int pelletAmount = 36;

    public SpriteModifier modifier;
    public int maxHealth;
    public int currentHealth;
    public float timer = 0;
    public float cooldown = 3;
    public bool alive = true;
    public bool launchingAttack = false;

    public Animator animator;
    private AudioSource audioSource;
    public List<AudioClip> soundEffects; //0 take damage sounds, 1 pellet shoot, 2 summon sound,3crash

    private Slider healthBar;
    public bool isBarMoving = false;
    public bool sequence = false;
    public float barTimer = 0f;

    public List<int> showAtLeast = new List<int>() { 0,1,2,3};

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
        if (alive)
        {
            

            if (launchingAttack) return;
            timer += Time.deltaTime;
            if (timer < cooldown) return;
            timer = 0;

            if (currentHealth > 2 * maxHealth / 3)
            {
                if(showAtLeast.Count > 0)
                {
                    int index = Random.Range(0, showAtLeast.Count);
                    int random = showAtLeast[index];
                    showAtLeast.RemoveAt(index);
                    switch (random)
                    {
                        case 0: StartCoroutine(SpawnPellets(2)); break;
                        case 1: StartCoroutine(SpawnLasers(2, 1.5f)); break;
                        case 2: StartCoroutine(Summon(2)); break;
                        case 3: StartCoroutine(OrderedAttack(2, true)); break;
                    }
                }
                else
                {
                    int random = Random.Range(0, 4);
                    switch (random)
                    {
                        case 0: StartCoroutine(SpawnPellets(Random.Range(1, 3))); break;
                        case 1: StartCoroutine(SpawnLasers(Random.Range(2, 4), 1.5f)); break;
                        case 2: StartCoroutine(Summon(Random.Range(1, 3))); break;
                        case 3: StartCoroutine(OrderedAttack(Random.Range(2, 4), true)); break;
                    }
                }
                
            }
            else if (currentHealth > maxHealth / 3) //two thirds
            {

                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0: StartCoroutine(SpawnPellets(Random.Range(2, 4))); break;
                    case 1: StartCoroutine(SpawnLasers(Random.Range(3, 4), 1.33f)); break;
                    case 2: StartCoroutine(Summon(Random.Range(2, 3))); break;
                    case 3: StartCoroutine(OrderedAttack(Random.Range(2, 5), true)); break;
                }
            }
            else if (currentHealth > 0) //final third
            {

                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0: StartCoroutine(PelletOnce()); break;
                    case 1: StartCoroutine(LaserOnce()); break;
                    case 2: StartCoroutine(SummonOnce()); break;
                    case 3: StartCoroutine(AttackOnce()); break;
                }
            }
        }
        
    }

    

    public IEnumerator OrderedAttack(int amount, bool stall)
    {
        if (!sequence)
        {
            sequence = true;

            launchingAttack = true;
            animator.SetBool("Warning", true);
            if (stall)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);

            }
            if (!alive) yield break;


            List<bool> orders = new List<bool>(); //false = spawns on the right, true = spawns on left
                                                  //false = arrow points to the right, true = points to the left
            for (int i = 0; i < amount; i++)
            {
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);

                int pos = Random.Range(0, 2);
                if (pos == 0)
                {
                    orders.Add(false);
                    animator.SetBool("Right", true);
                    //Debug.Log("Go Right");
                }
                else
                {
                    orders.Add(true);
                    animator.SetBool("Left", true);
                    //Debug.Log("Go Left");
                }
                animator.SetBool("Warning", false);
                yield return new WaitForSeconds(0.85f);
                if (!alive) yield break;
            }
            animator.SetBool("Warning", true);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);


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

                electricPrefab.GetComponent<AudioSource>().volume = DataManager.soundEffectVolume;
                Instantiate(electricPrefab, spawnPosition, Quaternion.identity);
                if (stall)
                {
                    yield return new WaitForSeconds(1.1f);
                }
                else
                {
                    yield return new WaitForSeconds(0.8f);
                }

                if (!alive) yield break;
            }

            launchingAttack = false;
            animator.SetBool("Warning", false);

            sequence = false;
        }
    }

    public IEnumerator SpawnLasers(int amount, float delay)
    {
        launchingAttack = true;
        animator.SetBool("Laser", true);
        for(int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(delay);
            if (!alive) yield break;
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

            laserPrefab.GetComponent<AudioSource>().volume = DataManager.soundEffectVolume;
            GameObject laser = Instantiate(laserPrefab, transform);
            
            
        }

        yield return new WaitForSeconds(1f);
        if (!alive) yield break;
        animator.SetBool("Laser", false);
        launchingAttack = false;
    }
    
    public IEnumerator SpawnPellets(int waves)
    {
        launchingAttack = true;
        animator.SetBool("Shooting", true);
        yield return new WaitForSeconds(1.2f);
        if (!alive) yield break;
        
        for(int z = 0; z < waves; z++)
        {
            audioSource.PlayOneShot(soundEffects[1]);
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
            if (!alive) yield break;
        }
        animator.SetBool("Shooting", false);
        launchingAttack = false;
    }
    public IEnumerator Summon(int amount)
    {
        launchingAttack = true;
        animator.SetBool("Summon", true);
        for (int i = 0; i < amount; i++)
        {
            if (!alive) yield break;
            yield return new WaitForSeconds(0.5f);
            if (!alive) yield break;
            audioSource.PlayOneShot(soundEffects[2]);
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
            cooldown = 2;
            CanvasReference.Instance.bossBarColor.color = Color.cyan;
        }
        else if(currentHealth > maxHealth / 3)
        {
            cooldown = 1.5f;
            CanvasReference.Instance.bossBarColor.color = Color.yellow;

        } else if(currentHealth > 0)
        {
            cooldown = 0.1f;
            CanvasReference.Instance.bossBarColor.color = Color.red;
        }

        if (currentHealth <= 0)
        {
            alive = false;
            if (maxHealth > 0)
            {
                maxHealth = 0;
                DropItems drops = GetComponent<DropItems>();
                if (drops != null)
                {
                    drops.SpawnDrops();
                }
                animator.SetBool("Dead", true);

                GetComponent<Rigidbody2D>().simulated = false;
                if (GetComponent<BoxCollider2D>()) { GetComponent<BoxCollider2D>().enabled = false; }

                foreach(HealthAttachment enemy in FindObjectsByType<HealthAttachment>(FindObjectsSortMode.None))
                {
                    if(enemy.gameObject.CompareTag("Enemy") || enemy.gameObject.CompareTag("Ranged"))
                    {
                        enemy.TakeDamage(100);
                    }
                    
                }
                StartCoroutine(CrashSprite());
            }

        }
        if (alive)
        {
            if (modifier != null)
            {
                modifier.GlitchSprite();
                if (currentHealth > 0)
                {
                    audioSource.PlayOneShot(soundEffects[0]);
                }
            }
        }
        

    }

    private IEnumerator LerpSlider()
    {
        barTimer = 0;
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

    public IEnumerator CrashSprite()
    {
        DataManager.time = GameManager.Instance.timer;

        for (int i = 1; i <= 30; i++)
        {
            audioSource.PlayOneShot(soundEffects[3]);
            GameObject emptyClone = Instantiate(crashableClone, transform);
            emptyClone.transform.position = transform.position + new Vector3(0.15f * i, -0.15f * i);
            emptyClone.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
            emptyClone.SetActive(true);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(6 + 30*0.03f);
        GameManager.Instance.ConcludeGame();
        Destroy(gameObject);
    }



    public IEnumerator AttackOnce()
    {
        if (!sequence)
        {
            sequence = true;
            launchingAttack = true;

            animator.SetBool("Left", false);
            animator.SetBool("Right", false);

            int pos = Random.Range(0, 2);
            if (pos == 0)
            {
                animator.SetBool("Right", true);
            }
            else
            {
                animator.SetBool("Left", true);
            }
            yield return new WaitForSeconds(0.85f);
            if (!alive) yield break;
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            yield return new WaitForSeconds(1f);
            if (!alive) yield break;
            Vector3 spawnPosition = Vector3.zero;
            if (pos == 1)
            {
                spawnPosition = electricPrefab.transform.position;
            }
            else
            {
                spawnPosition = new Vector3(6.5f, electricPrefab.transform.position.y);
            }
            spawnPosition += transform.position;

            electricPrefab.GetComponent<AudioSource>().volume = DataManager.soundEffectVolume;
            Instantiate(electricPrefab, spawnPosition, Quaternion.identity);

            launchingAttack = false;

            sequence = false;
        }
    }
    public IEnumerator LaserOnce()
    {
        launchingAttack = true;
        animator.SetBool("Laser", true);

        yield return new WaitForSeconds(0.5f);
        if (!alive) yield break;
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

        laserPrefab.GetComponent<AudioSource>().volume = DataManager.soundEffectVolume;
        GameObject laser = Instantiate(laserPrefab, transform);

        yield return new WaitForSeconds(0.1f);
        if (!alive) yield break;
        animator.SetBool("Laser", false);
        launchingAttack = false;
    }
    public IEnumerator PelletOnce()
    {
        launchingAttack = true;
        animator.SetBool("Shooting", true);
        yield return new WaitForSeconds(0.5f);
        if (!alive) yield break;
        

        audioSource.PlayOneShot(soundEffects[1]);
        float offset = pelletSpread / 2;
        float directionFacing = 185 + Random.Range(0, 10f);

        for (int i = 1; i <= pelletAmount; i++)
        {
            float section = (float)i / pelletAmount;
            float degree = section * pelletSpread - pelletSpread / 2 - offset + directionFacing;
            GameObject pelletClone = Instantiate(pelletPrefab, transform.position, Quaternion.Euler(0, 0, degree));
            pelletClone.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * 10, ForceMode2D.Impulse);
        }

        animator.SetBool("Shooting", false);
        launchingAttack = false;
    }

    public IEnumerator SummonOnce()
    {
        launchingAttack = true;
        animator.SetBool("Summon", true);

        if (!alive) yield break;
        yield return new WaitForSeconds(0.5f);
        if (!alive) yield break;
        audioSource.PlayOneShot(soundEffects[2]);
        int randomIndex = Random.Range(0, summonPrefabs.Count);

        Vector3 randomPos = Vector3.zero;
        //spawn them near the edge;
        int axis = Random.Range(0, 3);
        if (axis == 0) randomPos = new Vector3(Random.Range(-12f, 12f), -12f);
        else if (axis == 1) randomPos = new Vector3(-12f, Random.Range(-4f, -12f));
        else if (axis == 2) randomPos = new Vector3(12f, Random.Range(-4f, -12f));

        Instantiate(summonPrefabs[randomIndex], randomPos + transform.position, transform.rotation);

        animator.SetBool("Summon", false);
        launchingAttack = false;
    }
}
