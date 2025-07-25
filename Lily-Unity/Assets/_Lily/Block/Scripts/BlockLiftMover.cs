using UnityEngine;

//Note: ドラッグ時の移動・着地
public class BlockLiftMover : MonoBehaviour
{
    private BlockController controller;
    private Camera          mainCamera;
    private BlockPlacer     blockPlacer;
    private float           liftHeight = 0f;

    private void Start()
    {
        controller = GetComponent<BlockController>();
        mainCamera = Camera.main;
        blockPlacer = FindFirstObjectByType<BlockPlacer>();
    }

    private void OnMouseDrag()
    {
        if (!mainCamera || !blockPlacer || !controller)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Board")))
        {
            Vector2Int gridPos = BoardCoordinateHelper.WorldToBoard(hit.point);
            Vector2Int moveDir = gridPos - controller.boardPosition;

            if (blockPlacer.CanPlace(controller, gridPos, moveDir))
            {
                Vector3 liftedPos = BoardCoordinateHelper.BoardToWorld(gridPos, controller.shapeType, transform.position.y + liftHeight);
                transform.position = liftedPos;
                controller.MoveTo(gridPos);
            }
        }
    }
}