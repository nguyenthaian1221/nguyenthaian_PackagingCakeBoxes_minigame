using LevelUnlockSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelBtnScript : MonoBehaviour
{

    private int levelIndex;
    [SerializeField] private Button btn;
    //private GameObject activeLevelIndicator;
    [SerializeField] private GameObject[] starsArray;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject lockIcon;

    private void Start()
    {
        btn.onClick.AddListener(() => Onclick());
    }

    public void SetLevelButton(LevelItem value, int index)
    {
        levelIndex = index + 1;
        levelText.text = "" + levelIndex;
        if (value.unlocked)
        {
            levelText.gameObject.SetActive(true);
            lockIcon.SetActive(false);
            btn.interactable = true;
            //activeLevelIndicator.SetActive(activeLevel);
            SetStar(value.starAchieved);
        }
        else
        {
            levelText.gameObject.SetActive(false);
            lockIcon.SetActive(true);
            btn.interactable = false;
        }

    }

    private void SetStar(int starAchieved)
    {
        Debug.Log("Da vao set star " + starAchieved);
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

    void Onclick()
    {
        LevelSystemManager.Instance.CurrentLevel = levelIndex - 1;
        // LoadLevel
        Debug.Log("Level" + levelIndex);

        GameManager.instance.StartAGame(levelIndex);
    }


}
