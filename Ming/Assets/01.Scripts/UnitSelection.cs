using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    private Vector3 startPos;
    private bool isSelecting = false;

    void Update()
    {
        // �巡�� ����
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isSelecting = true;
        }

        // �巡�� ��
        if (Input.GetMouseButton(0))
        {
            // �巡�� ������ �ð������� ������ (�ɼ�)
            DrawSelectionBox();

            // �巡�� ������ ���
            Vector3 endPos = Input.mousePosition;

            // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(startPos);
            float distance;
            plane.Raycast(ray, out distance);
            Vector3 boxStart = ray.GetPoint(distance);

            ray = Camera.main.ScreenPointToRay(endPos);
            plane.Raycast(ray, out distance);
            Vector3 boxEnd = ray.GetPoint(distance);

            // �ڽ� ĳ��Ʈ�� ���� �巡�� ���� ���� Collider�� ����
            RaycastHit[] hits = Physics.BoxCastAll((boxStart + boxEnd) / 2, (boxEnd - boxStart) / 2, Vector3.forward);

            // ���õ� ������Ʈ ó��
            foreach (RaycastHit hit in hits)
            {
                // ���õ� ������Ʈ ó�� ����
                GameObject selectedObject = hit.collider.gameObject;
                Debug.Log("Selected: " + selectedObject.name);
            }
        }

        // �巡�� ����
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
        }
    }

    void DrawSelectionBox()
    {
        if (!isSelecting)
            return;

        // �巡�� ������ �ð������� �����ִ� �ڵ� (�ɼ�)
        Vector3 currentMousePosition = Input.mousePosition;

        // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(startPos);
        float distance;
        plane.Raycast(ray, out distance);
        Vector3 boxStart = ray.GetPoint(distance);

        ray = Camera.main.ScreenPointToRay(currentMousePosition);
        plane.Raycast(ray, out distance);
        Vector3 boxEnd = ray.GetPoint(distance);

        // �巡�� ������ �ð������� ǥ���ϴ� �ڵ� �ۼ� (��: GUI.DrawTexture ��)
        // �� �κ��� ȭ�鿡 �巡�� ������ ǥ���ϴ� ����� ���� �޶��� �� �ֽ��ϴ�.
    }
}
