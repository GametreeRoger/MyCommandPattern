using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public enum MapType
    {
        Wall,
        Star,
        Role
    }
    public Sprite OneUnit;
    public Sprite Star;

    public GameObject OneUnitPrefab;

    public TextAsset MapSource;

    private List<List<char>> mMapList = new List<List<char>>();

    private Vector2 mMapSize;

    private Vector2 mRoleIndex = new Vector2();

    public GameObject Role { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        loadMap();
        createMap();
    }

    private void loadMap()
    {
        string[] lines = MapSource.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            mMapList.Add(new List<char>());
            for (int j = 0; j < lines[i].Length; j++)
            {
                mMapList[i].Add(lines[i][j]);
                //Debug.Log(lines[i][j]);
            }
            //Debug.LogFormat("length:{0}, {1}", lines[i].Length, lines[i]);
        }

        mMapSize = new Vector2(mMapList[0].Count, mMapList.Count);
        //for(int i = 0; i < mMapList.Count; i++)
        //{
        //    for(int j = 0; j < mMapList[i].Count; j++)
        //    {
        //        Debug.Log(mMapList[i][j]);
        //    }
        //}
    }

    private void createMap()
    {
        Vector2 up_left = new Vector2();

        up_left.x = -((mMapSize.x - 1f) / 2f);
        up_left.y = (mMapSize.y - 1f) / 2f;

        //Debug.LogFormat("up_left:{0}", up_left);

        for (int i = 0; i < mMapList.Count; i++)
        {
            for (int j = 0; j < mMapList[i].Count; j++)
            {
                //Debug.Log(mMapList[i][j]);
                if (mMapList[i][j] != '.')
                {
                    getUnit(mMapList[i][j], new Vector3(up_left.x + j, up_left.y - i));
                    if ('o' == mMapList[i][j])
                    {
                        mRoleIndex = new Vector2(j, i);
                    }
                }
            }
        }
    }

    private void getUnit(char unitType, Vector3 position)
    {
        GameObject oneUnit = Instantiate(OneUnitPrefab);
        oneUnit.transform.SetParent(transform);
        oneUnit.transform.localPosition = position;
        switch (unitType)
        {
            case '*':
                oneUnit.name = "Wall";
                oneUnit.GetComponent<SpriteRenderer>().color = new Color(150f / 255f, 50f / 255f, 50f / 255f);
                break;
            case '+':
                oneUnit.name = "Star";
                SpriteRenderer renderer = oneUnit.GetComponent<SpriteRenderer>();
                renderer.sprite = Star;
                break;
            case 'o':
                oneUnit.name = "Role";
                oneUnit.GetComponent<SpriteRenderer>().color = Color.green;
                Role = oneUnit;
                break;
        }
        //return oneUnit;
    }

    private bool checkList(Vector2 index)
    {
        if (index.x < 0 || index.y < 0)
            return false;

        if (index.y >= mMapList.Count || index.x >= mMapList[0].Count)
            return false;

        if (isRoad(index) || isGoal(index))
            return true;

        return false;
    }

    private bool isRoad(Vector2 index)
    {
        return mMapList[(int)index.y][(int)index.x] == '.';
    }

    private bool isGoal(Vector2 index)
    {
        return mMapList[(int)index.y][(int)index.x] == '+';
    }

    public bool IsGoal()
    {
        return isGoal(mRoleIndex);
    }

    public bool IsCanMove(MoveDirection direction)
    {
        bool canMove = false;
        switch (direction)
        {
            case MoveDirection.left:
                canMove = checkList(new Vector2(mRoleIndex.x - 1, mRoleIndex.y));
                break;
            case MoveDirection.right:
                canMove = checkList(new Vector2(mRoleIndex.x + 1, mRoleIndex.y));
                break;
            case MoveDirection.up:
                canMove = checkList(new Vector2(mRoleIndex.x, mRoleIndex.y - 1));
                break;
            case MoveDirection.down:
                canMove = checkList(new Vector2(mRoleIndex.x, mRoleIndex.y + 1));
                break;
        }
        return canMove;
    }

    public void UpdateRoleIndex(MoveDirection direction, bool isUndo)
    {
        char sign = 'o';
        if (isUndo)
        {
            sign = '.';
            mMapList[(int)mRoleIndex.y][(int)mRoleIndex.x] = sign;
        }

        Vector2 temp;
        switch (direction)
        {
            case MoveDirection.left:
                temp = new Vector2(mRoleIndex.x - 1, mRoleIndex.y);
                if (!isGoal(temp) && !isUndo)
                    mMapList[(int)temp.y][(int)temp.x] = sign;

                mRoleIndex = temp;
                break;
            case MoveDirection.right:
                temp = new Vector2(mRoleIndex.x + 1, mRoleIndex.y);
                if (!isGoal(temp) && !isUndo)
                    mMapList[(int)temp.y][(int)temp.x] = sign;

                mRoleIndex = temp;
                break;
            case MoveDirection.up:
                temp = new Vector2(mRoleIndex.x, mRoleIndex.y - 1);
                if (!isGoal(temp) && !isUndo)
                    mMapList[(int)temp.y][(int)temp.x] = sign;

                mRoleIndex = temp;
                break;
            case MoveDirection.down:
                temp = new Vector2(mRoleIndex.x, mRoleIndex.y + 1);
                if (!isGoal(temp) && !isUndo)
                    mMapList[(int)temp.y][(int)temp.x] = sign;

                mRoleIndex = temp;
                break;
        }
    }

}
