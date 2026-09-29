using UnityEngine;
using UnityEngine.Events;

public class MicRecorder : MonoBehaviour
{
    public UnityEvent<byte[]> OnRecordEnd = new();
    private AudioClip record;
    [SerializeField] private int frequency;
    [SerializeField] private int maxLength;
    private int position;
    private bool isRecording = false;
    
    public void StartRecording()
    {
        if(isRecording) { return; }

        record = Microphone.Start(null, false, maxLength, frequency);
        isRecording = true;

        Invoke(nameof(EndRecording), maxLength);
    }
    public void EndRecording()
    {
        CancelInvoke(nameof(EndRecording));

        if(!isRecording) { return; }

        position = Microphone.GetPosition(null);

        Microphone.End(null);
        isRecording = false;

        byte[] rawWav = ConvertAudioClipToWav.ConvertToWav(record, position, frequency);
        OnRecordEnd?.Invoke(rawWav);
    }
}
