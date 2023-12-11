using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : Singleton<PlayerCharacter>
{
    [Header("Player Assets")]
    public TestTouch touchInput;
    public GameObject playerCursor;
    public Animator playerCharAnimator;
    Vector2 cursorOrigin;

    [Header("Player Health")]
    public float currPlayerHealth = 10f;
    public float maxPlayerHealth = 10f;
    public Slider healthBar;

    public void StartCharacterAttack()
    {
        StartCoroutine(CharacterAttack());
    }

    IEnumerator CharacterAttack()
    {
        //Reset cursor
        playerCharAnimator.SetBool("IsAttacking", true);
        touchInput.StopSwipe(cursorOrigin, 0f);
        touchInput.enableMove = false;
        playerCursor.transform.position = cursorOrigin;
        playerCursor.SetActive(false);

        yield return new WaitForSeconds(2f);
        playerCharAnimator.SetBool("IsAttacking", false);
        touchInput.enableMove = true;
        playerCursor.SetActive(true);
    }

    public void StartResetCursor()
    {
        StartCoroutine(ResetCursor());
    }

    IEnumerator ResetCursor()
    {
        //Reset cursor
        touchInput.StopSwipe(cursorOrigin, 0f);
        touchInput.enableMove = false;
        playerCursor.transform.position = cursorOrigin;
        playerCursor.SetActive(false);

        yield return new WaitForSeconds(2f);
        touchInput.enableMove = true;
        playerCursor.SetActive(true);
    }

    public void ResetCurrHealth()
    {
        currPlayerHealth = maxPlayerHealth;
        healthBar.value = (currPlayerHealth / maxPlayerHealth);
    }

    public bool CheckDeath()
    {
        return currPlayerHealth <= 0;
    }

    public void ChangeHealth(float change)
    {
        if (change < 0)
        {
            playerCharAnimator.Play("Damage");
        }

        currPlayerHealth += change;
        Mathf.Clamp(currPlayerHealth, 0, maxPlayerHealth);
        healthBar.value = (currPlayerHealth / maxPlayerHealth);
    }

    //EnableMove
    private void Start()
    {
        cursorOrigin = playerCursor.transform.position;
        ResetCurrHealth();
    }
}
