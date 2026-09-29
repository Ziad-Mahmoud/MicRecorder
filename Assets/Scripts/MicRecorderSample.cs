using System.IO;
using UnityEngine;

public class MicRecorderSample : MonoBehaviour
{
    [SerializeField] private MicRecorder micRecorder;
    [SerializeField] private string fileName;
    private void Awake()
    {
        micRecorder.OnRecordEnd.AddListener(OnRecordEndHandler);
    }
    private void OnDestroy()
    {
        micRecorder.OnRecordEnd.RemoveListener(OnRecordEndHandler);
    }
    private void OnRecordEndHandler(byte[] rawWav)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, rawWav);
    }
}
