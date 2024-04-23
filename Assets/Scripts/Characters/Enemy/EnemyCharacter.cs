using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyCharacter : Character
{
    public float attackCooldown = 3f;
    public int damage = 15;
    public GameObject floatingHealthBar;
    public GameObject shadow;
    private EnemyController enemyController;
    public CapsuleCollider2D collisionsCapsule;
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
        // Check if the enemy is already dead
        if (hitPoints <= 0)
        {
            yield break; // Exit the coroutine if the enemy is already dead
        }

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


    public override void KillCharacter()
    {
        collisionsCapsule.enabled = false;
        floatingHealthBar.SetActive(false);
        shadow.SetActive(false);
        enemyController.CurrentState = EnemyController.EnemyStates.DIE;
        enemyController.canMove = false;
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        base.KillCharacter();
    }

    public void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnedEnemies.Remove(gameObject);
        }

    }

}
