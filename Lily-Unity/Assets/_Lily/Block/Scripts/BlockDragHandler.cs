using UnityEngine;

[RequireComponent(typeof(BlockController))]
public class BlockDragHandler : MonoBehaviour
{
    private BlockController controller;
    private Camera          mainCamera;
    private BlockPlacer     blockPlacer;

    private void Start()
    {
        controller = GetComponent<BlockController>();
        mainCamera = Camera.main;
        blockPlacer = FindFirstObjectByType<BlockPlacer>();
    }

    private void OnMouseDrag()
    {
        if (mainCamera == null || blockPlacer == null || controller == null)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Board")))
        {
            //Note: ワールド座標 → ボード座標
            Vector2Int gridPos = new(
                Mathf.FloorToInt(hit.point.x),
                Mathf.FloorToInt(hit.point.z)
            );

            //Note: 現在のブロック位置との差分から移動方向を計算
            Vector2Int moveDir = gridPos - controller.boardPosition;

            //Note: 同じ位置なら無視
            if (moveDir == Vector2Int.zero)
            {
                return;
            }

            //Note: 配置可能なら移動
            if (blockPlacer.CanPlace(controller, gridPos, moveDir))
            {
                controller.MoveTo(gridPos);
            }
        }
    }
}