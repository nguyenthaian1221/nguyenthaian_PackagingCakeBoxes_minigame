using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace LevelUnlockSystem
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] starsArray;
        //[SerializeField] private Sprite[] spritesArray;
        //[SerializeField] private Text levelStatusText;
        [SerializeField] private GameObject overWinPanel;
        [SerializeField] private GameObject overLosePanel;



        public static GameUI Instance;

        private void Awake()
        {
            Instance ??= this;
        }


        public void GameOver(int starCount)
        {
            if (starCount > 0)   // Win condition
            {
                //levelStatusText.text = "Level " + (LevelSystemManager.Instance.CurrentLevel + 1) + " Completed";
                LevelSystemManager.Instance.LevelComplete(starCount);
            }
            // else     // Lose condition
            //{
            //levelStatusText.text = "Level " + (LevelSystemManager.Instance.CurrentLevel + 1) + " Failed";

            //}

            SetStar(starCount);
        }



        private void SetStar(int starAchieved)
        {

            switch (starAchieved)
            {
                case 0:
                    starsArray[0].SetActive(false);
                    starsArray[1].SetActive(false);
                    starsArray[2].SetActive(false);
                    break;
                case 1:
                    starsArray[0].SetActive(true);
                    starsArray[1].SetActive(false);
                    starsArray[2].SetActive(false);
                    break;
                case 2:
                    starsArray[0].SetActive(true);
                    starsArray[1].SetActive(true);
                    starsArray[2].SetActive(false);
                    break;
                case 3:
                    starsArray[0].SetActive(true);
                    starsArray[1].SetActive(true);
                    starsArray[2].SetActive(true);
                    break;
            }
        }


    }




}



