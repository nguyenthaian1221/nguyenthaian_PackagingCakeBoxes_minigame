using LevelUnlockSystem;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtnScript : MonoBehaviour
{

    private int levelIndex;
    private Button btn;
    private GameObject activeLevelIndicator;
    private Image[] starsArray;



    private void Start()
    {
        btn.onClick.AddListener(() => Onclick());    
    }

    public void SetLevelButton(LevelItem value, int index, bool activeLevel)
    {
        if (value.unlocked)
        {
            levelIndex = index + 1;
            btn.interactable = true;
            activeLevelIndicator.SetActive(activeLevel);
            SetStar(value.starAchieved);
        }
        else
        {
            btn.interactable = false;
        }

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

    void Onclick()
    {
        LevelSystemManager.Instance.CurrentLevel = levelIndex - 1;
        // LoadLevel

    }


}
