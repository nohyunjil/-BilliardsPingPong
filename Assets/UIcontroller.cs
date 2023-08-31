using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    public float powerMultiplier = 10.0f;
    public float maxDragLength = 100.0f;
    private LineRenderer lineRenderer;  // LineRenderer ����
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 startDragPos;

    // �ؽ�Ʈ UI ��Ҹ� �����ϱ� ���� ����
    public Text statusText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;  // �ʱ⿡�� ������ ������ �ʰ� ����
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
                // �巡�� ���� �� ������ ǥ��
                Vector3 endDragPos = Input.mousePosition;
                Vector3 dragVector = endDragPos - startDragPos;
                Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(startDragPos.x, startDragPos.y, Camera.main.nearClipPlane));
                Vector3 worldEnd = Camera.main.ScreenToWorldPoint(new Vector3(endDragPos.x, endDragPos.y, Camera.main.nearClipPlane));
                DrawLine(worldStart, worldEnd);

                float distance = Vector3.Distance(transform.position, worldEnd);
                statusText.text = $"���� ����: {rb.velocity.magnitude < 0.1f}\n����� ���� �Ŀ�: {dragVector.magnitude * powerMultiplier}\n�� ��ǥ���� ���콺���� �Ÿ�: {distance}";
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
                lineRenderer.enabled = false;  // ���콺�� ������ �� ���� �����
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
