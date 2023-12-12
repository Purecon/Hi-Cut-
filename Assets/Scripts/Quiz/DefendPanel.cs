using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefendPanel : MonoBehaviour
{
    public List<TMP_Text> buttonTexts;
    public int correctIndex;

    public void DefendButton(int number)
    {
        if(number == correctIndex)
        {
            QuizManager.Instance.Defend(true); 
        }
        else
        {
            QuizManager.Instance.Defend(false);
        }

        Destroy(gameObject);
    }
}
