using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace csvtowav;

public class CommandLineParams
{
    [Value(index: 0, Required = true, HelpText = "Input csv file", MetaName = "input")]
    public string InputFilename { get; set; }

    [Value(index: 1, Required = false, HelpText = "Output wav file", MetaName = "output")]
    public string OutputFilename { get; set; }

    [Option("skip", HelpText = "Skip first N rows in csv file", Default = 0)]
    public int SkipRows { get; set; }

    [Option('c', "column", HelpText = "Data column number, zero based.", Default = 0)]
    public int DataColumn { get; set; }

    [Option("colseparator", HelpText = "Column separator symbol in input csv.\nSingle character like ';' or hex code like '0009'", Default = ",")]
    public string ColumnSeparator { get; set; }

    public char ColumnSeparatorChar { get; set; }

    [Option("samplerate", HelpText = "Output wav samplerate", Default = 44100)]
    public int OutputSamplerate { get; set; }

    [Option("progress", HelpText = "Report progress every N rows", Default = 1_000_000)]
    public int RowProgressReportEvery { get; set; }

    [Option("buffersize", HelpText = "Read buffer size", Default = 4096 * 1024)]
    public int BufferSize { get; set; }
}

internal class CommandLineParser
{
    public static CommandLineParams Parse(string[] args)
    {
        var parseResult = Parser.Default.ParseArguments<CommandLineParams>(args);

        if (parseResult.Errors.Any()) { return null; }

        if (parseResult.Value.ColumnSeparator.Length == 1)
        {
            parseResult.Value.ColumnSeparatorChar = parseResult.Value.ColumnSeparator[0];
        }
        else if (ushort.TryParse(parseResult.Value.ColumnSeparator, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var charCode))
        {
            parseResult.Value.ColumnSeparatorChar = (char)charCode;
        }
        else 
        {
            Console.WriteLine("Invalid column separator format. Use --help for info.");
            return null;        
        }


        return parseResult.Value;
    }
}
