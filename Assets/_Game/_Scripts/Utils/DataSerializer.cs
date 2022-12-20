using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class DataSerializer :ISerializable
{

    private static BinaryFormatter m_binaryFormatter;
    private static FileStream m_file;
    public static string saveLocation;


    public static T Load<T>(string fileName) where T : class
    {
        SetDataPath();

        if (CheckForFile(fileName))
        {
            //Debug.LogWarning ("Save file found");
            try
            {
                m_binaryFormatter = new BinaryFormatter();
                m_file = File.Open(saveLocation + "/" + fileName, FileMode.Open);
                T type = m_binaryFormatter.Deserialize(m_file) as T;
                m_file.Close();
                return type;

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
        return default(T);
    }

    public static void Save<T>(string fileName, T data) where T : class
    {

        SetDataPath();
        m_binaryFormatter = new BinaryFormatter();
        m_file= File.Create(saveLocation + "/" + fileName);
        m_binaryFormatter.Serialize(m_file, data);
        m_file.Close();
    }
    public static bool CheckForFile(string fileName)
    {

        SetDataPath();

        return File.Exists(saveLocation + "/" + fileName);
    }

    private static void SetDataPath()
    {

#if UNITY_EDITOR
        saveLocation = Application.dataPath;
#else
		saveLocation = Application.persistentDataPath;
#endif
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {

    }
}
