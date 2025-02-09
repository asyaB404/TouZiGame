using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using Unity.VisualScripting;
using UnityEngine;

public class CamarControl : MonoBehaviour
{
    public static CamarControl Instance;
    private void Awake()
    {
        Instance = this;
    }
    // 记录鼠标是否按下
    private bool isRightMouseButtonDown;
    // 记录鼠标按下时的位置
    private Vector3 mouseDownPosition;
    public float threshold = 10.0f; // 阈值，可以根据需要调整
    public float rotationSpeed = 0.5f; // 旋转速度，可以根据需要调整
    public GameObject[] fires;
    void Update()
    {
        // 检查鼠标右键是否按下
        if (Input.GetMouseButtonDown(1)) // 1 表示鼠标右键
        {
            isRightMouseButtonDown = true;
            mouseDownPosition = Input.mousePosition;
        }

        // 检查鼠标右键是否抬起
        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonDown = false;
        }

        // 如果鼠标右键保持按下状态，并且鼠标位置发生了变化
        if (isRightMouseButtonDown)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - mouseDownPosition;

            // 检测拖动距离（例如，如果拖动超过一定阈值）

            if (mouseDelta.magnitude > threshold)
            {
                OnRightMouseDrag(mouseDelta);
                // 重置鼠标按下位置，以避免重复触发
                mouseDownPosition = currentMousePosition;
            }
        }
    }
    int maxX = 25;
    int maxY = 30;
    // 自定义的拖动处理函数
    private void OnRightMouseDrag(Vector3 delta)
    {
        if (!GameManager.Instance.gameObject.activeSelf) return;
        Quaternion xRotation = Quaternion.Euler(-delta.y * rotationSpeed, 0, 0);
        Quaternion yRotation = Quaternion.Euler(0, delta.x * rotationSpeed, 0);
        gameObject.transform.rotation *= xRotation * yRotation;
        Vector3 eulerAngles = gameObject.transform.rotation.eulerAngles;
        eulerAngles.z = 0;
        if (eulerAngles.y > maxY + 180)
            eulerAngles.y -= 360;
        if (eulerAngles.x > maxX + 180)
            eulerAngles.x -= 360;
        if (eulerAngles.x > maxX)
        {
            // Debug.Log(eulerAngles.x);
            eulerAngles.x = maxX;
        }
        if (eulerAngles.x < -maxX)
            eulerAngles.x = -maxX;
        if (eulerAngles.y > maxY)
            eulerAngles.y = maxY;
        if (eulerAngles.y < -maxY)
            eulerAngles.y = -maxY;


        gameObject.transform.rotation = Quaternion.Euler(eulerAngles);
    }
    public void setFires(int num)
    {
        fires[num].SetActive(true);
        fires[1 - num].SetActive(false);
    }
    public void hideFires(){
        fires[0].SetActive(false);
        fires[1].SetActive(false);
    }
    public void Reset()
    {
        Debug.Log("reset");
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
