using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public QuizManager quizManager;
    public TutorialManager tutorialManager;
    public LevelSO level;
    public int stageIndex = 0;

    private void Start()
    {
        if(quizManager == null)
        {
            quizManager = QuizManager.Instance;
        }
        if (tutorialManager == null)
        {
            tutorialManager = TutorialManager.Instance;
        }

        NextStage();
        //tutorialManager.StartTutorial(0);
        //quizManager.StartQuiz(0);
    }

    public void NextStage()
    {
        if(stageIndex >= level.StageList.Count)
        {
            Debug.Log("Finish");

            SoundManager.Instance.PlaySound("StageFinished", 0.75f);
        }
        else
        {
            LevelSO.Stage stage = level.StageList[stageIndex];
            if (stage.levelType == LevelSO.StageType.Tutorial)
            {
                tutorialManager.StartTutorial(stage.groupNumber);
            }
            else if (stage.levelType == LevelSO.StageType.SliceQuiz)
            {
                quizManager.StartQuiz(stage.groupNumber);
            }
            stageIndex++;
        }
    }
}
