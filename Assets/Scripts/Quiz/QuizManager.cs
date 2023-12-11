using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class QuizManager : Singleton<QuizManager>
{
    [Header("Quiz Assets")]
    public List<QuizGroup> groups;
    public int currentGroupIndex = 0;
    public int currentQuestionIndex = 0;
    public List<int> askedGroup;
    public List<int> askedQuestion;
    QuizGroup currentGroup;
    QuestionScriptable currentQuestion;
    public QuizTimer quizTimer;

    [Header("Display Assets")]
    public GameObject panels;
    GameObject currentPanels;
    List<SlicablePanelDisplay> displays;
    public TMP_Text qText;
    public GameObject qGroup;

    [Header("Enemy")]
    public Enemy enemy;
    //Health bar
    public GameObject enemyGameObject;
    public GameObject enemyHealthBar;
    public GameObject playerHealthBar;

    /*
    [Header("Player Assets")]
    public PlayerCharacter playerCharacter;
    */
    bool firstTime = true;

    private void OnEnable()
    {
        SlicableObject.OnSliced += SliceAnswer;
        QuizTimer.QuizTimeUp += TimeOut;
    }

    public void SliceAnswer(GameObject panel)
    {
        //Debug.LogFormat("Object sliced! {0}", panel.name);
        SlicablePanelDisplay display = panel.GetComponent<SlicablePanelDisplay>();
        SlicableObject sliceObj = panel.GetComponent<SlicableObject>();
        if (!sliceObj.isTutorial)
        {
            if (display.displayText.text == currentQuestion.answer)
            {
                Debug.Log("Answer correct 正解");

                sliceObj.Slice();

                foreach (SlicablePanelDisplay dp in displays)
                {
                    if (dp.name != panel.name)
                    {
                        SlicableObject slice = dp.GetComponent<SlicableObject>();
                        slice.isSlicable = false;
                    }
                }

                quizTimer.StopTime();
                StartCoroutine(NextQuestion(true));
            }
            else
            {
                Debug.Log("Answer wrong ブー"); //不正解

                quizTimer.PenaltyTime();
                sliceObj.WrongSlice();
            }
        }
    }

    public void TimeOut()
    {
        Debug.Log("Time up ");
        SoundManager.Instance.PlaySound("QuizWrong", 0.7f);
        StartCoroutine(NextQuestion(false));
    }

    public void StartQuiz(int groupIndex)
    {
        //Set questions
        currentGroupIndex = groupIndex;
        currentGroup = groups[currentGroupIndex];
        currentQuestionIndex = Random.Range(0, currentGroup.pool.Count);
        currentQuestion = currentGroup.pool[currentQuestionIndex];
        askedGroup.Add(currentGroupIndex);
        askedQuestion.Add(currentQuestionIndex);

        //Health
        PlayerCharacter.Instance.ResetCurrHealth();
        enemy.ResetCurrHealth();

        //Display
        firstTime = true;
        SetupPanel();
        DisplayQuiz();
    }

    public void SetupPanel()
    {
        currentPanels = Instantiate(panels);
        CanvasGroup canvasGroup = qGroup.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(1, 0.5f);
        qGroup.SetActive(false);
        currentPanels.SetActive(false);
        //Hide enemy health bar
        enemyHealthBar.SetActive(false);
        playerHealthBar.SetActive(false);
        enemyGameObject.SetActive(false);

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
        qGroup.SetActive(true);
        CanvasGroup canvasGroup = qGroup.GetComponent<CanvasGroup>();
        RectTransform rectTransform = qGroup.GetComponent<RectTransform>();
        canvasGroup.alpha = 0f;
        if (firstTime)
        {
            rectTransform.localPosition = new Vector2(0f, -1500f);
            rectTransform.DOAnchorPos(new Vector2(0f, -1707f), 0.5f, false).SetEase(Ease.OutElastic);
            firstTime = false;
        }
        canvasGroup.DOFade(1, 0.5f);
        currentPanels.SetActive(true);
        qText.text = currentQuestion.question;

        //QuizTimer
        quizTimer.StartTime();

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

            //Display Enemy
            enemyHealthBar.SetActive(true);
            playerHealthBar.SetActive(true);
            enemyGameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Display not enough for this pool");
        }
    }

    IEnumerator NextQuestion(bool answeredCorrectly)
    {
        if (answeredCorrectly)
        {
            PlayerCharacter.Instance.StartCharacterAttack();
            //Attack the enemy
            enemy.ChangeHealth(-2f);
        }
        else
        {
            PlayerCharacter.Instance.StartResetCursor();
        }
        
        yield return new WaitForSeconds(2f);
        
        if (enemy.CheckDeath())
        {
            CleanPanel();

            GameManager.Instance.NextStage();
        }
        else
        {
            if (askedQuestion.Count >= currentGroup.pool.Count)
            {
                askedQuestion = new List<int>();
            }

            //TODO: Enemy attack
            PlayerCharacter.Instance.ChangeHealth(-1f);

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
            CleanPanel();
            DisplayQuiz();
        }

        yield return null;
    }

    private void Start()
    {
        

        //StartQuiz();
    }
}
