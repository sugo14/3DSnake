using UnityEngine;

public enum Square {
    Top, Bottom, Left, Right, Front, Back
};

public class CubeOrient {
    public static int SquareSize = 3;

    public Square square;
    public Vector2Int pos;
    public Vector2Int dir;

    public CubeOrient() {
        square = Square.Front;
        pos = new Vector2Int(1, 1);
        dir = new Vector2Int(1, 0);
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

    public void GoUp() {
        pos.y++;
        if (pos.y >= SquareSize) {
            square = Above();
            pos.y = 0;
        }
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
            vec.x = half - pos.x;
            vec.z = half - pos.y;
        }
        if (square == Square.Back) {
            vec.z = half + 1;
            vec.x = half - pos.x;
            vec.y = half - pos.y;
        }
        if (square == Square.Front) {
            vec.z = -half - 1;
            vec.x = pos.x - half;
            vec.y = pos.y - half;
        }
        return vec;
    }

    public override string ToString()
    {
        Vector3 wp = WorldPosition();
        return $"{square.ToString()} ({pos.x},{pos.y}) ({wp.x},{wp.y},{wp.z})";
    }
}
