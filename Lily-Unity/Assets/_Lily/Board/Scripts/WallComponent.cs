using UnityEngine;

public enum WallDirection
{
    Up,
    Down,
    Left,
    Right
}

public class WallComponent : MonoBehaviour
{
    public Vector2Int boardPosition;  // 壁が隣接するタイルの位置（左下など）
    public WallDirection direction;   // どの方向に壁が存在するか
    public Color wallColor;

#if UNITY_EDITOR
    private void OnValidate()
    {
        boardPosition = new Vector2Int(
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.z)
        );
    }
#endif
}