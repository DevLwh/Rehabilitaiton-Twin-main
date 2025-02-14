using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill_Result : MonoBehaviour
{
    public GameObject AccuracyBar;
    public GameObject perfectCircle;
    public GameObject goodCircle;
    public GameObject badCircle;

    public GameObject AccuracyBar2;
    public GameObject perfectCircle2;
    public GameObject goodCircle2;
    public GameObject badCircle2;


    private float maxFill;
    private float maxFill2;

    private void Start()
    {
        float[] state = new float[4];
        float[] state2 = new float[4];


        // GameManager의 인스턴스에서 운동 state정보를 받아옴

        {
            switch (GameManager.instance.nowExercise)
            {
                case GameManager.ExerciseList.Squat:
                    state = GameManager.instance.Squat_PGBS;
                    break;

                case GameManager.ExerciseList.ExternalRotation:
                    state = GameManager.instance.External_PGBS;
                    break;

                case GameManager.ExerciseList.StickMobility:
                    state = GameManager.instance.Stick_PGBS;
                    break;

                case GameManager.ExerciseList.SideShoulderDeltoid:
                    state = GameManager.instance.SideShoulderDeltoid_PGBS;
                    if (GameManager.instance.ForNumber)
                        state2 = GameManager.instance.SideShoulderDeltoid2_PGBS;
                    break;

                case GameManager.ExerciseList.BackShoulderDeltoid:
                    state = GameManager.instance.BackShoulderDeltoid_PGBS;
                    if (GameManager.instance.ForNumber)
                        state2 = GameManager.instance.BackShoulderDeltoid2_PGBS;
                    break;

                case GameManager.ExerciseList.BottomShoulder:
                    state = GameManager.instance.BottomShoulder_PGBS;
                    if (GameManager.instance.ForNumber)
                        state2 = GameManager.instance.BottomShoulder2_PGBS;
                    break;

                case GameManager.ExerciseList.ShoulderRotation:
                    state = GameManager.instance.ShoulderRotation_PGBS;
                    if (GameManager.instance.ForNumber)
                        state2 = GameManager.instance.ShoulderRotation2_PGBS;
                    break;

                case GameManager.ExerciseList.FrontShoulder:
                    state = GameManager.instance.FrontShoulder_PGBS;
                    if (GameManager.instance.ForNumber)
                        state2 = GameManager.instance.FrontShoulder2_PGBS;
                    break;

                case GameManager.ExerciseList.SpineRotation:
                    state = GameManager.instance.SpineRotation_PGBS;
                    if (GameManager.instance.ForNumber)
                        state2 = GameManager.instance.SpineRotation2_PGBS;
                    break;
            }
        }

        maxFill = state[0] + state[1] + state[2];
        maxFill2 = state2[0] + state2[1] + state2[2];

        // 동그라미 퍼펙트, 굿, 배드 채우기
        CircleFill perfect = perfectCircle.GetComponent<CircleFill>();
        CircleFill good = goodCircle.GetComponent<CircleFill>();
        CircleFill bad = badCircle.GetComponent<CircleFill>();

        
        perfect.fillValue = state[0] / maxFill;
        good.fillValue = state[1] / maxFill;
        bad.fillValue = state[2] / maxFill;

        perfect.value = (int)state[0];
        good.value = (int)state[1];
        bad.value = (int)state[2];

        perfect.maxValue = (int)maxFill;
        good.maxValue = (int)maxFill;
        bad.maxValue = (int)maxFill;

        ProgressBar progressBar = AccuracyBar.GetComponent<ProgressBar>();
        progressBar.fillValue = state[3];

        // 2인용 일때 동그라미 퍼펙트, 굿, 배드 채우기
        if (GameManager.instance.ForNumber)
        {
            CircleFill perfect2 = perfectCircle2.GetComponent<CircleFill>();
            CircleFill good2 = goodCircle2.GetComponent<CircleFill>();
            CircleFill bad2 = badCircle2.GetComponent<CircleFill>();

            perfect2.fillValue = state2[0] / maxFill2;
            good2.fillValue = state2[1] / maxFill2;
            bad2.fillValue = state2[2] / maxFill2;

            perfect2.value = (int)state2[0];
            good2.value = (int)state2[1];
            bad2.value = (int)state2[2];

            perfect2.maxValue = (int)maxFill2;
            good2.maxValue = (int)maxFill2;
            bad2.maxValue = (int)maxFill2;

            ProgressBar progressBar2 = AccuracyBar2.GetComponent<ProgressBar>();
            progressBar2.fillValue = state2[3];
        }
    }
}