/// <summary>
/// A simple example of a class inheriting from a command pattern
/// This handles execution of the command as well as unexecution of the command
/// </summary>

using System.Collections;
using UnityEngine;

// A basic enum to describe our movement
public enum MoveDirection { up, down, left, right };

class MoveCommand : Command
{
    private MoveDirection _direction;
    private MoveCommandReceiver _receiver;
    private float _distance;
    private GameObject _gameObject;
    private Map _Map;

    //Constructor
    public MoveCommand(MoveCommandReceiver reciever, MoveDirection direction, float distance, GameObject gameObjectToMove, Map map)
    {
        this._receiver = reciever;
        this._direction = direction;
        this._distance = distance;
        this._gameObject = gameObjectToMove;
        this._Map = map;
    }

    //Execute new command
    public IEnumerator Execute()
    {
        yield return _receiver.MoveOperation(_gameObject, _Map, _direction, _distance, false);
    }

    //Undo last command
    public IEnumerator UnExecute()
    {
        yield return _receiver.MoveOperation(_gameObject, _Map, _direction, _distance, true);
    }

    //So we can show this command in debug output easily
    public override string ToString()
    {
        return _gameObject.name + " : " + MoveDirectionString(_direction) + " : " + _distance.ToString();
    }

    //Convert the MoveDirection enum to a string for debug
    public string MoveDirectionString(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.up:
                return "up";
            case MoveDirection.down:
                return "down";
            case MoveDirection.left:
                return "left";
            case MoveDirection.right:
                return "right";
            default:
                return "unkown";
        }
    }
}
