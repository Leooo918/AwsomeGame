using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    private Vector3 startPos;
    private bool isSelecting = false;

    void Update()
    {
        // 드래그 시작
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isSelecting = true;
        }

        // 드래그 중
        if (Input.GetMouseButton(0))
        {
            // 드래그 영역을 시각적으로 보여줌 (옵션)
            DrawSelectionBox();

            // 드래그 영역을 계산
            Vector3 endPos = Input.mousePosition;

            // 스크린 좌표를 월드 좌표로 변환
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(startPos);
            float distance;
            plane.Raycast(ray, out distance);
            Vector3 boxStart = ray.GetPoint(distance);

            ray = Camera.main.ScreenPointToRay(endPos);
            plane.Raycast(ray, out distance);
            Vector3 boxEnd = ray.GetPoint(distance);

            // 박스 캐스트를 통해 드래그 영역 안의 Collider를 감지
            RaycastHit[] hits = Physics.BoxCastAll((boxStart + boxEnd) / 2, (boxEnd - boxStart) / 2, Vector3.forward);

            // 선택된 오브젝트 처리
            foreach (RaycastHit hit in hits)
            {
                // 선택된 오브젝트 처리 예시
                GameObject selectedObject = hit.collider.gameObject;
                Debug.Log("Selected: " + selectedObject.name);
            }
        }

        // 드래그 종료
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
        }
    }

    void DrawSelectionBox()
    {
        if (!isSelecting)
            return;

        // 드래그 영역을 시각적으로 보여주는 코드 (옵션)
        Vector3 currentMousePosition = Input.mousePosition;

        // 스크린 좌표를 월드 좌표로 변환
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(startPos);
        float distance;
        plane.Raycast(ray, out distance);
        Vector3 boxStart = ray.GetPoint(distance);

        ray = Camera.main.ScreenPointToRay(currentMousePosition);
        plane.Raycast(ray, out distance);
        Vector3 boxEnd = ray.GetPoint(distance);

        // 드래그 영역을 시각적으로 표시하는 코드 작성 (예: GUI.DrawTexture 등)
        // 이 부분은 화면에 드래그 영역을 표시하는 방법에 따라 달라질 수 있습니다.
    }
}
