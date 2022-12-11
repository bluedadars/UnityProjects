using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    // Start is called before the first frame update
    public float currentHealth { get; private set; }

    [Header("Health")]
    [SerializeField] private float startingHealth;
    private Animator anim;
    private bool Dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOffFlashes;
    private SpriteRenderer spriteRend;
    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    private bool invulnerable;
    void Awake()
    {
        currentHealth = startingHealth;

        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {

        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //Player Hurt
            anim.SetTrigger("isHurt");
            StartCoroutine(Invulnerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            //Player Dead
            if (!Dead)
            {
                anim.SetTrigger("Die");

                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                Dead = true;
                Destroy(gameObject, 1f);
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    public void AddHealthValue(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        invulnerable = false;

    }

}
