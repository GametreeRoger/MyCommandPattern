using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCommandExample : MonoBehaviour
{
    public float moveDistance = 1;
    public Map MapMgr;
    private MoveCommandReceiver moveCommandReciever;
    private List<MoveCommand> commands = new List<MoveCommand>();
    private int currentCommandNum = 0;

    private GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        moveCommandReciever = new MoveCommandReceiver();
    }

    public void Undo()
    {
        Debug.Log("Undo");
        if (currentCommandNum > 0)
        {
            currentCommandNum--;
            MoveCommand moveCommand = commands[currentCommandNum];
            moveCommand.UnExecute();
        }
    }

    public void Redo()
    {
        if (currentCommandNum < commands.Count)
        {
            MoveCommand moveCommand = commands[currentCommandNum];
            currentCommandNum++;
            moveCommand.Execute();
        }
    }

    private void Move(MoveDirection direction)
    {
        Debug.LogFormat("Move:{0}", direction);
        MoveCommand moveCommand = new MoveCommand(moveCommandReciever, direction, moveDistance, MapMgr.Role, MapMgr);
        moveCommand.Execute();
        commands.Add(moveCommand);
        currentCommandNum++;
    }


    //Simple move commands to attach to UI buttons
    public void MoveUp() { Move(MoveDirection.up); }
    public void MoveDown() { Move(MoveDirection.down); }
    public void MoveLeft() { Move(MoveDirection.left); }
    public void MoveRight() { Move(MoveDirection.right); }

    //Shows what's going on in the command list
    void OnGUI()
    {
        string label = "   start";
        if (currentCommandNum == 0)
        {
            label = ">" + label;
        }
        label += "\n";

        for (int i = 0; i < commands.Count; i++)
        {
            if (i == currentCommandNum - 1)
                label += "> " + commands[i].ToString() + "\n";
            else
                label += "   " + commands[i].ToString() + "\n";

        }
        guiStyle.fontSize = 40;
        GUI.Label(new Rect(0, 0, 400, 800), label, guiStyle);

        if(GUI.Button(new Rect(500, 10, 100, 50), "start"))
        {
            if (DeepFirstSearch())
                Debug.Log("Goal!!!");
            else
                Debug.Log("Star can not found.....");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Redo();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Undo();
        }
    }


    bool DeepFirstSearch()
    {
        if (MapMgr.IsGoal())
            return true;

        if (MapMgr.IsCanMove(MoveDirection.left))
        {
            MoveLeft();
            if (DeepFirstSearch())
                return true;
            else
                Undo();
        }

        if (MapMgr.IsCanMove(MoveDirection.up))
        {
            MoveUp();
            if (DeepFirstSearch())
                return true;
            else
                Undo();
        }

        if (MapMgr.IsCanMove(MoveDirection.right))
        {
            MoveRight();
            if (DeepFirstSearch())
                return true;
            else
                Undo();
        }

        if (MapMgr.IsCanMove(MoveDirection.down))
        {
            MoveDown();
            if (DeepFirstSearch())
                return true;
            else
                Undo();
        }

        return false;
    }
}
