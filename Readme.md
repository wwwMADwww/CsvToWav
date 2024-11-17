# CSV to WAV Converter

Yet another converter.

.NET 6 or later required.

## Formats

CSV rows must be placed on separate lines. Column separator defined by `--colseparator` parameter, default `,`.

WAV is single channel IEEE 32 bit Float.

## Usage

```cmd
  --skip             (Default: 0) Skip first N rows in csv file

  -c, --column       (Default: 0) Data column number, zero based.

  --colseparator     (Default: ,) Column separator symbol in input csv.
                     Single character like ';' or hex code like '0009'

  --samplerate       (Default: 44100) Output wav samplerate

  --progress         (Default: 1000000) Report progress every N rows

  --buffersize       (Default: 4194304) Read buffer size

  --help             Display this help screen.

  --version          Display version information.

  input (pos. 0)     Required. Input csv file

  output (pos. 1)    Output wav file
```

### Examples

- Read `test.csv`, write to `test.wav`

    ```cmd
    csvtowav test.csv
    ```

- Read `test.csv`, write to `qwe.wav`

    ```cmd
    csvtowav test.csv qwe.wav
    ```

- Read `test.csv` using tab symbol (char code 9) as column delimiter, write to `qwe.wav`

    ```cmd
    csvtowav test.csv --colseparator 09
    ```

- Read data from second column of file `D:\path with spaces\samples from oscilloscope.csv` skipping first 4 rows, write to `D:\path with spaces\samples from oscilloscope.wav` with samplerate of 1000000

    ```cmd
    csvtowav "D:\path with spaces\samples from oscilloscope.csv" --skip 4 -c 1 --samplerate 1000000
    ```
