using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using NAudio.Wave;

namespace csvtowav;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Start");

        var cmdParams = CommandLineParser.Parse(args);

        if (cmdParams == null) { return; }


        var inputFilename = Path.GetFullPath(cmdParams.InputFilename);

        var outputFilename = cmdParams.OutputFilename ?? 
            Path.Combine(Path.GetDirectoryName(inputFilename), Path.GetFileNameWithoutExtension(inputFilename) + ".wav");

        using var csvReader = new StreamReader(inputFilename, new FileStreamOptions() { 
            BufferSize = cmdParams.BufferSize,
            Options = FileOptions.SequentialScan | FileOptions.Asynchronous
        });


        var wavFormat = WaveFormat.CreateIeeeFloatWaveFormat(cmdParams.OutputSamplerate, 1);

        using var wavWriter = new WaveFileWriter(outputFilename, wavFormat);


        var rowNumber = 0;

        while (true)
        {
            rowNumber++;

            float sample;

            try
            {
                var row = await csvReader.ReadLineAsync();

                if (row == null) { break; }

                if (rowNumber <= cmdParams.SkipRows) { continue; }

                var columns = row.Split(cmdParams.ColumnSeparatorChar);


                if (columns.Length <= cmdParams.DataColumn)
                {
                    sample = default;
                }
                else
                {
                    var colValue = columns[cmdParams.DataColumn];
                    sample = Convert.ToSingle(colValue, CultureInfo.InvariantCulture);
                }

                if (rowNumber % cmdParams.RowProgressReportEvery == 0) { Console.WriteLine($"Processed {rowNumber} rows"); }
            }
            catch (Exception ex) 
            { 
                Console.Error.WriteLine($"Stopping. Row {rowNumber} error: {ex}");
                break;
            }

            wavWriter.WriteSample(sample);
        }

        await wavWriter.FlushAsync();

        Console.WriteLine($"Done. Processed {rowNumber} rows");
    }


}
