using System.IO;
using System.Text;
using UnityEngine;

public static class ConvertAudioClipToWav
{
    static float[] _samples = new float[0];
    static short[] _pcm = new short[0];
    static MemoryStream _mem = new();
    static BinaryWriter _writer = new(_mem);
    public static byte[] ConvertToWav(AudioClip clip, int sampleCount, int sampleRate)
    {
        if(sampleCount == 0)
            sampleCount = Mathf.CeilToInt(clip.length * sampleRate);

        if(_samples.Length != sampleCount)
        {
            _samples = new float[sampleCount];
            _pcm = new short[sampleCount];
        }
        clip.GetData(_samples, 0);

        for (int i = 0; i < sampleCount; i++)
            _pcm[i] = (short)(Mathf.Clamp(_samples[i], -1f, 1f) * 32767f);

        _mem = new MemoryStream();
        _mem.SetLength(0);
        _mem.Position = 0;
        _writer = new BinaryWriter(_mem);
        int dataBytes = sampleCount * 2;

        _writer.Write(Encoding.ASCII.GetBytes("RIFF"));
        _writer.Write(36 + dataBytes);
        _writer.Write(Encoding.ASCII.GetBytes("WAVE"));
        _writer.Write(Encoding.ASCII.GetBytes("fmt "));
        _writer.Write(16);
        _writer.Write((short)1);
        _writer.Write((short)1);
        _writer.Write(sampleRate);
        _writer.Write(sampleRate * 2);
        _writer.Write((short)2);
        _writer.Write((short)16);
        _writer.Write(Encoding.ASCII.GetBytes("data"));
        _writer.Write(dataBytes);

        foreach (short sample in _pcm)
            _writer.Write(sample);

        return _mem.ToArray();
    }
}
