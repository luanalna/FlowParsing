using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CSVManager : MonoBehaviour
{
    public TextAsset csvFile; // Reference to the CSV file
    public float CameraDir; // Public member for camera direction
    public float TargetVelocity; // Public member for target speed
    public float TargetDistance; // Public member of target viewing distance
    public float FallAngle; // Public member for fall angle
    public float ResponseAngle; // Public member for response angle
    public static int rowCount = 1; // Static counter to track the number of rows read

    private List<DataRow> dataRows = new List<DataRow>(); // List to hold data rows
    private string filePath; // Path to the CSV file for writing


    /* SUBJE T INFORMATION */
    public string Name_subject = "Gia";
    public string Surname_subject = "Patate";
    public string Number_subject = "6";

    void Start()
    {
        // UpdateSubjectCredentials()
        string file_name =  Name_subject + "_" + Surname_subject + "_N_" + Number_subject + "_exp_data.csv";
        filePath = Path.Combine(Application.dataPath, file_name);
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

                // Try to parse the Camera Direction
                if (float.TryParse(row[0], out float cameraDir))
                {
                    dataRow.CameraDir = cameraDir;
                }
                else
                {
                    Debug.LogError($"Failed to parse Camera Direction at row {i}: '{row[0]}'");
                    continue;
                }

                // Try to parse the Target Velocity
                if (float.TryParse(row[1], out float targetVelocity_in))
                {
                    dataRow.TargetVelocity = targetVelocity_in;
                }
                else
                {
                    Debug.LogError($"Failed to parse Target Speed at row {i}: '{row[1]}'");
                    continue;
                }


                // Try to parse the Fall Angle
                if (float.TryParse(row[2], out float fallAngle))
                {
                    dataRow.FallAngle = fallAngle;
                }
                else
                {
                    Debug.LogError($"Failed to parse Fall Angle at row {i}: '{row[2]}'");
                    continue;
                }

                  // Try to parse the R
                if (float.TryParse(row[3], out float targetDistance))
                {
                    dataRow.TargetDistance = targetDistance;
                }
                else
                {
                    Debug.LogError($"Failed to parse Fall Angle at row {i}: '{row[3]}'");
                    continue;
                } 

                // Initialize the Response Angle to a default value
                dataRow.ResponseAngle = 0f; 

                dataRows.Add(dataRow);
            }
        }
    }

    public bool ReadCSV_row()
    {
        if (rowCount <= dataRows.Count)
        {
            DataRow dataRow = dataRows[rowCount - 1];

            CameraDir = dataRow.CameraDir;
            TargetVelocity = dataRow.TargetVelocity;
            FallAngle = dataRow.FallAngle;
            TargetDistance = dataRow.TargetDistance;
            //ResponseAngle = dataRow.ResponseAngle; // what is .ResponseAngle ??? why this name? is it a function? where is it?

            rowCount++;
            return true;
        }
        else
        {
            Debug.Log("END FILE");
            return false;
        }
    }

    public void UpdateCSVWithAngle(float responseAngle)
    {
        if (rowCount > 1 && rowCount <= dataRows.Count + 1)
        {
            dataRows[rowCount - 2].ResponseAngle = responseAngle;
            WriteCSV();
        }
    }

    void WriteCSV()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("CameraDir,TargetVelocity,FallAngle,TargetDistance,ResponseAngle"); // CSV header
            foreach (var row in dataRows)
            {
                writer.WriteLine($"{row.CameraDir},{row.TargetVelocity},{row.FallAngle},{row.TargetDistance},{row.ResponseAngle}");
            }
        }
    }

}

public class DataRow
{
    public float CameraDir { get; set; }
    public float TargetVelocity { get; set; }
    public float FallAngle { get; set; }
    public float TargetDistance { get; set; }
    public float ResponseAngle { get; set; }
}
