using System.Runtime.Serialization;

namespace Analysis
{
    public class Enums
    {
        public enum LifeState
        {
            Killed,
            Knocked
        }

        public enum DamageReason
        {
            HeadShot,
            TorsoShot,
            PelvisShot,
            None,
            ArmShot,
            LegShot,
            NonSpecific
        }

        public enum DamageTypeCategory
        {
            [EnumMember(Value = "Damage_Gun")] Gun,
            [EnumMember(Value = "Damage_Groggy")] Groggy,

            [EnumMember(Value = "Damage_Explosion_Grenade")]
            ExplosionGrenade,
            [EnumMember(Value = "Damage_Molotov")] Molotov,

            [EnumMember(Value = "Damage_VehicleHit")]
            VehicleHit,

            [EnumMember(Value = "Damage_BlueZone")]
            BlueZone,

            [EnumMember(Value = "Damage_Instant_Fall")]
            InstantFall,

            [EnumMember(Value = "Damage_Explosion_Vehicle")]
            ExplosionVehicle,

            [EnumMember(Value = "Damage_MeleeThrow")]
            MeleeThrow,
            [EnumMember(Value = "Damage_Punch")] Punch
        }

        public enum EventGroup
        {
            [EnumMember(Value = "ReplaySummary")] ReplaySummary,
            [EnumMember(Value = "level")] Level,
            [EnumMember(Value = "checkpoint")] Checkpoint,
            [EnumMember(Value = "camera")] Camera,
            [EnumMember(Value = "groggy")] Groggy,
            [EnumMember(Value = "kill")] Kill
        }
    }
}