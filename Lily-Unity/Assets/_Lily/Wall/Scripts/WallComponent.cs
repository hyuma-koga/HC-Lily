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
    public Vector2Int boardPosition;  // �ǂ��אڂ���^�C���̈ʒu�i�����Ȃǁj
    public WallDirection direction;   // �ǂ̕����ɕǂ����݂��邩
    public Color wallColor;

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            wallColor = renderer.material.color;
        }
    }

    private void OnValidate()
    {
        boardPosition = new Vector2Int(
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.z)
        );
    }
}