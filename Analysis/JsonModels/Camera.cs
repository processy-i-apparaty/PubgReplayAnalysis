using System;
using Analysis.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Analysis.JsonModels
{
    public class Camera
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("group")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.EventGroup Group { get; set; }

        [JsonProperty("meta")] public CameraMeta CameraMeta { get; set; }

        [JsonProperty("time1")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan Time1 { get; set; }
        
        
        [JsonProperty("time2")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan Time2 { get; set; }

        [JsonProperty("data")] public string Data { get; set; }
    }

    public class CameraMeta
    {
        [JsonProperty("DataSetNumber")] public long DataSetNumber { get; set; }

        [JsonProperty("Type")] public string Type { get; set; }

        [JsonProperty("Fov")] public long Fov { get; set; }

        [JsonProperty("IsSkip")] public bool IsSkip { get; set; }

        [JsonProperty("PositionX")] public long PositionX { get; set; }

        [JsonProperty("PositionY")] public long PositionY { get; set; }

        [JsonProperty("PositionZ")] public double PositionZ { get; set; }

        [JsonProperty("RotationX")] public double RotationX { get; set; }

        [JsonProperty("RotationY")] public double RotationY { get; set; }

        [JsonProperty("RotationZ")] public double RotationZ { get; set; }

        [JsonProperty("Angle")] public long Angle { get; set; }

        [JsonProperty("Target")] public string Target { get; set; }

        [JsonProperty("InterpolationType")] public string InterpolationType { get; set; }

        [JsonProperty("CameraDirection")] public string CameraDirection { get; set; }

        [JsonProperty("ReplaySpeed")] public string ReplaySpeed { get; set; }

        [JsonProperty("IsAutoExposureEnable")] public bool IsAutoExposureEnable { get; set; }

        [JsonProperty("DofMethod")] public string DofMethod { get; set; }

        [JsonProperty("DofFocalRegion")] public long DofFocalRegion { get; set; }

        [JsonProperty("IsDofAutoFocus")] public bool IsDofAutoFocus { get; set; }

        [JsonProperty("DofFocalDistance")] public long DofFocalDistance { get; set; }

        [JsonProperty("DofNearTransitionRegion")]
        public long DofNearTransitionRegion { get; set; }

        [JsonProperty("DofFarTransitionRegion")]
        public long DofFarTransitionRegion { get; set; }

        [JsonProperty("DofBokehScale")] public double DofBokehScale { get; set; }

        [JsonProperty("DofBokehMaxSize")] public long DofBokehMaxSize { get; set; }

        [JsonProperty("DofGaussianNearBlurSize")]
        public long DofGaussianNearBlurSize { get; set; }

        [JsonProperty("DofGaussianFarBlurSize")]
        public long DofGaussianFarBlurSize { get; set; }

        [JsonProperty("ColorGradingLUT")] public string ColorGradingLut { get; set; }

        [JsonProperty("ColorGradingLUTIntensity")]
        public long ColorGradingLutIntensity { get; set; }

        [JsonProperty("BloomIntensity")] public long BloomIntensity { get; set; }

        [JsonProperty("VignetteIntensity")] public long VignetteIntensity { get; set; }
    }
}