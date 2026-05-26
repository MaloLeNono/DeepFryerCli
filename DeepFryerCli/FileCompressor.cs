using FFMpegCore;

namespace DeepFryerCli;

public class FileCompressor(string inputFilePath, string outputFilePath)
{
    public async Task<bool> CompressAndOutputAsync() =>
        await FFMpegArguments
            .FromFileInput(inputFilePath)
            .OutputToFile(outputFilePath, addArguments: options => options
                .WithAudioBitrate(10)
                .WithVideoBitrate(30)
            ).ProcessAsynchronously();
}