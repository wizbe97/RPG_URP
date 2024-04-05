using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using TMPro;

public class RangedEnemyCharacter : Character
{
    public int damageStrength;
    public GameObject floatingHealthBar;
    public GameObject shadow;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    [SerializeField] private FloatingHealthBar healthBar;
    public GameObject floatingDamage;

    float hitPoints;

    private void OnEnable()
    {
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.UpdateHealthBar(hitPoints, maxHitPoints);

    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints -= damage;
            healthBar.UpdateHealthBar(hitPoints, maxHitPoints);
            GameObject damageNumber = Instantiate(floatingDamage, transform.position, Quaternion.identity) as GameObject;
            TextMeshPro damageText = damageNumber.transform.GetChild(0).GetComponent<TextMeshPro>();
            damageText.text = damage.ToString();
            if (hitPoints <= 0)
            {
                KillCharacter();
                break;
            }

            if (interval > 0)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    // Kill Character (Enemy)
    public override void KillCharacter()
    {
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        floatingHealthBar.SetActive(false);
        shadow.SetActive(false);
        animator.Play("Death");
        capsuleCollider2D.enabled = false;

        RangedEnemyController rangedEnemyController = GetComponent<RangedEnemyController>();
        if (rangedEnemyController != null)
        {
            rangedEnemyController.DisableMovement(); // Call a method to disable movement
        }

    }

    public void OnDeathEnd()
    {
        base.KillCharacter();
    }

}
