using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Client
{
    public class MavlinkCommon : MavlinkMicroserviceClient, IMavlinkCommon
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MavlinkCommon(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, IScheduler scheduler):base(connection,identity,seq,"COMMON", scheduler)
        {
        }

        

        public Task SetMode(uint baseMode, uint customMode, CancellationToken cancel)
        {
            Logger.Info($"{LogSend} {nameof(SetMode)}({(MavMode)baseMode:G},{customMode})");
            return InternalSend<SetModePacket>(_ =>
            {
                _.Payload.BaseMode = (MavMode)baseMode;
                _.Payload.CustomMode = customMode;
            }, cancel);
        }

        public Task RequestDataStream(int streamId, int rateHz, bool startStop,CancellationToken cancel)
        {
            Logger.Debug($"{LogSend} {( startStop ? "Enable stream":"DisableStream")} with ID '{streamId}' and rate {rateHz} Hz");
            return InternalSend<RequestDataStreamPacket>(_ =>
            {
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.ReqStreamId = (byte)streamId;
                _.Payload.ReqMessageRate = (byte)rateHz;
                _.Payload.StartStop = (byte)(startStop ? 1 : 0);
            }, cancel);
        }


        public Task SetPositionTargetGlobalInt(uint timeBootMs, MavFrame coordinateFrame, int latInt, int lonInt, float alt,
            float vx, float vy, float vz, float afx, float afy, float afz, float yaw,
            float yawRate, PositionTargetTypemask typeMask, CancellationToken cancel)
        {
            Logger.Debug($"{LogSend} {nameof(SetPositionTargetGlobalInt)} ");
            return InternalSend<SetPositionTargetGlobalIntPacket>(_ =>
            {
                _.Payload.TimeBootMs = timeBootMs;
                _.Payload.TargetSystem = Identity.TargetSystemId;
                _.Payload.TargetComponent = Identity.TargetComponentId;
                _.Payload.CoordinateFrame = coordinateFrame;
                _.Payload.LatInt = latInt;
                _.Payload.LonInt = lonInt;
                _.Payload.Alt = alt;
                _.Payload.Vx = vx;
                _.Payload.Vy = vy;
                _.Payload.Vz = vz;
                _.Payload.Afx = afx;
                _.Payload.Afy = afy;
                _.Payload.Afz = afz;
                _.Payload.Yaw = yaw;
                _.Payload.YawRate = yawRate;
                _.Payload.TypeMask = typeMask;
            }, cancel);
        }
    }
}
