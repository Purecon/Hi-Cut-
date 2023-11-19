using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz/Quizgroup")]
public class QuizGroup : ScriptableObject
{
    public string ID;
    public string group;
    public List<QuestionScriptable> pool;
}
