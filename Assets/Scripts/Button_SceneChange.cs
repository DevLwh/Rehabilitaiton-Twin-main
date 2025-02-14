using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_SceneChange : MonoBehaviour
{
    // SCENE 이동 용 버튼 함수
    public void SceneChanger(string scene_name)
    {
        switch (scene_name)
        {
            case "home":
                SceneManager.LoadScene("Home1");
                break;
            case "hardwareSelect":
                SceneManager.LoadScene("HardwareSelect1");
                break;
            case "wearYourHMD":
                SceneManager.LoadScene("WearYourHMD1");
                break;
            case "themaSelect":
                SceneManager.LoadScene("ThemaSelect1");
                break;
            case "exerciseSelect":
                SceneManager.LoadScene("ExerciseSelect1");
                break;
            case "exercise":
                if (GameManager.instance.ForNumber == false)
                {
                    SceneManager.LoadScene("Exercise_Content1");
                }
                else if (GameManager.instance.ForNumber == true)
                {
                    SceneManager.LoadScene("Exercise_Content2_1");
                }
                break;
            case "exercise_external":
                SceneManager.LoadScene("Exercise_ExternalRotation");
                break;
            case "result":
                if (GameManager.instance.ForNumber == false)
                {
                    SceneManager.LoadScene("Result1");
                }
                else if (GameManager.instance.ForNumber == true)
                {
                    SceneManager.LoadScene("Result2_1");
                }
                break;
        }

    }

}
