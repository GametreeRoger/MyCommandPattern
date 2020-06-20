using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MyCommandExample : MonoBehaviour
{
    public float moveDistance = 1;
    public Map MapMgr;
    private MoveCommandReceiver moveCommandReciever;
    private Stack<MoveCommand> commands = new Stack<MoveCommand>();

    private GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        moveCommandReciever = new MoveCommandReceiver();
    }

    public IEnumerator Undo()
    {
        Debug.Log("Undo");
        if (commands.Count > 0)
        {
            MoveCommand moveCommand = commands.Pop();
            yield return moveCommand.UnExecute();
        }
    }

    private IEnumerator Move(MoveDirection direction)
    {
        Debug.LogFormat("Move:{0}", direction);
        MoveCommand moveCommand = new MoveCommand(moveCommandReciever, direction, moveDistance, MapMgr.Role, MapMgr);
        commands.Push(moveCommand);
        yield return moveCommand.Execute();
    }

    //Simple move commands to attach to UI buttons
    public IEnumerator MoveUp() { yield return Move(MoveDirection.up); }
    public IEnumerator MoveDown() { yield return Move(MoveDirection.down); }
    public IEnumerator MoveLeft() { yield return Move(MoveDirection.left); }
    public IEnumerator MoveRight() { yield return Move(MoveDirection.right); }

    //Shows what's going on in the command list
    void OnGUI()
    {
        string label = "";

        foreach(MoveCommand com in commands)
        {
            label = "   " + com.ToString() + "\n" + label;
        }
        guiStyle.fontSize = 40;
        GUI.Label(new Rect(0, 0, 400, 800), label, guiStyle);

        if(GUI.Button(new Rect(500, 10, 100, 50), "start"))
        {
            StartCoroutine(doSearchPath());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(MoveUp());
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(MoveDown());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(MoveLeft());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(MoveRight());
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(Undo());
        }
    }

    IEnumerator doSearchPath()
    {
        yield return DeepFirstSearch();
        if (mIsRightWay)
            Debug.Log("Goal!!!!");
        else
            Debug.Log("Star not found....");
    }

    private bool mIsRightWay = false;

    IEnumerator DeepFirstSearch()
    {
        if (MapMgr.IsGoal())
        {
            mIsRightWay = true;
            
            yield break;
        }

        if (MapMgr.IsCanMove(MoveDirection.left))
        {
            yield return MoveLeft();
            yield return DeepFirstSearch();

            if (!mIsRightWay)
                yield return Undo();
            else
                yield break;
        }

        if (MapMgr.IsCanMove(MoveDirection.up))
        {
            yield return MoveUp();
            yield return DeepFirstSearch();
            if (!mIsRightWay)
                yield return Undo();
            else
                yield break;
        }

        if (MapMgr.IsCanMove(MoveDirection.right))
        {
            yield return MoveRight();
            yield return DeepFirstSearch();
            if (!mIsRightWay)
                yield return Undo();
            else
                yield break;
        }

        if (MapMgr.IsCanMove(MoveDirection.down))
        {
            yield return MoveDown();
            yield return DeepFirstSearch();
            if (!mIsRightWay)
                yield return Undo();
            else
                yield break;
        }

        mIsRightWay = false;
        yield break;
    }
}
