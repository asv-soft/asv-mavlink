namespace Asv.Mavlink;

public class FtpNackEndOfFileException(FtpOpcode action) : FtpNackException(action, NackError.EOF);