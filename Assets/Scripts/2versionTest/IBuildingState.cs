using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int gridPos);
    void UpdateState(Vector3Int gridPos);
    Quaternion Rotate();
}