
namespace KirinUtil
{
    public enum ImageFormat { PNG, JPG };

    public enum SpecialFolder
    {
        MyDocuments, Desktop, ProgramFiles, Startup, System
    };

    public enum KinectJoint
    {
        SpineBase = 0,
        SpineMid = 1,
        Neck = 2,
        Head = 3,
        ShoulderLeft = 4,
        ElbowLeft = 5,
        WristLeft = 6,
        HandLeft = 7,
        ShoulderRight = 8,
        ElbowRight = 9,
        WristRight = 10,
        HandRight = 11,
        HipLeft = 12,
        KneeLeft = 13,
        AnkleLeft = 14,
        FootLeft = 15,
        HipRight = 16,
        KneeRight = 17,
        AnkleRight = 18,
        FootRight = 19,
        SpineShoulder = 20,
        HandTipLeft = 21,
        ThumbLeft = 22,
        HandTipRight = 23,
        ThumbRight = 24
    }

    public enum AzureJoint : int
    {
        Pelvis = 0,
        SpineNaval = 1,
        SpineChest = 2,
        Neck = 3,
        Head = 4,

        ClavicleLeft = 5,
        ShoulderLeft = 6,
        ElbowLeft = 7,
        WristLeft = 8,
        HandLeft = 9,

        ClavicleRight = 10,
        ShoulderRight = 11,
        ElbowRight = 12,
        WristRight = 13,
        HandRight = 14,

        HipLeft = 15,
        KneeLeft = 16,
        AnkleLeft = 17,
        FootLeft = 18,

        HipRight = 19,
        KneeRight = 20,
        AnkleRight = 21,
        FootRight = 22,

        Nose = 23,
        EyeLeft = 24,
        EarLeft = 25,
        EyeRight = 26,
        EarRight = 27,

        HandtipLeft = 28,
        ThumbLeft = 29,
        HandtipRight = 30,
        ThumbRight = 31
    }

    public enum Direction
    {
        Up, Down, Left, Right
    }

    public enum FadeType
    {
        FadeIn, FadeOut
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum AnimationType
    {
        Horizon, Vertical, Random, Shuffle, Rotate, Scale
    }

    public enum AxisType
    {
        X, Y, Z, XY, XZ, YZ, XYZ
    }
}
