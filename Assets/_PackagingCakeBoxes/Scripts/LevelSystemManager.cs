using LevelUnlockSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace LevelUnlockSystem
{
    [System.Serializable]
    public class LevelItem
    {
        public bool unlocked;
        public int starAchieved;
    }

    [System.Serializable]
    public class LevelData
    {
        public int lastUnlockedLevel = 0;
        public LevelItem[] levelItemArray;
    }

    public class LevelSystemManager : MonoBehaviour
    {

        private static LevelSystemManager instance;

        [SerializeField]
        private LevelData levelData;

        private int currentLevel;

        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }

        public static LevelSystemManager Instance { get => instance; }
        public LevelData LevelData { get => levelData; }




        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SaveLoadData.Instance.Initialized();
        }


        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        SaveLoadData.Instance.SaveData();
        //    }
        //}

        public void LevelComplete(int starAchieved)                             //method called when player win the level
        {
            levelData.levelItemArray[currentLevel].starAchieved = starAchieved;    //save the stars achieved by the player in level
            if (levelData.lastUnlockedLevel < (currentLevel + 1))
            {
                levelData.lastUnlockedLevel = currentLevel + 1;           //change the lastUnlockedLevel to next level
                                                                          //and make next level unlock true
                levelData.levelItemArray[levelData.lastUnlockedLevel].unlocked = true;
                //SaveLoadData.Instance.SaveData();
            }
        }

    }

}


