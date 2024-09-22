namespace Asv.Mavlink;

public class FtpEndOfFileException(FtpOpcode action) : FtpNackException(action, NackError.EOF);