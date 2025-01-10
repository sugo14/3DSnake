using UnityEngine;

public enum Square {
    Top, Bottom, Left, Right, Front, Back
};

public class CubeOrient {
    public static int SquareSize = 2;

    public Square square;
    public Vector2Int pos;
    public Vector2Int dir;

    public CubeOrient() {
        square = Square.Left;
        pos = new Vector2Int(1, 1);
        dir = new Vector2Int(1, 0);
    }
    public CubeOrient(CubeOrient cubeOrient) {
        square = cubeOrient.square;
        pos = cubeOrient.pos;
        dir = cubeOrient.dir;
    }

    public void UpInput() {
        { dir = Vector2Int.up; }
    }
    public void DownInput() {
        { dir = Vector2Int.down; }
    }
    public void LeftInput() {
        { dir = Vector2Int.left; }
    }
    public void RightInput() {
        { dir = Vector2Int.right; }
    }

    // Returns the square that is to a direction of the current square.
    Square ToLeft() {
        if (square == Square.Right) { return Square.Front; }
        if (square == Square.Left) { return Square.Back; }
        return Square.Left;
    }
    Square ToRight() {
        if (square == Square.Right) { return Square.Back; }
        if (square == Square.Left) { return Square.Front; }
        return Square.Right;
    }
    Square Above() {
        if (square == Square.Top) { return Square.Back; }
        if (square == Square.Back) { return Square.Bottom; }
        if (square == Square.Bottom) { return Square.Front; }
        return Square.Top;
    } 
    Square Below() {
        if (square == Square.Top) { return Square.Front; }
        if (square == Square.Bottom) { return Square.Back; }
        if (square == Square.Back) { return Square.Top; }
        return Square.Bottom;
    }

    public void Go() {
        if (dir == Vector2Int.up) { GoUp(); }
        else if (dir == Vector2Int.down) { GoDown(); }
        else if (dir == Vector2Int.left) { GoLeft(); }
        else if (dir == Vector2Int.right) { GoRight(); }
    }

    void GoUp() {
        pos.y++;
        if (pos.y < SquareSize) { return; }
        if (square == Square.Left) {
            pos = new Vector2Int(0, pos.x);
            dir = Vector2Int.right;
        }
        else if (square == Square.Right) {
            pos = new Vector2Int(SquareSize - 1, pos.x);
            dir = Vector2Int.left;
        }
        else { pos.y = 0; }
        square = Above();
    }
    void GoDown() {
        pos.y--;
        if (pos.y >= 0) { return; }
        if (square == Square.Left) {
            pos = new Vector2Int(0, pos.x);
            dir = Vector2Int.right;
        }
        else if (square == Square.Right) {
            pos = new Vector2Int(SquareSize - 1, SquareSize - pos.x - 1);
            dir = Vector2Int.left;
        }
        else { pos.y = SquareSize - 1; }
        square = Below();
    }
    void GoLeft() {
        pos.x--;
        if (pos.x >= 0) { return; }
        else if (square == Square.Top) {
            pos = new Vector2Int(0, pos.x);
            dir = Vector2Int.down;
        }
        else if (square == Square.Bottom) {
            pos = new Vector2Int(pos.y, 0);
            dir = Vector2Int.up;
        }
        if (square == Square.Left) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.y);
            dir = Vector2Int.right;
        }
        else if (square == Square.Back) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.y);
            dir = Vector2Int.right;
        }
        else { pos.x = SquareSize - 1; }
        square = ToLeft();
    }
    void GoRight() {
        pos.x++;
        if (pos.x < SquareSize) { return; }
        else if (square == Square.Top) {
            pos = new Vector2Int(pos.y, SquareSize - 1);
            dir = Vector2Int.left;
        }
        else if (square == Square.Bottom) {
            pos = new Vector2Int(SquareSize - 1 - pos.y, 0);
            dir = Vector2Int.up;
        }
        if (square == Square.Left) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.y);
            dir = Vector2Int.left;
        }
        else if (square == Square.Back) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.y);
            dir = Vector2Int.left;
        }
        else { pos.x = 0; }
        square = ToRight();
    }

    public Vector3 WorldPosition() {
        Vector3 vec = Vector3.zero;
        float half = (SquareSize-1)/2.0f;
        if (square == Square.Top) {
            vec.y = half + 1;
            vec.x = pos.x - half;
            vec.z = pos.y - half;
        }
        if (square == Square.Bottom) {
            vec.y = -half - 1;
            vec.x = pos.x - half;
            vec.z = half - pos.y;
        }
        if (square == Square.Back) {
            vec.z = half + 1;
            vec.x = pos.x - half;
            vec.y = half - pos.y;
        }
        if (square == Square.Front) {
            vec.z = -half - 1;
            vec.x = pos.x - half;
            vec.y = pos.y - half;
        }
        return vec;
    }

    public Vector3 SnakeUp() {
        CubeOrient next = new CubeOrient(this);
        next.GoUp();
        return next.WorldPosition() - WorldPosition();
    }

    public Vector3 WorldDirection() {
        CubeOrient next = new CubeOrient(this);
        next.Go();
        return next.WorldPosition() - WorldPosition();
    }

    public override string ToString()
    {
        Vector3 wp = WorldPosition();
        return $"{square.ToString()} ({pos.x},{pos.y}) ({wp.x},{wp.y},{wp.z})";
    }
}
