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
                Vector3 endDragPos = Input.mousePosition;
                Vector3 dragVector = endDragPos - startDragPos;
                Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(startDragPos.x, startDragPos.y, Camera.main.nearClipPlane));
                Vector3 worldEnd = Camera.main.ScreenToWorldPoint(new Vector3(endDragPos.x, endDragPos.y, Camera.main.nearClipPlane));
                DrawLine(worldStart, worldEnd);

                float distance = Vector3.Distance(transform.position, worldEnd);
                statusText.text = $"���� ����: {isStopped}\n����� ���� �Ŀ�: {dragVector.magnitude * powerMultiplier}\n���콺���� �Ÿ�: {distance}";
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

                Vector3 force = new Vector3(-dragVector.x, 0, -dragVector.y) * powerMultiplier;
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