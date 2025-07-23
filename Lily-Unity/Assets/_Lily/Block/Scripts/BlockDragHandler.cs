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
            //Note: ���[���h���W �� �{�[�h���W
            Vector2Int gridPos = new(
                Mathf.FloorToInt(hit.point.x),
                Mathf.FloorToInt(hit.point.z)
            );

            //Note: ���݂̃u���b�N�ʒu�Ƃ̍�������ړ��������v�Z
            Vector2Int moveDir = gridPos - controller.boardPosition;

            //Note: �����ʒu�Ȃ疳��
            if (moveDir == Vector2Int.zero)
            {
                return;
            }

            //Note: �z�u�\�Ȃ�ړ�
            if (blockPlacer.CanPlace(controller, gridPos, moveDir))
            {
                controller.MoveTo(gridPos);
            }
        }
    }
}