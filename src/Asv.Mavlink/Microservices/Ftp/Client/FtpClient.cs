using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Ftp message payload
    /// Max size = 251
    /// https://mavlink.io/en/services/ftp.html
    /// </summary>
    public class FtpMessagePayload
    {
        public FtpMessagePayload()
        {
            
        }

        public FtpMessagePayload(FileTransferProtocolPayload payload)
        {
            Deserialize(payload);
        }
        
        public FtpMessagePayload(FileTransferProtocolPacket packet)
        {
            Deserialize(packet);
        }
        
        /// <summary>
        /// FTP message payload max size in bytes
        /// </summary>
        private const int MAX_PAYLOAD_SIZE = 251;
        
        /// <summary>
        /// Offset of "Data" buffer in bytes
        /// </summary>
        private const int DATA_BYTES_OFFSET = 12;

        public void Serialize(FileTransferProtocolPacket dest)
        {
            using (var strm = new MemoryStream())
            {
                using (var wrt = new StreamWriter(strm))
                {
                    wrt.Write(SequenceNumber);
                    wrt.Write(Session);
                    wrt.Write((byte)OpCodeId);
                    wrt.Write(Size);
                    wrt.Write((byte)ReqOpCodeId);
                    wrt.Write((byte)(BurstComplete ? 1:0));
                    wrt.Write(Padding);
                    wrt.Write(Offset);
                    strm.Write(Data,0,Data.Length);
                    strm.Position = 0;
                    dest.Payload.Payload = strm.ToArray();
                }
            }
        }

        public void Deserialize(FileTransferProtocolPacket src)
        {
            var buffer = src.Payload.Payload;
            int index = 0;
            SequenceNumber = BitConverter.ToUInt16(buffer, index); index += 2;
            Session = buffer[index]; index += 1;
            OpCodeId = (OpCode)buffer[index]; index += 1;
            Size = buffer[index]; index += 1;
            ReqOpCodeId = (OpCode)buffer[index]; index += 1;
            BurstComplete = buffer[index] != 0; index += 1;
            Padding = buffer[index]; index += 1;
            
            Offset = BitConverter.ToUInt32(buffer, index);
            
            Data = new byte[MAX_PAYLOAD_SIZE - DATA_BYTES_OFFSET];
            Buffer.BlockCopy(src.Payload.Payload,DATA_BYTES_OFFSET,Data,0,Data.Length);
        }
        
        public void Deserialize(FileTransferProtocolPayload src)
        {
            var buffer = src.Payload;
            int index = 0;
            SequenceNumber = BitConverter.ToUInt16(buffer, index); index += 2;
            Session = buffer[index]; index += 1;
            OpCodeId = (OpCode)buffer[index]; index += 1;
            Size = buffer[index]; index += 1;
            ReqOpCodeId = (OpCode)buffer[index]; index += 1;
            BurstComplete = buffer[index] != 0; index += 1;
            Padding = buffer[index]; index += 1;
            
            Offset = BitConverter.ToUInt32(buffer, index);
            
            Data = new byte[MAX_PAYLOAD_SIZE - DATA_BYTES_OFFSET];
            Buffer.BlockCopy(src.Payload,DATA_BYTES_OFFSET,Data,0,Data.Length);
        }

        /// <summary>
        /// Command/response data. Varies by OpCode. This contains the path for operations that act on a file or directory.
        /// For an ACK for a read or write this is the requested information.
        /// For an ACK for a OpenFileRO operation this is the size of the file that was opened.
        /// For a NAK the first byte is the error code and the (optional) second byte may be an error number.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Offsets into data to be sent for ListDirectory and ReadFile commands.
        /// </summary>
        public uint Offset { get; set; }
        
        /// <summary>
        /// 32 bit alignment padding.
        /// </summary>
        public byte Padding { get; set; }

        /// <summary>
        /// Code to indicate if a burst is complete. 1: set of burst packets complete, 0: More burst packets coming.
        /// - Only used if req_opcode is BurstReadFile.
        /// </summary>
        public bool BurstComplete { get; set; }
        
        /// <summary>
        /// OpCode (of original message) returned in an ACK or NAK response.
        /// </summary>
        public OpCode ReqOpCodeId { get; set; }

        /// <summary>
        /// Depends on OpCode. For Reads/Writes this is the size of the data transported. For NAK it is the number of bytes used for error information (1 or 2).
        /// </summary>
        public byte Size { get; set; }
        /// <summary>
        /// Ids for particular commands and ACK/NAK messages.
        /// </summary>
        public OpCode OpCodeId { get; set; }
        /// <summary>
        /// Session id for read/write operations (the server may use this to reference the file handle and information about the progress of read/write operations).
        /// </summary>
        public byte Session { get; set; }
        /// <summary>
        /// All new messages between the GCS and drone iterate this number. Re-sent commands/ACK/NAK should use the previous response's sequence number.
        /// </summary>
        public ushort SequenceNumber { get; set; }
    }
    
    /// <summary>
    /// DTO for ListDirectory command
    /// </summary>
    public class FtpFileListItem
    {
        public int Index { get; set; }
        public string FileName { get; set; }
        public FileItemType Type { get; set; }
    }
    
    public class FtpClientConfig
    {
        /// <summary>
        /// Network ID (0 for broadcast)
        /// </summary>
        public byte TargetNetwork { get; set; } = 0;
    }

    public class FtpFileInfo : FileSystemInfo
    {
        public FtpFileInfo(string name, string parent, bool isdirectory = false, ulong size = 0)
        {
            Name = name;
            isDirectory = isdirectory;
            Size = size;
            Parent = parent;
            FullPath = (Parent.EndsWith("/") ? Parent : Parent + '/') + Name;
        }

        public override bool Exists => true;
        public bool isDirectory { get; set; }
        public override string Name { get; }
        public string Parent { get; }
        public ulong Size { get; set; }

        public override void Delete()
        {
            
        }

        public override string ToString()
        {
            if (isDirectory)
                return "Directory: " + Name;
            return "File: " + Name + " " + Size;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class FtpClient: MavlinkMicroserviceClient, IFtpClient
    {
        private readonly MavlinkClientIdentity _identity;
        private readonly IPacketSequenceCalculator _seq;
        private readonly FtpClientConfig _cfg;
        private int _sessionCounter;
        
        public FtpClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, FtpClientConfig cfg, 
            IPacketSequenceCalculator seq, IScheduler scheduler) : base("FTP", connection, identity, seq, scheduler)
        {
            _cfg = cfg;
            
            OnReceivedPacket = InternalFilteredVehiclePackets.Where(_ => _.Payload is FileTransferProtocolPayload);
        }

        #region Directory methods
        
        public async Task<FtpMessagePayload> ListDirectory(string path, uint offset, byte sequenceNumber, CancellationToken cancel)
        {
            #region DOCS
            //Последовательность операций такова:

            //1. GCS отправляет команду ListDirectory с указанием пути к каталогу и индекса записи.
            //    * В полезной нагрузке должны быть указаны:
            //      data[0]= путь к файлу,
            //      size= длина строки пути,
            //      offset= Индекс первой записи, которую необходимо получить
            //      (0 для первой записи, 1 для второй и т. д.).

            //2. Дрон отвечает ACK, содержащим одну или несколько записей
            //(первая запись указана в offset поле запроса).
            //    * В полезной нагрузке должны быть указаны:
            //      data[0]= Информация для одной или нескольких (последовательных) записей,
            //      начиная с запрошенного индекса записи (offset).
            //      Каждая запись отделяется нулем (\0) и имеет следующий формат
            //      (где type одна из букв F (файл), D (директория), S (пропуск))
            //      <type><file_or_folder_name>\t<file_size_in_bytes>\0
            //
            //          Например, для пяти файлов с именами от TestFile1.xml до TestFile5.xml
            //          записи, возвращаемые по смещению 2, могут выглядеть так:
            //          FTestFile3.xml\t223\0FTestFile4.xml\t755568\0FTestFile5.xml\t11111\0
            //      size= Размер файла data.

            //3. Затем операция повторяется с разными смещениями, чтобы загрузить весь список каталогов.
            //   * Смещение для каждого запроса будет зависеть от того,
            //    сколько записей было возвращено предыдущим запросом (запросами).

            //4. Операция завершается, когда GCS запрашивает индекс записи (offset),
            //превышающий или равный количеству записей. В этом случае дрон отвечает NAK,
            //содержащим EOF (конец файла).

            //GSC должен создать тайм-аут после ListDirectory отправки команды и повторно отправить сообщение
            //по мере необходимости (и описано выше).

            //Дрон также может NAK с неожиданной ошибкой. Как правило, ошибки неисправимы, и дрон должен очистить
            //все ресурсы (т. е. закрыть файловые дескрипторы), связанные с запросом, после отправки NAK.
            #endregion
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                    fillPacket: _ =>
                    {
                        _.Payload.TargetComponent = _identity.TargetComponentId;
                        _.Payload.TargetSystem = _identity.TargetSystemId;
                        _.Payload.TargetNetwork = _cfg.TargetNetwork;

                        var messagePayload = new FtpMessagePayload
                        {
                            Data = MavlinkTypesHelper.GetBytes(path),
                            Size = (byte)path.Length,
                            Offset = offset,
                            OpCodeId = OpCode.ListDirectory,
                            Session = GenerateSession(),
                            SequenceNumber = sequenceNumber
                        };
                        messagePayload.Serialize(_);

                    }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                    timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
            .ConfigureAwait(false);
            
            return new FtpMessagePayload(result);
        }
        
        public async Task CreateDirectory(string path, CancellationToken cancel)
        {
            // Последовательность операций такова:
            
            //1. GCS отправляет команду CreateDirectory , указав полный путь к создаваемой директории.
            //     * В полезной нагрузке должны быть указаны:
            //          data[0]= строка пути к каталогу,
            //          size= длина строки пути к каталогу.
            
            //2. Дрон пытается создать каталог и отвечает на сообщение:
            //     * ACK в случае успеха, содержащий полезную нагрузку size= 0 (т. е. без данных).
            //     * NAK при сбое с информацией об ошибке .
            //     * Дрон должен очистить все ресурсы, связанные с запросом, после отправки ответа.
            
            // GSC не должен создавать тайм-ауты или обрабатывать случай NAK (кроме сообщения об ошибке пользователю).
        }

        public async Task RemoveDirectory(string path, CancellationToken cancel)
        {
            //Последовательность операций такова:
            
            //1. GCS отправляет команду RemoveDirectory , указав полный путь к удаляемому каталогу.
            //    * В полезной нагрузке должны быть указаны:
            //          data[0]= строка пути к каталогу,
            //          size= длина строки пути к каталогу.
            
            //2. Дрон пытается удалить каталог и отвечает на сообщение:
            //    * ACK в случае успеха, содержащий полезную нагрузку size= 0 (т. е. без данных).
            //    * NAK при сбое с информацией об ошибке .
            //    * Дрон должен очистить все ресурсы, связанные с запросом, после отправки ответа.
            
            //GSC должен создать тайм-аут после RemoveDirectory отправки команды и повторно отправить сообщение
            //по мере необходимости (и описано выше ).
        }
        #endregion

        #region File methods
        
        /// <summary>
        /// Reads file from drone to
        /// </summary>
        /// <param name="readPath"></param>
        /// <param name="savePath"></param>
        /// <param name="cancel"></param>
        public async Task ReadFile(string readPath, string savePath, CancellationToken cancel)
        {
            //Последовательность операций такова:
            
            //1. GCS (клиент) отправляет команду OpenFileRO, указывающую путь к файлу для открытия.
            //   В полезной нагрузке должны быть указаны:
            //      data[0]= строка пути к файлу,
            //      size= длина строки пути к файлу.
            
            //2. Дрон (сервер) отвечает либо:
            //  * ACK в случае успеха.
            //  В полезной нагрузке должны быть указаны поля:
            //      session= идентификатор сеанса файла,
            //      size= 4,
            //      data= длина открытого файла.
            //
            //  * NAK с информацией об ошибке, например NoSessionsAvailable, FileExists.
            //  GCS может отменить операцию в зависимости от ошибки.
            
            //3. GCS отправляет команды ReadFile для загрузки фрагмента данных из файла.
            //    Полезная нагрузка должна указывать:
            //      session= текущий сеанс,
            //      size= размер данных для чтения,
            //      offset= позиция в данных для начала чтения
            
            //4. Дрон отвечает на каждое сообщение либо
            //    * ACK в случае успеха.
            //    Поля полезной нагрузки:
            //      data= запрошенный фрагмент данных,
            //      size= размер данных в data поле.
            //
            //    * NAK при сбое с информацией об ошибке .
            
            //5. Приведенная выше последовательность ReadFile/ACK повторяется с разными смещениями
            //   для загрузки всего файла.
            //
            //   В конце концов GCS будет (должна) запросить смещение за конец файла.
            //   Дрон вернет NAK с кодом ошибки EOF.
            //   GCS использует это сообщение, чтобы распознать завершение загрузки.
            
            //6. GCS отправляет TerminateSession , чтобы закрыть файл.
            //  Дрон должен отправить ACK/NAK, но это может (вообще говоря) быть проигнорировано GCS.
            //
            //  GSC должен создать тайм-аут после отправки команд
            //  OpenFileRO и ReadFile повторной отправки сообщений
            //  по мере необходимости (и описано выше ).
            //  Тайм-аут не установлен для TerminateSession(сервер может игнорировать отказ команды или ACK).
            
            await InternalSend<FileTransferProtocolPacket>(pkt =>
            {
                var messagePayload = new FtpMessagePayload
                {
                    Data = Encoding.UTF8.GetBytes(readPath),
                    ReqOpCodeId = OpCode.OpenFileRO,
                    Size = (byte)readPath.Length,
                    OpCodeId = OpCode.OpenFileRO,
                    Session = GenerateSession(),
                    SequenceNumber = _seq.GetNextSequenceNumber()
                };
                messagePayload.Serialize(pkt);
            }, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="savePath"></param>
        /// <param name="cancel"></param>
        public async Task BurstReadFile(string path, string savePath, CancellationToken cancel)
        {
            //Последовательность операций такова:
            
            //1. GCS (клиент) отправляет команду OpenFileRO, указывающую путь к файлу для открытия.
            //  * В полезной нагрузке должны быть указаны:
            //      data[0]= строка пути к файлу,
            //      size= длина строки пути к файлу.
            
            //2. Дрон (сервер) отвечает либо:
            //  * ACK в случае успеха.
            //  В полезной нагрузке должны быть указаны поля:
            //      session= идентификатор сеанса файла,
            //      size= 4,
            //      data= длина открытого файла.
            //
            //  * NAK с информацией об ошибке, например NoSessionsAvailable, FileExists.
            //  GCS может отменить операцию в зависимости от ошибки.
            
            //3. Клиент (т.е. GCS) отправляет команду BurstReadFile
            //с указанием части файла, которую он хочет получить.
            //
            //  * В полезной нагрузке должны быть указаны:
            //      session: текущий идентификатор сеанса,
            //      offset= смещение в файле начала пакета,
            //      data= длина пакета, size= 4.
            
            //4. Сервер (дрон) отвечает либо:
            //  * ACK в случае успеха.
            //  Полезная нагрузка должна указывать поля:
            //      session= идентификатор сеанса файла,
            //      size= 4,
            //      data= длина файла в пакете.
            //
            // * NAK с информацией об ошибке.
            // Клиент может отменить операцию в зависимости от ошибки.
            
            //5. Сервер отправляет клиенту поток данных BurstReadFile (без ACK) до тех пор,
            //пока не будет отправлен весь пакет или не будет достигнут
            //предел размера пакета, определенный сервером.
            //  * Полезная нагрузка должна указывать:
            //      session= текущий сеанс,
            //      size= размер данных для чтения,
            //      offset= положение в исходных данных текущего фрагмента.
            //  * Полезная нагрузка должна указываться burst_complete=0 для всех чанков, кроме последнего,
            //  для которого необходимо установить burst_complete=1.
            
            //6. Клиент повторяет BurstReadFile цикл с разными смещениями,
            //пока не будет получен весь файл.
            
            //7. Клиент должен вести собственный учет полученных (и отсутствующих) фрагментов.
            //Он может запросить любые отсутствующие фрагменты либо в конце пакета, либо в конце файла.
            //Отсутствующие фрагменты можно запросить с помощью BurstReadFile или ReadFile.
            
            //8. Клиент отправляет TerminateSession, чтобы закрыть файл после того,
            //как все фрагменты будут загружены. Сервер должен отправить ACK/NAK,
            //но это может (вообще говоря) быть проигнорировано клиентом.
            
            //Клиент должен создать тайм-аут во время ожидания нового BurstReadFile,
            //а по истечении срока действия может запросить отсутствующие части файла, используя либо BurstReadFile.
            //Тайм ReadFile-аут не установлен для TerminateSession(сервер может игнорировать отказ команды или ACK).
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancel"></param>
        public async Task UploadFile(string path, CancellationToken cancel)
        {
            //Последовательность операций такова:
            
            //1. GCS (клиент) отправляет команду CreateFile, указав путь к файлу, куда файл должен быть загружен.
            //  * В полезной нагрузке должны быть указаны:
            //      data[0]= строка пути к целевому файлу,
            //      size= длина строки пути к файлу.
            
            //2. Дрон (сервер) пытается создать файл и отвечает либо:
            //  * ACK в случае успеха.
            //  В полезной нагрузке должны быть указаны поля:
            //      session= новый идентификатор файловой сессии,
            //      size= 0.
            //
            // * NAK с информацией об ошибке.
            //    * GCS должен отменить всю операцию при ошибке.
            //    * Если на этом этапе возникает ошибка последовательности,
            //      GCS должен отправить команду на ResetSessions.
            
            //3. GCS отправляет команды WriteFile для загрузки блока данных в дрон.
            //  * Полезная нагрузка должна указывать:
            //      session= текущий идентификатор сеанса,
            //      data= фрагмент файла, size= длина data, offset= смещение данных для записи
            
            //4. Дрон отвечает на каждое сообщение либо:
            //    * ACK в случае успеха.
            //    Поля полезной нагрузки:
            //      size= 0.
            //
            //    * NAK при сбое с информацией об ошибке .
            //          GCS должен отменить всю операцию загрузки,
            //          отправив команду, ResetSessions, если есть NAK.
            
            //5. Приведенная выше последовательность WriteFile/ACK повторяется с разными смещениями
            //для загрузки всего файла. Как только GCS определит, что загрузка завершена,
            //он переходит к следующему шагу.
            
            //6. GCS отправляет TerminateSession, чтобы закрыть файл.
            //Дрон должен отправить ACK/NAK, но это может (вообще говоря) быть проигнорировано GCS.
            
            //GSC должен создать тайм-аут после CreateFile отправки WriteFile команд
            //и повторно отправить сообщения по мере необходимости (и описано выше ).
            //Тайм-аут не установлен для TerminateSession(сервер может игнорировать отказ команды или ACK).
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancel"></param>
        public async Task RemoveFile(string path, CancellationToken cancel)
        {
            //Последовательность операций такова:
            
            //1. GCS отправляет команду RemoveFile, указав полный путь к удаляемому файлу.
            //    * В полезной нагрузке должны быть указаны:
            //      data[0]= строка пути к файлу,
            //      size= длина строки пути к файлу.
            
            //2. Дрон пытается удалить файл и отвечает на сообщение:
            //    * ACK в случае успеха, содержащий полезную нагрузку size= 0 (т. е. без данных).
            //
            //    * NAK при сбое с информацией об ошибке.
            //
            //    * Дрон должен очистить все ресурсы, связанные с запросом, после отправки ответа.
            
            //GSC должен создать тайм-аут после RemoveFile отправки команды и повторно отправить сообщение
            //по мере необходимости (и описано выше).
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="offset"></param>
        /// <param name="cancel"></param>
        public async Task TruncateFile(string path, int offset, CancellationToken cancel)
        {
            //Последовательность операций такова:
            
            //1. GCS отправляет команду TruncateFile , указывающую файл для усечения и смещение для усечения.
            //    * В полезной нагрузке должны быть указаны:
            //          data[0]= строка пути к файлу,
            //          size= длина строки пути к файлу,
            //          offset= точка усечения в файле (количество сохраняемых данных).
            
            //2. Дрон пытается обрезать файл и отвечает на сообщение:
            //    * ACK в случае успеха, содержащий полезную нагрузку size= 0 (т. е. без данных).
            //      # Запрос должен быть успешным, если смещение совпадает с размером файла,
            //      и может быть предпринята, если смещение равно нулю (т. е. обрезать весь файл).
            //
            //    * NAK при сбое с информацией об ошибке.
            //      # Запрос должен завершиться ошибкой, если смещение равно 0 (усекать весь файл)
            //      и при обычных ошибках файловой системы.
            //
            //    * Дрон должен очистить все ресурсы, связанные с запросом, после отправки ответа.
            
            //GSC должен создать тайм-аут после TruncateFile отправки команды и повторно отправить сообщение
            //по мере необходимости (и описано выше).
        }
        
        #endregion
        
        public IObservable<IPacketV2<IPayload>> OnReceivedPacket { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private byte GenerateSession()
        {
            return (byte)(Interlocked.Increment(ref _sessionCounter) % byte.MaxValue);
        }
    }
}
