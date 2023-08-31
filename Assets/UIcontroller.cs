using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    public float powerMultiplier = 10.0f;
    public float maxDragLength = 100.0f;
    private LineRenderer lineRenderer;  // LineRenderer 참조
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 startDragPos;

    // 텍스트 UI 요소를 참조하기 위한 변수
    public Text statusText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;  // 초기에는 라인을 보이지 않게 설정
    }

    void Update()
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                startDragPos = Input.mousePosition;
            }

            if (isDragging)
            {
                // 드래그 중일 때 라인을 표시
                Vector3 endDragPos = Input.mousePosition;
                Vector3 dragVector = endDragPos - startDragPos;
                Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(startDragPos.x, startDragPos.y, Camera.main.nearClipPlane));
                Vector3 worldEnd = Camera.main.ScreenToWorldPoint(new Vector3(endDragPos.x, endDragPos.y, Camera.main.nearClipPlane));
                DrawLine(worldStart, worldEnd);

                float distance = Vector3.Distance(transform.position, worldEnd);
                statusText.text = $"멈춤 상태: {rb.velocity.magnitude < 0.1f}\n당겼을 때의 파워: {dragVector.magnitude * powerMultiplier}\n공 좌표에서 마우스와의 거리: {distance}";
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector3 endDragPos = Input.mousePosition;
                Vector3 dragVector = endDragPos - startDragPos;

                if (dragVector.magnitude > maxDragLength)
                {
                    dragVector = dragVector.normalized * maxDragLength;
                }

                Vector3 force = new Vector3(-dragVector.x, 0, -dragVector.y) * powerMultiplier;
                rb.AddForce(force);

                isDragging = false;
                lineRenderer.enabled = false;  // 마우스를 놓았을 때 라인 숨기기
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
