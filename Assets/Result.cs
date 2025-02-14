using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text[] PGBS = new Text[4];

    // Start is called before the first frame update
    void Start()
    {
/*        if (GameManager.instance.nowExercise == GameManager.ExerciseList.Squat)
        {
            for (int i = 0; i < 4; i++)
            {
                PGBS[i].text = GameManager.instance.Squat_PGBS[i].ToString();
            }
            PGBS[3].text += "%";
        }

        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.ExternalRotation)
        {
            for (int i = 0; i < 4; i++)
                {
                    PGBS[i].text = GameManager.instance.External_PGBS[i].ToString();
                }
             PGBS[3].text += "%";
        }
        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.StickMobility)
        {
            for (int i = 0; i < 4; i++)
            {
                PGBS[i].text = GameManager.instance.Stick_PGBS[i].ToString();
            }
            PGBS[3].text += "%";

        }
        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.SideShoulderDeltoid)
        {
            for (int i = 0; i < 4; i++)
            {
                PGBS[i].text = GameManager.instance.SideShoulderDeltoid_PGBS[i].ToString();
            }
            PGBS[3].text += "%";

        }
        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.BackShoulderDeltoid)
        {
            for (int i = 0; i < 4; i++)
            {
                PGBS[i].text = GameManager.instance.BackShoulderDeltoid_PGBS[i].ToString();
            }
            PGBS[3].text += "%";

        }
        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.BottomShoulder)
        {
            for (int i = 0; i < 4; i++)
            {
                PGBS[i].text = GameManager.instance.BottomShoulder_PGBS[i].ToString();
            }
            PGBS[3].text += "%";

        }
        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.ShoulderRotation)
        {
            for (int i = 0; i < 4; i++)
            {
                PGBS[i].text = GameManager.instance.ShoulderRotation_PGBS[i].ToString();
            }
            PGBS[3].text += "%";

        }
        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.FrontShoulder)
        {
            for (int i = 0; i < 4; i++)
            {
                PGBS[i].text = GameManager.instance.FrontShoulder_PGBS[i].ToString();
            }
            PGBS[3].text += "%";

        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
