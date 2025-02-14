using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject; // 활성화/비활성화할 오브젝트

    public void Toggle()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
