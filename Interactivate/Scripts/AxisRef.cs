using UnityEngine;

namespace Nightbyte.Interactivate
{
    public readonly struct AxisRef
    {
        readonly int i;
        public AxisRef(Axis a) => i = a == Axis.Y ? 1 : a == Axis.Z ? 2 : 0;
        public float Get (Vector3 v)           => v[i];
        public void  Set (ref Vector3 v,float val) => v[i] = val;
    }

    public static class AxisHelper
    {
        static readonly AxisRef[] _single = { new AxisRef(Axis.X), new AxisRef(Axis.Y), new AxisRef(Axis.Z) };
        public static AxisRef AX(Axis a) => _single[(int)a];

        public static Vector3 Mask(Axis a,float v) => a switch
        {
            Axis.X   => new Vector3(v,0,0),
            Axis.Y   => new Vector3(0,v,0),
            Axis.Z   => new Vector3(0,0,v),
            Axis.XY  => new Vector3(v,v,0),
            Axis.XZ  => new Vector3(v,0,v),
            Axis.YZ  => new Vector3(0,v,v),
            Axis.XYZ => new Vector3(v,v,v),
            _        => Vector3.zero
        };

        public static void SetAxes(ref Vector3 vec,Axis ax,float value)
        {
            switch (ax)
            {
                case Axis.X:   vec.x           = value; break;
                case Axis.Y:   vec.y           = value; break;
                case Axis.Z:   vec.z           = value; break;
                case Axis.XY:  vec.x = vec.y   = value; break;
                case Axis.XZ:  vec.x = vec.z   = value; break;
                case Axis.YZ:  vec.y = vec.z   = value; break;
                case Axis.XYZ: vec             = new Vector3(value,value,value); break;
            }
        }
    }
}