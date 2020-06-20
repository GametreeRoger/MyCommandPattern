/// <summary>
/// The 'Receiver' class - this handles what a move command actually does
/// </summary>

using System.Collections;
using DG.Tweening;
using UnityEngine;

class MoveCommandReceiver
{
    const float MOVE_SPEED = 0.5f;
    public IEnumerator MoveOperation(GameObject gameObjectToMove, Map map, MoveDirection direction, float distance, bool isUndo)
    {
        if (isUndo)
            direction = InverseDirection(direction);

        map.UpdateRoleIndex(direction, isUndo);

        switch (direction)
        {
            case MoveDirection.up:
                yield return MoveY(gameObjectToMove, distance);
                break;
            case MoveDirection.down:
                yield return MoveY(gameObjectToMove, -distance);
                break;
            case MoveDirection.left:
                yield return MoveX(gameObjectToMove, -distance);
                break;
            case MoveDirection.right:
            default:
                yield return MoveX(gameObjectToMove, distance);
                break;
        }
        
    }

    IEnumerator MoveY(GameObject gameObjectToMove, float distance)
    {
        Vector3 newPos = gameObjectToMove.transform.position;
        newPos.y += distance;
        yield return gameObjectToMove.transform.DOMove(newPos, MOVE_SPEED).WaitForCompletion();
    }

    IEnumerator MoveX(GameObject gameObjectToMove, float distance)
    {
        Vector3 newPos = gameObjectToMove.transform.position;
        newPos.x += distance;
        yield return gameObjectToMove.transform.DOMove(newPos, MOVE_SPEED).WaitForCompletion();
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

