﻿using System.Runtime.Serialization.Formatters.Binary;
using LabFusion.Utilities;

namespace LabFusion.Data
{
    public static class DataSaver
    {
        public static void WriteBinary(string path, object value, BinaryFormatter formatter = null)
        {
            formatter ??= new BinaryFormatter();

            string fullPath = PersistentData.GetPath(path);
            string directoryName = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            FileStream file = File.Create(fullPath);

            formatter.Serialize(file, value);

            file.Close();
        }

        public static T ReadBinary<T>(string path, BinaryFormatter formatter = null)
        {
            formatter ??= new BinaryFormatter();

            string fullPath = PersistentData.GetPath(path);

            if (!File.Exists(fullPath))
                return default;

            FileStream file;

            try
            {
                file = File.Open(fullPath, FileMode.Open);
            }
            catch (UnauthorizedAccessException e)
            {
                FusionLogger.LogException($"reading save data at {path}", e);
                return default;
            }

            try
            {
                T obj = (T)formatter.Deserialize(file);
                file.Close();
                return obj;
            }
            catch
            {
                file.Close();
                return default;
            }
        }
    }
}