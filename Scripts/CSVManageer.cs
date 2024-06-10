using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSVManager : MonoBehaviour
{
    public TextAsset csvFile; // Reference to the CSV file
    public float CameraDir; // Public member for speed
    public float TargetDir; // Public member for direction
    public float DepthView; // Public member for depth
    public float Angle; // Public member for angle
    public static int rowCount = 1; // Static counter to track the number of rows read

    private List<DataRow> dataRows = new List<DataRow>(); // List to hold data rows
    private string filePath; // Path to the CSV file for writing

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "experiment_data.csv");
        LoadCSV();
    }

    void LoadCSV()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
            if (!string.IsNullOrEmpty(data[i]))
            {
                string[] row = data[i].Split(new char[] { ',' });
                DataRow dataRow = new DataRow();

                // Try to parse the Speed
                if (float.TryParse(row[0], out float speed))
                {
                    dataRow.Speed = speed;
                }
                else
                {
                    Debug.LogError($"Failed to parse Speed at row {i}: '{row[0]}'");
                    continue;
                }

                // Try to parse the Direction
                if (float.TryParse(row[1], out float direction))
                {
                    dataRow.Direction = direction;
                }
                else
                {
                    Debug.LogError($"Failed to parse Direction at row {i}: '{row[1]}'");
                    continue;
                }

                // Try to parse the DepthView, if it exists
                if (row.Length > 2 && float.TryParse(row[2], out float depth))
                {
                    dataRow.DepthView = depth;
                }
                else
                {
                    dataRow.DepthView = 20f; // Default value if no depth is provided
                }

                // Try to parse the Angle, if it exists
                if (row.Length > 3 && float.TryParse(row[3], out float angle))
                {
                    dataRow.Angle = angle;
                }
                else
                {
                    dataRow.Angle = 0f; // Default value if no angle is provided
                }

                dataRows.Add(dataRow);
            }
        }
    }

    public bool ReadCSV_row()
    {
        if (rowCount <= dataRows.Count)
        {
            DataRow dataRow = dataRows[rowCount - 1];

            CameraDir = dataRow.Speed;
            TargetDir = dataRow.Direction;
            DepthView = dataRow.DepthView;
            Angle = dataRow.Angle;

            rowCount++;
            return true;
        }
        else
        {
            Debug.Log("END FILE");
            return false;
        }
    }

    public void UpdateCSVWithAngle(float angle)
    {
        if (rowCount > 1 && rowCount <= dataRows.Count + 1)
        {
            dataRows[rowCount - 2].Angle = angle;
            WriteCSV();
        }
    }

    void WriteCSV()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("CameraDir,TargetDir,DepthView,Angle"); // CSV header
            foreach (var row in dataRows)
            {
                writer.WriteLine($"{row.Speed},{row.Direction},{row.DepthView},{row.Angle}");
            }
        }
    }
}

public class DataRow
{
    public float Speed { get; set; }
    public float Direction { get; set; }
    public float DepthView { get; set; }
    public float Angle { get; set; }
}
