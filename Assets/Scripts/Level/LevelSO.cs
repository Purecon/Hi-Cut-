using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Level/LevelSO")]
public class LevelSO : ScriptableObject
{
    public enum StageType
    {
        Tutorial,
        SliceQuiz
    }

    [Serializable]
    public struct Stage
    {
        public StageType levelType;
        public int groupNumber;
    }

    public List<Stage> StageList;
}
