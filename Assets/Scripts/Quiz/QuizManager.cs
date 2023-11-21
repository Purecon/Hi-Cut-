using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Assets")]
    public List<QuizGroup> groups;

    public int currentGroupIndex = 0;
    public int currentQuestionIndex = 0;
    public List<int> askedGroup;
    public List<int> askedQuestion;
    public QuizGroup currentGroup;
    public QuestionScriptable currentQuestion;

    [Header("Display Assets")]
    public GameObject panels;
    public GameObject currentPanels;
    public List<SlicablePanelDisplay> displays;
    public TMP_Text qText;

    [Header("Player Assets")]
    public TestTouch touchInput;
    public GameObject playerCursor;
    public Animator playerCharAnimator;
    Vector2 cursorOrigin;

    bool nextGroup = true;

    private void OnEnable()
    {
        SlicableObject.OnSliced += SliceAnswer;
    }

    public void SliceAnswer(GameObject panel)
    {
        Debug.LogFormat("Object sliced! {0}", panel.name);
        SlicablePanelDisplay display = panel.GetComponent<SlicablePanelDisplay>();
        SlicableObject sliceObj = panel.GetComponent<SlicableObject>();

        if (display.displayText.text == currentQuestion.answer)
        {
            Debug.Log("Answer correct 正解");

            sliceObj.Slice();

            foreach (SlicablePanelDisplay dp in displays)
            {
                if(dp.name != panel.name)
                {
                    SlicableObject slice = dp.GetComponent<SlicableObject>();
                    slice.isSlicable = false;
                }
            }

            StartCoroutine(NextQuestion());
        }
        else
        {
            Debug.Log("Answer wrong ブー"); //不正解

            //ShakeBehavior.InvokeShakeEvent(1f);
            sliceObj.WrongSlice();
        }
    }

    public void StartQuiz()
    {
        SetupPanel();

        //currentGroupIndex = Random.Range(0, groups.Count);
        currentGroupIndex = 0;
        currentGroup = groups[currentGroupIndex];
        currentQuestionIndex = Random.Range(0, currentGroup.pool.Count);
        currentQuestion = currentGroup.pool[currentQuestionIndex];
        askedGroup.Add(currentGroupIndex);
        askedQuestion.Add(currentQuestionIndex);
        DisplayQuiz();
    }

    public void SetupPanel()
    {
        currentPanels = Instantiate(panels);

        displays = new List<SlicablePanelDisplay>(currentPanels.GetComponentsInChildren<SlicablePanelDisplay>());
    }

    public void CleanPanel()
    {
        Destroy(currentPanels);

        SetupPanel();
    }

    public void DisplayQuiz()
    {
        Debug.LogFormat("[Group {0}]", currentGroup);
        //Debug.LogFormat("[Question {0}]", currentQuestion.ID);
        //Debug.LogFormat("What is {0}? Answer: {1}", currentQuestion.question,currentQuestion.answer);
        qText.text = currentQuestion.question;

        List<string> answers = new List<string>();
        //Need 4 answer
        string correctAnswer = currentQuestion.answer;
        answers.Add(correctAnswer); //Correct one
        if (currentGroup.pool.Count >= displays.Count)
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

    IEnumerator NextQuestion()
    {
        //Reset cursor
        playerCharAnimator.SetBool("IsAttacking", true);
        touchInput.StopSwipe(cursorOrigin, 0f);
        touchInput.enableMove = false;
        playerCursor.transform.position = cursorOrigin;
        playerCursor.SetActive(false);

        yield return new WaitForSeconds(2f);
        playerCharAnimator.SetBool("IsAttacking", false);
        playerCursor.SetActive(true);

        if (askedQuestion.Count == currentGroup.pool.Count)
        {
            //NOTE: Currently used this instead of different level (subject to change)
            if (nextGroup)
            {
                if (askedGroup.Count != groups.Count)
                {
                    currentGroupIndex++;
                    currentGroup = groups[currentGroupIndex];
                    askedGroup.Add(currentGroupIndex);
                    Debug.LogFormat("[Group {0}]", currentGroup);

                    askedQuestion = new List<int>();
                    currentQuestionIndex = Random.Range(0, currentGroup.pool.Count);
                    currentQuestion = currentGroup.pool[currentQuestionIndex];
                    askedQuestion.Add(currentQuestionIndex);

                    touchInput.enableMove = true;
                    CleanPanel();
                    DisplayQuiz();
                    yield return null;
                }
            }
            yield return null;
        }
        else
        {
            while (askedQuestion.Contains(currentQuestionIndex))
            {
                int tempCurrentQuestionIndex = Random.Range(0, currentGroup.pool.Count);
                if (!askedQuestion.Contains(tempCurrentQuestionIndex))
                {
                    currentQuestionIndex = tempCurrentQuestionIndex;
                    currentQuestion = currentGroup.pool[currentQuestionIndex];
                }
            }
            askedQuestion.Add(currentQuestionIndex);

            touchInput.enableMove = true;
            CleanPanel();
            DisplayQuiz();
            yield return null;
        }
    }

    private void Start()
    {
        cursorOrigin = playerCursor.transform.position;

        StartQuiz();
    }
}
