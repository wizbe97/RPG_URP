using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyCharacter : Character
{
    public int damageStrength;
    public GameObject floatingHealthBar;
    public GameObject shadow;
    private EnemyController enemyController;
    public CapsuleCollider2D capsuleCollider2D;
    [SerializeField] private FloatingHealthBar healthBar;
    public GameObject floatingDamage;

    public float hitPoints;

    private void OnEnable()
    {
        enemyController = GetComponent<EnemyController>();
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
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
        capsuleCollider2D.enabled = false;
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        floatingHealthBar.SetActive(false);
        shadow.SetActive(false);
        enemyController.CurrentState = EnemyController.EnemyStates.DIE;
        enemyController.canMove = false;
    }

    public void OnDeathEnd()
    {
        base.KillCharacter();
    }

}
