using UnityEngine;
using UnityEngine.UI;

public class ExerciseSwitcher : MonoBehaviour
{
    public GameObject[] objects;  // 6���� ��� ���� �迭
    public Button Move_Left;
    public Button Move_Right;
    private int currentIndex = 0;

    void Start()
    {
        // �ʱ� ���� ����: ù ��° ������Ʈ�� Ȱ��ȭ
        UpdateObjectVisibility();

        // ��ư Ŭ�� �̺�Ʈ�� �޼��� ����
        Move_Left.onClick.AddListener(ShowPreviousObject);
        Move_Right.onClick.AddListener(ShowNextObject);
    }

    void UpdateObjectVisibility()
    {
        // ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� ���� �ε����� ������Ʈ�� Ȱ��ȭ
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == currentIndex);
        }
    }

    void ShowPreviousObject()
    {
        // ���� �ε����� ���ҽ�Ű�� ������ ����� ������ �ε����� ����
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length;
        UpdateObjectVisibility();
    }

    void ShowNextObject()
    {
        // ���� �ε����� ������Ű�� ������ ����� ù ��° �ε����� ����
        currentIndex = (currentIndex + 1) % objects.Length;
        UpdateObjectVisibility();
    }
}
