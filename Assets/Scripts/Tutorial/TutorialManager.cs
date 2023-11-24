using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialManager : Singleton<TutorialManager>
{
    [Header("Quiz Assets")]
    public List<QuizGroup> groups;
    public int currentGroupIndex = 0;
    public int currentQuestionIndex = 0;
    QuizGroup currentGroup;
    QuestionScriptable currentQuestion;

    [Header("Display Assets")]
    public GameObject panels;
    GameObject currentPanels;
    SlicablePanelDisplay display;
    public GameObject tGroup;
    bool firstTime = true;

    private void OnEnable()
    {
        SlicableObject.OnSliced += SliceAnswer;
    }

    public void StartTutorial(int groupIndex)
    {
        currentGroupIndex = groupIndex;
        currentGroup = groups[currentGroupIndex];
        currentQuestionIndex = 0;
        currentQuestion = currentGroup.pool[currentQuestionIndex];
        firstTime = true;
        SetupPanel();
        DisplayTutorial();
    }

    public void SetupPanel()
    {
        currentPanels = Instantiate(panels);
        CanvasGroup canvasGroup = tGroup.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(1, 0.5f);
        tGroup.SetActive(false);
        currentPanels.SetActive(false);

        display = currentPanels.GetComponentInChildren<SlicablePanelDisplay>();
    }

    public void CleanPanel()
    {
        Destroy(currentPanels);

        SetupPanel();
    }

    public void DisplayTutorial()
    {
        tGroup.SetActive(true);
        CanvasGroup canvasGroup = tGroup.GetComponent<CanvasGroup>();
        RectTransform rectTransform = tGroup.GetComponent<RectTransform>();
        canvasGroup.alpha = 0f;
        if (firstTime)
        {
            rectTransform.localPosition = new Vector2(0f, -1500f);
            rectTransform.DOAnchorPos(new Vector2(0f, -1707f), 0.5f, false).SetEase(Ease.OutElastic);
            firstTime = false;
        }
        canvasGroup.DOFade(1, 0.5f);
        currentPanels.SetActive(true);

        display.displayText.text = currentQuestion.question;
        display.explanationDisplayText.text = currentQuestion.answer;
    }

    public void SliceAnswer(GameObject panel)
    {
        SlicablePanelDisplay display = panel.GetComponent<SlicablePanelDisplay>();
        SlicableObject sliceObj = panel.GetComponent<SlicableObject>();
        if (sliceObj.isTutorial)
        {
            Debug.Log("Tutorial");
            sliceObj.Slice();

            StartCoroutine(NextTutorial());
        }
    }

    IEnumerator NextTutorial()
    {
        PlayerCharacter.Instance.StartCharacterAttack();
        yield return new WaitForSeconds(2f);


        if (currentQuestionIndex == currentGroup.pool.Count-1)
        {
            CleanPanel();

            GameManager.Instance.NextStage();
            yield return null;
        }
        else
        {
            currentQuestionIndex++;
            currentQuestion = currentGroup.pool[currentQuestionIndex];

            CleanPanel();
            DisplayTutorial();
            yield return null;
        }
    }
}
