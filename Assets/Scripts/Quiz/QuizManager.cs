using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Assets")]
    public List<QuizGroup> groups;

    public int currentGroupIndex = 0;
    public int currentQuestionIndex = 0;
    public QuizGroup currentGroup;
    public QuestionScriptable currentQuestion;

    [Header("Display Assets")]
    public List<SlicablePanelDisplay> displays;

    public void StartQuiz()
    {
        currentGroup = groups[currentGroupIndex];
        currentQuestion = currentGroup.pool[currentQuestionIndex];
        DisplayQuiz();
    }

    public void DisplayQuiz()
    {
        Debug.LogFormat("[Group {0}]", currentGroup);
        Debug.LogFormat("[Question {0}]", currentQuestion.ID);
        Debug.LogFormat("What is {0}? Answer: {1}", currentQuestion.question,currentQuestion.answer);
        List<string> answers = new List<string>();
        //Need 4 answer
        answers.Add(currentQuestion.answer); //Correct one
        
        if(currentGroup.pool.Count >= displays.Count)
        {
            while (answers.Count < displays.Count)
            {
                int a = Random.Range(0, currentGroup.pool.Count);
                string tempAnswer = currentGroup.pool[a].answer;
                if (!answers.Contains(tempAnswer))
                {
                    answers.Add(tempAnswer);
                }
            }
            //Debug.LogFormat("The answers: {0}",answers.ToString());
            foreach (SlicablePanelDisplay display in displays)
            {
                int b = Random.Range(0, answers.Count);
                display.displayText.text = answers[b];
                answers.RemoveAt(b);
            }
        }
        else
        {
            Debug.LogWarning("Display not enough for this pool");
        }
    }

    private void Start()
    {
        StartQuiz();
    }
}
