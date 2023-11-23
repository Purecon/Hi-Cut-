using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Singleton<PlayerCharacter>
{
    [Header("Player Assets")]
    public TestTouch touchInput;
    public GameObject playerCursor;
    public Animator playerCharAnimator;
    Vector2 cursorOrigin;

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




    //EnableMove

    private void Start()
    {
        cursorOrigin = playerCursor.transform.position;
    }
}
