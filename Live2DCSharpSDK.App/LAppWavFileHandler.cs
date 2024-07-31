using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class WavFileInfo
{
    public string _fileName;
    public int _numberOfChannels;
    public int _samplingRate;
    public int _bitsPerSample;
    public int _samplePerChannel;
}

public class LAppWavFileHandler
{
    private float[] _pcmData;
    private double _userTimeSeconds;
    private double _lastRms;
    private double _sampleOffset;
    private WavFileInfo _wavFileInfo;
    private ByteReader _byteReader;

    public LAppWavFileHandler()
    {
        _pcmData = Array.Empty<float>();
        this._userTimeSeconds = 0.0;
        this._lastRms = 0.0;
        this._sampleOffset = 0.0;
        this._wavFileInfo = new WavFileInfo();
        this._byteReader = new ByteReader();
    }

    //public static LAppWavFileHandler GetInstance()
    //{
    //    if (s_instance == null)
    //    {
    //        s_instance = new LAppWavFileHandler();
    //    }

    //    return s_instance;
    //}

    //public static void ReleaseInstance()
    //{
    //    if (s_instance != null)
    //    {
    //        s_instance = null!;
    //    }
    //}

    public bool Update(float deltaTimeSeconds)
    {
        int goalOffset;
        float rms;

        // データロード前/ファイル末尾に達した場合は更新しない
        if (_pcmData == null || _sampleOffset >= _wavFileInfo._samplePerChannel)
        {
            _lastRms = 0.0f;
            return false;
        }

        // 経過時間後の状態を保持
        _userTimeSeconds += deltaTimeSeconds;
        goalOffset = (int)Math.Floor(_userTimeSeconds * _wavFileInfo._samplingRate);
        if (goalOffset > _wavFileInfo._samplePerChannel)
        {
            goalOffset = _wavFileInfo._samplePerChannel;
        }

        // RMS計測
        rms = 0.0f;
        for (int channelCount = 0; channelCount < _wavFileInfo._numberOfChannels; channelCount++)
        {
            for (int sampleCount = (int)_sampleOffset; sampleCount < goalOffset; sampleCount++)
            {
                float pcm = _pcmData[sampleCount * _wavFileInfo._numberOfChannels + channelCount];
                rms += pcm * pcm;
            }
        }
        rms = (float)Math.Sqrt(rms / (_wavFileInfo._numberOfChannels * (goalOffset - _sampleOffset)));

        _lastRms = rms;
        _sampleOffset = goalOffset;
        return true;
    }

    public void Start(string filePath)
    {
        // サンプル位参照位置を初期化
        _sampleOffset = 0;
        _userTimeSeconds = 0.0f;

        // RMS値をリセット
        _lastRms = 0.0f;
    }

    public double GetRms()
    {
        return _lastRms;
    }

    public async Task<bool> LoadWavFile(string filePath)
    {
        bool ret = false;

        if (_pcmData != null)
        {
            ReleasePcmData();
        }

        // ファイルロード
        var response = await FetchAsync(filePath);
        if (response != null)
        {
            // Process the response to load PCM data
            ret = await AsyncWavFileManager(filePath);
        }

        return ret;
    }

    private async Task<byte[]> FetchAsync(string filePath)
    {
        //using (var httpClient = new System.Net.Http.HttpClient())
        //{
        //    return await httpClient.GetByteArrayAsync(filePath);
        //}

        // local file
        return System.IO.File.ReadAllBytes(filePath);
    }

