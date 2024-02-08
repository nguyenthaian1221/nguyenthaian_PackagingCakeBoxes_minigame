using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LevelUnlockSystem
{
    public class LevelUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelBtnGridHolder;
        [SerializeField] private LevelBtnScript levelBtnPrefab;




        private void Start()
        {
            InitializedUI();
        }

        public void InitializedUI()
        {
            LevelItem[] levelItemArr = LevelSystemManager.Instance.LevelData.levelItemArray;

            for (int i = 0; i < levelItemArr.Length; i++)
            {
                LevelBtnScript levelButton = Instantiate(levelBtnPrefab, levelBtnGridHolder.transform);
                levelButton.SetLevelButton(levelItemArr[i],i);
            }

        }

    }

}



