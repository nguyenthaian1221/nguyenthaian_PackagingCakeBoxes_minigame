using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;


namespace LevelUnlockSystem
{
    public class LevelUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelBtnGridHolder;
        [SerializeField] private LevelBtnScript levelBtnPrefab;
        private List<GameObject> arrBtn;
        public static LevelUIManager Instance;

        private void Awake()
        {
            Instance ??= this;
        }




        private void Start()
        {
            arrBtn = new List<GameObject>();
            SaveLoadData.Instance.Initialized();
            InitializedUI();
        }

        public void InitializedUI()
        {
            LevelItem[] levelItemArr = LevelSystemManager.Instance.LevelData.levelItemArray;

            for (int i = 0; i < levelItemArr.Length; i++)
            {
                LevelBtnScript levelButton = Instantiate(levelBtnPrefab, levelBtnGridHolder.transform);
                arrBtn.Add(levelButton.gameObject);
                levelButton.SetLevelButton(levelItemArr[i], i);
            }

        }


        public void ResetList()
        {
            Debug.Log("Da xoa het buttn1");
            foreach (var item1 in arrBtn)
            {
                Destroy(item1.gameObject);
            }

            arrBtn.Clear();
            SaveLoadData.Instance.LoadData();
            InitializedUI();
        }
    }

}



