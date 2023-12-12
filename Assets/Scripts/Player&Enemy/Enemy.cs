using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Health")]
    public float currEnemyHealth = 10f;
    public float maxEnemyHealth = 10f;
    public Slider healthBar;
    [Header("Enemy Attack")]
    public float attackDamage = 1f;
    [Header("Enemy GO")]
    public GameObject enemyGO;

    public void ResetCurrHealth()
    {
        currEnemyHealth = maxEnemyHealth;
        healthBar.value = (currEnemyHealth / maxEnemyHealth);
    }

    public bool CheckDeath()
    {
        return currEnemyHealth <= 0;
    }
    public void ChangeHealth(float change)
    {
        if (change < 0)
        {
            //Tweening
            Vector3 originalScale = enemyGO.transform.localScale;
            enemyGO.transform.DOShakeRotation(0.75f, new Vector3(0, 0, 30f), 5);
            float scaleSpeed = 0.5f;
            var sequence = DOTween.Sequence()
                .Append(enemyGO.GetComponent<SpriteRenderer>().DOColor(Color.red, scaleSpeed/2))
                .Append(enemyGO.transform.DOScale(new Vector3(originalScale.x + 0.5f, originalScale.y + 0.5f, originalScale.z + 0.5f), scaleSpeed))
                .Append(enemyGO.transform.DOScale(originalScale, scaleSpeed))
                .Append(enemyGO.GetComponent<SpriteRenderer>().DOColor(Color.white, scaleSpeed/2));
        }

        currEnemyHealth += change;
        Mathf.Clamp(currEnemyHealth, 0, maxEnemyHealth);
        healthBar.value = (currEnemyHealth / maxEnemyHealth);
    }

    public void EnemyAttack()
    {
        //Tweening attack anim
        Vector3 originalScale = enemyGO.transform.localScale;
        float scaleSpeed = 0.5f;
        var sequence = DOTween.Sequence()
            .Append(enemyGO.transform.DOScale(new Vector3(originalScale.x + 1f, originalScale.y + 1f, originalScale.z + 1f), scaleSpeed))
            .Append(enemyGO.transform.DOScale(originalScale, scaleSpeed));
        PlayerCharacter.Instance.ChangeHealth(-attackDamage);
    }

    private void Start()
    {
        ResetCurrHealth();
    }
}
