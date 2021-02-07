using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace YDK
{
    [Serializable]
    public class Data
    {
        public string[] values;

        public Data()
        {
            values = new string[512];
        }
        public void SetValue(int id, string value)
        {
            values[id] = value;
        }
        public string GetValue(int id)
        { 
            //throw new Exception("This computer has the gay!");
            return values[id];
            
        }
        public void ToFile(string path)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            if (File.Exists(path))
                File.Delete(path);
            using (FileStream fs = File.OpenWrite(path))
            {
                serializer.Serialize(fs, this);
                fs.Close();
            }
        }
        public void ToStream(Stream stream)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, this);
        }

        public int FindValue(string content)
        {
            for(int i = 0; i< values.Length; i++)
            {
                if(values[i] == content)
                {
                    return i;
                }
            }
            throw new KeyNotFoundException("Value not found.");
        }
        public static Data FromFile(string path)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (FileStream fs = File.OpenRead(path))
            {
                Data returndata = (Data)serializer.Deserialize(fs);
                fs.Dispose();
                return returndata;
            }
        }
        public static Data FromStream(Stream stream)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using (stream)
            {
                Data returndata = (Data)serializer.Deserialize(stream);
                stream.Dispose();
                return returndata;
            }
        }
        // Purely for backwards compatibility purposes.
        public static Data FromXMLFile(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));
            using (TextReader reader = new StreamReader(File.OpenRead(path)))
            {
                Data returndata = (Data)xmlSerializer.Deserialize(reader);
                reader.Dispose();
                return returndata;
            }
           
        }

    }
}
