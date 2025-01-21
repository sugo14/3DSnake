using UnityEngine;

public enum SideEffects {
    Normal,
    StopGrow,
    StopShrink,
    SpeedUp,
    SlowDown,
    DoublePoints,
    HalfPoints
}

public class CubeScript : MonoBehaviour
{
    public SideEffects[] sideEffects = new SideEffects[6];

    void Start() {
        sideEffects[0] = SideEffects.SpeedUp;
        sideEffects[1] = SideEffects.Normal;
        sideEffects[2] = SideEffects.Normal;
        sideEffects[3] = SideEffects.Normal;
        sideEffects[4] = SideEffects.Normal;
        sideEffects[5] = SideEffects.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        /* front.transform.position = new Vector3(0, 0, sideLength / 2);
        back.transform.position = new Vector3(0, 0, -sideLength / 2);
        left.transform.position = new Vector3(sideLength / 2, 0, 0);
        right.transform.position = new Vector3(-sideLength / 2, 0, 0);
        top.transform.position = new Vector3(0, sideLength / 2, 0);
        bottom.transform.position = new Vector3(0, -sideLength / 2, 0);

        front.transform.rotation = Quaternion.Euler(0, 0, 0); */
    }
}
