/// <summary>
/// The 'Receiver' class - this handles what a move command actually does
/// </summary>

using UnityEngine;

class MoveCommandReceiver
{
    public void MoveOperation(GameObject gameObjectToMove, Map map, MoveDirection direction, float distance, bool isUndo)
    {
        if (isUndo)
            direction = InverseDirection(direction);

        switch (direction)
        {
            case MoveDirection.up:
                MoveY(gameObjectToMove, distance);
                break;
            case MoveDirection.down:
                MoveY(gameObjectToMove, -distance);
                break;
            case MoveDirection.left:
                MoveX(gameObjectToMove, -distance);
                break;
            case MoveDirection.right:
                MoveX(gameObjectToMove, distance);
                break;
        }
        map.UpdateRoleIndex(direction, isUndo);
    }

    private void MoveY(GameObject gameObjectToMove, float distance)
    {
        Vector3 newPos = gameObjectToMove.transform.position;
        newPos.y += distance;
        gameObjectToMove.transform.position = newPos;
    }

    private void MoveX(GameObject gameObjectToMove, float distance)
    {
        Vector3 newPos = gameObjectToMove.transform.position;
        newPos.x += distance;
        gameObjectToMove.transform.position = newPos;
    }

    //invert the direction for undo
    private MoveDirection InverseDirection(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.up:
                return MoveDirection.down;
            case MoveDirection.down:
                return MoveDirection.up;
            case MoveDirection.left:
                return MoveDirection.right;
            case MoveDirection.right:
                return MoveDirection.left;
            default:
                Debug.LogError("Unknown MoveDirection");
                return MoveDirection.up;
        }
    }
}

