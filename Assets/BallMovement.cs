using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    public float powerMultiplier;
    public float maxDragLength = 100.0f;
    private LineRenderer lineRenderer;
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 startDragPos;

    public Text statusText;
    private string defaultText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        defaultText = statusText.text;

    }


    void Update()
    {
        bool isStopped = rb.velocity.magnitude < 0.1f;  // 공이 멈췄는지 여부 판단

        if (isStopped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                startDragPos = Input.mousePosition;
            }

            if (isDragging)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Vector3.Distance(Camera.main.transform.position, transform.position)));

                // 방향 벡터 계산
                Vector3 direction = (transform.position - mouseWorldPos).normalized;
                float lineLength = (mouseWorldPos - transform.position).magnitude;

                if (lineLength > maxDragLength)
                {
                    lineLength = maxDragLength;
                }

                // 시작점과 끝점 설정
                Vector3 lineStart = new Vector3(transform.position.x, 1, transform.position.z);
                Vector3 lineEnd = new Vector3(transform.position.x + direction.x * lineLength, 1, transform.position.z + direction.z * lineLength);

                DrawLine(lineStart, lineEnd);

                Vector3 endDragPos = Input.mousePosition;
                Vector3 dragVector = endDragPos - startDragPos;

                statusText.text = $"멈춤 상태: {isStopped}\n당겼을 때의 파워: {dragVector.magnitude * powerMultiplier}\n마우스와의 거리: {lineLength}";
            }
            else
            {
                statusText.text = defaultText;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector3 endDragPos = Input.mousePosition;
                Vector3 dragVector = endDragPos - startDragPos;

                if (dragVector.magnitude > maxDragLength)
                {
                    dragVector = dragVector.normalized * maxDragLength;
                }

                float dragDistance = dragVector.magnitude;
                float powerRatio = dragDistance / maxDragLength; // 0에서 1 사이의 값
                float appliedPower = powerMultiplier * powerRatio; // 만약 20만큼 당기면 appliedPower는 100이 됩니다.

                Vector3 force = new Vector3(-dragVector.x, 0, -dragVector.y) * appliedPower;
                rb.AddForce(force);

                isDragging = false;
                lineRenderer.enabled = false;
            }
        }
        else
        {
            statusText.text = $"멈춤 상태: {isStopped}";  // 공이 움직이고 있을 때의 상태만 표시
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}