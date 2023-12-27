using System;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurLevelResourcesManager : MonoBehaviour
{
    public List<LevelResources> _levelResources;

    public int GetMaximumMoneyByLevel(int level)
    {
        return 0; // _levelResources[level]._maximumMoneyForTime;
    }

    [Serializable]
    public class LevelResources
    {
        public int _maximumMoneyForTime;
    }
}
