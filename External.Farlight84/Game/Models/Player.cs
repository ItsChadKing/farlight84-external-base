using System.Numerics;
using System.Text;
using External.Farlight84.Drawing;
using External.Farlight84.Game.Internal.Enums;
using External.Farlight84.Game.Internal.Models;
using External.Farlight84.Memory;

namespace External.Farlight84.Game.Models
{
    internal class Player
    {
        public long ActorMeshPointer { get; set; }
        public long ActorStatePointer { get; set; }
        public float Distance { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Shield { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Vector3 Location { get; set; }
        public Dictionary<PlayerBone, Vector3> Bones { get; set; }

        public float GetHealth()
        {
            return MemoryService.Read<float>(ActorStatePointer + 0x570);
        }

        public float GetMaxHealth()
        {
            return MemoryService.Read<float>(ActorStatePointer + 0x574);
        }

        public float GetShield()
        {
            return MemoryService.Read<float>(ActorStatePointer + 0x57C);
        }

        public float GetTeamId()
        {
            return MemoryService.Read<float>(ActorStatePointer + 0x3F4);
        }

        public string GetPlayerName()
        {
            var fString = MemoryService.Read<FString>(ActorStatePointer + 0x308);
            return MemoryService.ReadString(fString.pBuffer, fString.length*2, Encoding.UTF8);
        }

        public Dictionary<PlayerBone, Vector3> GetBones()
        {
            var bones = new Dictionary<PlayerBone, Vector3>();
            var boneIds = Enum.GetValues(typeof(PlayerBone));

            foreach (PlayerBone bone in boneIds)
            {
                bones[bone] = Renderer.WorldToScreenX(GetBoneWithRotation(Offsets.ComponentToWorld, Offsets.Actor.BoneArray, (int)bone), LocalPlayer.GetInstance.CameraCacheEntry);
            }

            return new Dictionary<PlayerBone, Vector3>(bones);
        }

        public Vector3 GetBoneWithRotation(long componentToWorldOffset, long boneArrayOffset, int boneIndex)
        {
            var bone = GetBoneIndex(ActorMeshPointer, boneArrayOffset, boneIndex);

            if (!bone.HasValue)
            {
                return new Vector3();
            }

            var componentToWorld = MemoryService.Read<FTransform>(ActorMeshPointer + componentToWorldOffset);

            var matrixMultiplied = Renderer.MatrixMultiplication(bone.Value.ToMatrixWithScale(), componentToWorld.ToMatrixWithScale());

            return new Vector3(matrixMultiplied.M41, matrixMultiplied.M42, matrixMultiplied.M43);
        }

        private static FTransform? GetBoneIndex(long aActorMesh, long boneArrayOffset, int boneIndex)
        {
            var aActorSkeletonMesh = MemoryService.Read<long>(aActorMesh + boneArrayOffset);

            if (aActorSkeletonMesh == 0)
            {
                aActorSkeletonMesh = MemoryService.Read<long>(aActorMesh + boneArrayOffset + 0x10);
            }

            if (aActorSkeletonMesh == 0)
            {
                return null;
            }

            return MemoryService.Read<FTransform>(aActorSkeletonMesh + (long)boneIndex * 0x30);
        }
    }
}
