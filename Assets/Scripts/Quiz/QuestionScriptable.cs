using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz/Question")]
public class QuestionScriptable : ScriptableObject
{
    public string ID;
    public QuizGroup group;
    public string question;
    public string answer;
}