    public async Task<bool> AsyncWavFileManager(string filePath)
    {
        bool ret = false;
        _byteReader._fileByte = await FetchAsync(filePath);
        _byteReader._fileDataView = new MemoryStream(_byteReader._fileByte);
        _byteReader._fileSize = _byteReader._fileByte.Length;
        _byteReader._readOffset = 0;

        // Check if file load failed or if there is not enough size for the signature "RIFF"
        if (_byteReader._fileByte == null || _byteReader._fileSize < 4)
        {
            return false;
        }

        // File name
        _wavFileInfo._fileName = filePath;

        try
        {
            // Signature "RIFF"
            if (!_byteReader.GetCheckSignature("RIFF"))
            {
                ret = false;
                throw new Exception("Cannot find Signature 'RIFF'.");
            }
            // File size - 8 (skip)
            _byteReader.Get32LittleEndian();
            // Signature "WAVE"
            if (!_byteReader.GetCheckSignature("WAVE"))
            {
                ret = false;
                throw new Exception("Cannot find Signature 'WAVE'.");
            }
            // Signature "fmt "
            if (!_byteReader.GetCheckSignature("fmt "))
            {
                ret = false;
                throw new Exception("Cannot find Signature 'fmt'.");
            }
            // fmt chunk size
            int fmtChunkSize = (int)_byteReader.Get32LittleEndian();
            // Format ID must be 1 (linear PCM)
            if (_byteReader.Get16LittleEndian() != 1)
            {
                ret = false;
                throw new Exception("File is not linear PCM.");
            }
            // Number of channels
            _wavFileInfo._numberOfChannels = (int)_byteReader.Get16LittleEndian();
            // Sampling rate
            _wavFileInfo._samplingRate = (int)_byteReader.Get32LittleEndian();
            // Data rate [byte/sec] (skip)
            _byteReader.Get32LittleEndian();
            // Block size (skip)
            _byteReader.Get16LittleEndian();
            // Bits per sample
            _wavFileInfo._bitsPerSample = (int)_byteReader.Get16LittleEndian();
            // Skip the extended part of the fmt chunk
            if (fmtChunkSize > 16)
            {
                _byteReader._readOffset += fmtChunkSize - 16;
            }
            // Skip until "data" chunk appears
            while (!_byteReader.GetCheckSignature("data") && _byteReader._readOffset < _byteReader._fileSize)
            {
                _byteReader._readOffset += (int)_byteReader.Get32LittleEndian() + 4;
            }
            // "data" chunk not found in the file
            if (_byteReader._readOffset >= _byteReader._fileSize)
            {
                ret = false;
                throw new Exception("Cannot find 'data' Chunk.");
            }
            // Number of samples
            {
                int dataChunkSize = (int)_byteReader.Get32LittleEndian();
                _wavFileInfo._samplePerChannel = (dataChunkSize * 8) / (_wavFileInfo._bitsPerSample * _wavFileInfo._numberOfChannels);
            }
            // Allocate memory
            _pcmData = new float[_wavFileInfo._numberOfChannels * _wavFileInfo._samplePerChannel];
            // Retrieve waveform data
            for (int sampleCount = 0; sampleCount < _wavFileInfo._samplePerChannel; sampleCount++)
            {
                for (int channelCount = 0; channelCount < _wavFileInfo._numberOfChannels; channelCount++)
                {
                    _pcmData[sampleCount * _wavFileInfo._numberOfChannels + channelCount] = GetPcmSample();
                }
            }

            ret = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return ret;
    }

    public float GetPcmSample()
    {
        int pcm32;

        // Expand to 32-bit width and round to the range -1 to 1
        switch (this._wavFileInfo._bitsPerSample)
        {
            case 8:
                pcm32 = this._byteReader.Get8() - 128;
                pcm32 <<= 24;
                break;
            case 16:
                pcm32 = (int)this._byteReader.Get16LittleEndian() << 16;
                break;
            case 24:
                pcm32 = (int)this._byteReader.Get24LittleEndian() << 8;
                break;
            default:
                // Unsupported bit width
                pcm32 = 0;
                break;
        }

        return pcm32 / 2147483647f; // float.MaxValue;
    }

    private void ReleasePcmData()
    {
        _pcmData = Array.Empty<float>();
    }
}

public class ByteReader
{
    public byte[] _fileByte;
    public MemoryStream _fileDataView;
    public int _fileSize;
    public int _readOffset;

    public int Get8()
    {
        int returnValue = this._fileDataView.ReadByte();
        this._readOffset++;
        return returnValue;
    }

    /**
     * @brief 16ビット読み込み（リトルエンディアン）
     * @return Csm::csmUint16 読み取った16ビット値
     */
    public uint Get16LittleEndian()
    {
        uint ret =
            (uint)(Get8() << 0) |
            (uint)(Get8() << 8);
        return ret;
    }

    /// <summary>
    /// 24ビット読み込み（リトルエンディアン）
    /// </summary>
    /// <returns>読み取った24ビット値（下位24ビットに設定）</returns>
    public uint Get24LittleEndian()
    {
        uint ret =
            (uint)(Get8() << 0) |
            (uint)(Get8() << 8) |
            (uint)(Get8() << 16);
        return ret;
    }

    /// <summary>
    /// 32ビット読み込み（リトルエンディアン）
    /// </summary>
    /// <returns>読み取った32ビット値</returns>
    public uint Get32LittleEndian()
    {
        uint ret =
            (uint)(Get8() << 0) |
            (uint)(Get8() << 8) |
            (uint)(Get8() << 16) |
            (uint)(Get8() << 24);
        return ret;
    }

    /// <summary>
    /// シグネチャの取得と参照文字列との一致チェック
    /// </summary>
    /// <param name="reference">検査対象のシグネチャ文字列</param>
    /// <returns>true 一致している, false 一致していない</returns>
    public bool GetCheckSignature(string reference)
    {
        if (reference.Length != 4)
        {
            return false;
        }

        byte[] getSignature = new byte[4];
        byte[] referenceString = Encoding.UTF8.GetBytes(reference);

        for (int signatureOffset = 0; signatureOffset < 4; signatureOffset++)
        {
            getSignature[signatureOffset] = (byte)Get8();
        }

        return getSignature[0] == referenceString[0] &&
               getSignature[1] == referenceString[1] &&
               getSignature[2] == referenceString[2] &&
               getSignature[3] == referenceString[3];
    }
}
