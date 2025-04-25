namespace Nightbyte.Interactivate
{
    public enum InputMode            { Button, Axis }
    public enum ButtonActivationMode { OnKeyDown, OnKeyUp, WhileKeyHeld, HoldForSeconds, Toggle }
    public enum ManipulationType     { Move, Rotate, Scale }
    public enum Axis                 { X, Y, Z, XY, XZ, YZ, XYZ }
}