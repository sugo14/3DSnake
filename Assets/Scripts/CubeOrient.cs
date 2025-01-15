using System;
using NUnit.Framework.Internal;
using UnityEngine;

public enum Square {
    Top, Bottom, Left, Right, Front, Back
};

public class CubeOrient {
    public static int SquareSize = 7;

    public Square square;
    public Vector2Int pos;
    public Vector2Int dir;

    public Square upSquare;
    bool goingInWorldUp;
    // so what i'm storing as the context of the world up direction is the up square
    // this means i have to make the up direction pressed the direction that leads to the up square
    // there are both funny ways and good ways to do this
    // funny way: compare all directions and their displacement from the up cube
    // performant way: hardcode? i mean really comparing isnt even that slow
    // for now i will just compare to find the up direction


    public CubeOrient() {
        square = Square.Left;
        pos = new Vector2Int(SquareSize/2, SquareSize/2);
        dir = new Vector2Int(1, 0);
    }
    public CubeOrient(CubeOrient cubeOrient) {
        square = cubeOrient.square;
        pos = cubeOrient.pos;
        dir = cubeOrient.dir;
    }

    public Vector2Int WorldUp() {
        Vector3 next = FacePosition(upSquare);
        CubeOrient t1 = new CubeOrient(this), t2 = new CubeOrient(this), t3 = new CubeOrient(this), t4 = new CubeOrient(this);
        t1.GoUp();
        t2.GoDown();
        t3.GoLeft();
        t4.GoRight();
        Vector3 u = t1.WorldPosition(), d = t2.WorldPosition(), l = t3.WorldPosition(), r = t4.WorldPosition();
        float w = Vector3.Distance(u, FacePosition(upSquare)), x = Vector3.Distance(d, FacePosition(upSquare)), y = Vector3.Distance(l, FacePosition(upSquare)), z = Vector3.Distance(r, FacePosition(upSquare));
        float min = Math.Min(w, Math.Min(x, Math.Min(y, z)));
        if (min == z) { return Vector2Int.right; }
        if (min == x) { return Vector2Int.down; }
        if (min == y) { return Vector2Int.left; }
        return Vector2Int.up;
    }

    // Transform a vector by 90 degrees to the left.
    Vector2Int Left(Vector2Int vec) {
        return new Vector2Int(-vec.y, vec.x);
    }
    // Transform a vector by 90 degrees to the right.
    Vector2Int Right(Vector2Int vec) {
        Debug.Log(vec);
        Debug.Log("right");
        return new Vector2Int(vec.y, -vec.x);
    }
    // Transform a vector by 180 degrees.
    Vector2Int Opposite(Vector2Int vec) {
        return new Vector2Int(-vec.x, -vec.y);
    }

    public void UpInput() {
        {
            dir = WorldUp();
            goingInWorldUp = true;
        }
    }
    public void DownInput() {
        {
            dir = Opposite(WorldUp());
            goingInWorldUp = false;
        }
    }
    public void LeftInput() {
        {
            dir = Left(WorldUp());
            goingInWorldUp = false;
        }
    }
    public void RightInput() {
        {
            dir = Right(WorldUp());
            goingInWorldUp = false;
        }
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
    Square Next() {
        if (dir == Vector2.up) { return Above(); }
        if (dir == Vector2.down) { return Below(); }
        if (dir == Vector2.left) { return ToLeft(); }
        if (dir == Vector2.right) { return ToRight(); }
        return square;
    }

    public void Go() {
        Square prev = square;
        if (dir == Vector2Int.up) { GoUp(); }
        else if (dir == Vector2Int.down) { GoDown(); }
        else if (dir == Vector2Int.left) { GoLeft(); }
        else if (dir == Vector2Int.right) { GoRight(); }
        if (square != prev && goingInWorldUp) { upSquare = Next(); }
        else if (square != prev && dir == -WorldUp()) { upSquare = prev; }
    }

    void GoUp() {
        pos.y++;
        if (pos.y < SquareSize) { return; }
        if (square == Square.Left) {
            pos = new Vector2Int(0, SquareSize - 1 - pos.x);
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
            pos = new Vector2Int(SquareSize - 1 - pos.y, SquareSize - 1);
            dir = Vector2Int.down;
        }
        else if (square == Square.Bottom) {
            pos = new Vector2Int(pos.y, 0);
            dir = Vector2Int.up;
        }
        else if (square == Square.Left) {
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
            dir = Vector2Int.down;
        }
        else if (square == Square.Bottom) {
            pos = new Vector2Int(SquareSize - 1 - pos.y, 0);
            dir = Vector2Int.up;
        }
        else if (square == Square.Right) {
            pos = new Vector2Int(SquareSize - 1, SquareSize - 1 - pos.y);
            dir = Vector2Int.left;
        }
        else if (square == Square.Back) {
            pos = new Vector2Int(SquareSize - 1, SquareSize - 1 - pos.y);
            dir = Vector2Int.left;
        }
        else { pos.x = 0; }
        square = ToRight();
    }

    public Vector3 WorldPosition() {
        Vector3 vec = Vector3.zero;
        float half = (SquareSize - 1) / 2.0f;
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
        if (square == Square.Left) {
            vec.x = -half - 1;
            vec.z = half - pos.x;
            vec.y = pos.y - half;
        }
        if (square == Square.Right) {
            vec.x = half + 1;
            vec.z = pos.x - half;
            vec.y = pos.y - half;
        }
        return vec;
    }

    public Vector3 FacePosition(Square face) {
        CubeOrient faceOrient = new CubeOrient();
        faceOrient.square = face;
        faceOrient.pos = new Vector2Int(SquareSize / 2, SquareSize / 2);
        return faceOrient.WorldPosition();
    }

    public Vector3 SnakeUp() {
        /* CubeOrient next = new CubeOrient(this);
        next.GoUp();
        return next.WorldPosition() - WorldPosition(); */
        /* return FacePosition(upSquare) - WorldPosition(); */
        // instantiate a cube at face position in the below line
        Vector3 facePos = FacePosition(upSquare);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject instance = GameObject.Instantiate(cube, facePos, Quaternion.identity);
        // delete the instantiated cube after one frame in the below line
        GameObject.Destroy(instance, 0.016f);
        return facePos - WorldPosition();
    }

    public Vector3 WorldDirection() {
        CubeOrient next = new CubeOrient(this);
        next.Go();
        return next.WorldPosition() - WorldPosition();
    }

    public override string ToString()
    {
        Vector3 wp = WorldPosition();
        return $"{square.ToString()} ({pos.x},{pos.y}) ({dir.x},{dir.y}) ({wp.x},{wp.y},{wp.z}) {upSquare.ToString()} {goingInWorldUp}";
    }
}
