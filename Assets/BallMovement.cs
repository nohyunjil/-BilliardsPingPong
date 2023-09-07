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
        bool isStopped = rb.velocity.magnitude < 0.1f;  // ���� ������� ���� �Ǵ�

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

                // ���� ���� ���
                Vector3 direction = (transform.position - mouseWorldPos).normalized;
                float lineLength = (mouseWorldPos - transform.position).magnitude;

                if (lineLength > maxDragLength)
                {
                    lineLength = maxDragLength;
                }

                // �������� ���� ����
                Vector3 lineStart = new Vector3(transform.position.x, 1, transform.position.z);
                Vector3 lineEnd = new Vector3(transform.position.x + direction.x * lineLength, 1, transform.position.z + direction.z * lineLength);

                DrawLine(lineStart, lineEnd);

                Vector3 endDragPos = Input.mousePosition;
                Vector3 dragVector = endDragPos - startDragPos;

                statusText.text = $"���� ����: {isStopped}\n����� ���� �Ŀ�: {dragVector.magnitude * powerMultiplier}\n���콺���� �Ÿ�: {lineLength}";
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
                float powerRatio = dragDistance / maxDragLength; // 0���� 1 ������ ��
                float appliedPower = powerMultiplier * powerRatio; // ���� 20��ŭ ���� appliedPower�� 100�� �˴ϴ�.

                Vector3 force = new Vector3(-dragVector.x, 0, -dragVector.y) * appliedPower;
                rb.AddForce(force);

                isDragging = false;
                lineRenderer.enabled = false;
            }
        }
        else
        {
            statusText.text = $"���� ����: {isStopped}";  // ���� �����̰� ���� ���� ���¸� ǥ��
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}