using External.Farlight84.Game.Internal.Enums;

namespace External.Farlight84.Game.Internal.Models
{
    internal class UnrealBoneConnections
    {
        public static readonly List<(PlayerBone, PlayerBone)> BoneConnections = new()
        {
            (PlayerBone.Head, PlayerBone.Neck),
            (PlayerBone.Neck, PlayerBone.SpineOne),

            (PlayerBone.SpineOne, PlayerBone.SpineTwo),
            (PlayerBone.SpineTwo, PlayerBone.SpineThree),

            (PlayerBone.SpineOne, PlayerBone.RightUpperArm),
            (PlayerBone.SpineOne, PlayerBone.LeftUpperArm),
            (PlayerBone.SpineTwo, PlayerBone.Pelvis),

            (PlayerBone.Pelvis, PlayerBone.RightThigh),
            (PlayerBone.Pelvis, PlayerBone.LeftThigh),

            (PlayerBone.RightThigh, PlayerBone.RightCalf),
            (PlayerBone.LeftThigh, PlayerBone.LeftCalf),

            (PlayerBone.RightCalf, PlayerBone.RightFoot),
            (PlayerBone.LeftCalf, PlayerBone.LeftFoot),

            (PlayerBone.RightUpperArm, PlayerBone.RightLowerArm),
            (PlayerBone.LeftUpperArm, PlayerBone.LeftLowerArm),

            (PlayerBone.RightLowerArm, PlayerBone.RightHand),
            (PlayerBone.LeftLowerArm, PlayerBone.LeftHand)
        };
    }
}
