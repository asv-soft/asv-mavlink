namespace Asv.Mavlink;

public interface IAsvGbsExClient:IAsvGbsCommon
{
    IAsvGbsClient Base { get; }
}