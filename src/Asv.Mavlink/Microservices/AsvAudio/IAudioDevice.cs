#nullable enable
using System;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.V2.AsvAudio;
using R3;


namespace Asv.Mavlink;

public class PacketCounter(byte initialCounter = 0)
{
    /// <summary>
    /// Проверяет, является ли новый счетчик инкрементом предыдущего.
    /// Возвращает количество пропущенных значений, если они есть.
    /// </summary>
    /// <param name="newCounter">Новое значение счетчика пакета.</param>
    /// <returns>Количество пропущенных значений (0, если последовательность не нарушена).</returns>
    public int CheckIncrement(byte newCounter)
    {
        int missedPackets = 0;

        // Проверяем переход через 255
        if (initialCounter == 255 && newCounter == 0)
        {
            missedPackets = 0; // Ничего не пропущено, корректный переход через границу
        }
        // Если новый счетчик больше предыдущего на 1
        else if (newCounter == initialCounter + 1)
        {
            missedPackets = 0; // Последовательность не нарушена
        }
        // Если новый счетчик меньше предыдущего (переполнение через 255) или пропущены пакеты
        else
        {
            // Рассчитываем количество пропущенных пакетов
            if (newCounter > initialCounter)
            {
                missedPackets = newCounter - initialCounter - 1;
            }
            else
            {
                // Учитываем переполнение через 255
                missedPackets = (256 - initialCounter) + (newCounter - 1);
            }
        }

        // Обновляем последний счетчик
        initialCounter = newCounter;

        // Возвращаем количество пропущенных пакетов
        return missedPackets;
    }
}

public interface IAudioDevice : IDisposable
{
    MavlinkIdentity FullId { get; }
    Observable<Unit> OnLinePing { get; }
    ReadOnlyReactiveProperty<string> Name { get; }
    void SendAudio(ReadOnlyMemory<byte> pcmRawAudioData);
    AsvAudioCodec RxCodec { get; }
}

