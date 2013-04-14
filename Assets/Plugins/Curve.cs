using UnityEngine;

[System.Serializable]
public class Curve : System.Object
{
	public static Vector3 ERROR = new Vector3(-100f, -100f, -100f);
	
    public Vector3 StartPoint;
    public Vector3 ControlPoint;
    public Vector3 EndPoint;

    public Curve( Vector3 start_point, Vector3 control_point, Vector3 end_point )
    {
        this.StartPoint = start_point;
        this.ControlPoint = control_point;
        this.EndPoint = end_point;
    }

    public Vector3 GetPointAtTime( float BezierTime )
    {
        if (BezierTime >= 1)
        {
            return Curve.ERROR;
        }
        float x = (((1-BezierTime)*(1-BezierTime)) * StartPoint.x) + (2 * BezierTime * (1 - BezierTime) * ControlPoint.x) + ((BezierTime * BezierTime) * EndPoint.x);
        float y = (((1-BezierTime)*(1-BezierTime)) * StartPoint.y) + (2 * BezierTime * (1 - BezierTime) * ControlPoint.y) + ((BezierTime * BezierTime) * EndPoint.y);
        float z = (((1-BezierTime)*(1-BezierTime)) * StartPoint.z) + (2 * BezierTime * (1 - BezierTime) * ControlPoint.z) + ((BezierTime * BezierTime) * EndPoint.z);
		return new Vector3( x, y, z );
    }
}