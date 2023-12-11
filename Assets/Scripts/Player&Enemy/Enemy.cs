using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Health")]
    public float currEnemyHealth = 10f;
    public float maxEnemyHealth = 10f;
    public Slider healthBar;

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
        currEnemyHealth += change;
        Mathf.Clamp(currEnemyHealth, 0, maxEnemyHealth);
        healthBar.value = (currEnemyHealth / maxEnemyHealth);
    }

    private void Start()
    {
        ResetCurrHealth();
    }
}
