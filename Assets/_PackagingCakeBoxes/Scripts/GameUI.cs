using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace LevelUnlockSystem
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Image[] starsArray;
        [SerializeField] private Sprite[] spritesArray;
        [SerializeField] private Text levelStatusText;
        [SerializeField] private GameObject overPanel;


        public void GameOver(int starCount)
        {
            if (starCount > 0)
            {
                levelStatusText.text = "Level " + (LevelSystemManager.Instance.CurrentLevel + 1) + " Completed";
                LevelSystemManager.Instance.LevelComplete(starCount);
            }
            else
            {
                levelStatusText.text = "Level " + (LevelSystemManager.Instance.CurrentLevel + 1) + " Failed";

            }

            SetStar(starCount);
        }



        private void SetStar(int starAchieved)
        {
            for (int i = 0; i < starsArray.Length; i++)
            {
                if (i < starAchieved)
                {

                }
                else
                {

                }
            }
        }

        public void OkBtn()
        {
            // LoadMap
        }



    }




}



