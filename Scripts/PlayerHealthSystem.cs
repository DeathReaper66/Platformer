using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private Rigidbody2D body;
    private bool isDead;

    [Header("HealthBarImage")]
    [SerializeField] private Image currenthealthBar;

    [Header("iFrames Parameters")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private float numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("DeathScreen")]
    [SerializeField] GameObject deathScreen;


    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        deathScreen.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        currenthealthBar.fillAmount -= damage / 10;

        if (currentHealth > 0)
        {
            StartCoroutine(Invunerability());
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!isDead)
            {
                Death();

                GetComponent<PlayersMovement>().enabled = false;
                body.velocity = new Vector2(0, 0);
                body.gravityScale = 0;
                GetComponent<PlayersAttack>().enabled = false;

                anim.StopPlayback();
                anim.SetTrigger("die");

                isDead = true;
            }
        }
    }
    public void AddHealth(float healingValue)
    {
        if (currenthealthBar.fillAmount < startingHealth / 10)
        {
            currentHealth = Mathf.Clamp(currentHealth + healingValue, 0, startingHealth);
            currenthealthBar.fillAmount += healingValue / 10;
            StartCoroutine(Healing());
        }
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
    private IEnumerator Healing()
    {
        spriteRend.color = Color.green;
        yield return new WaitForSeconds(0.3f);
        spriteRend.color = Color.white;
    }

    private void Death()
    {
        if (!deathScreen.activeSelf)
            deathScreen.SetActive(true);
    }
}
