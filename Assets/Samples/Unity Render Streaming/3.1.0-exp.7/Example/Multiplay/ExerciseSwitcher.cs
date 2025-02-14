using UnityEngine;
using UnityEngine.UI;

public class ExerciseSwitcher : MonoBehaviour
{
    public GameObject[] objects;  // 6개의 운동을 담을 배열
    public Button Move_Left;
    public Button Move_Right;
    private int currentIndex = 0;

    void Start()
    {
        // 초기 상태 설정: 첫 번째 오브젝트만 활성화
        UpdateObjectVisibility();

        // 버튼 클릭 이벤트에 메서드 연결
        Move_Left.onClick.AddListener(ShowPreviousObject);
        Move_Right.onClick.AddListener(ShowNextObject);
    }

    void UpdateObjectVisibility()
    {
        // 모든 오브젝트를 비활성화하고 현재 인덱스의 오브젝트만 활성화
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == currentIndex);
        }
    }

    void ShowPreviousObject()
    {
        // 현재 인덱스를 감소시키고 범위를 벗어나면 마지막 인덱스로 설정
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length;
        UpdateObjectVisibility();
    }

    void ShowNextObject()
    {
        // 현재 인덱스를 증가시키고 범위를 벗어나면 첫 번째 인덱스로 설정
        currentIndex = (currentIndex + 1) % objects.Length;
        UpdateObjectVisibility();
    }
}
