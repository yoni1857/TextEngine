using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

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
            XmlSerializer serializer = new XmlSerializer(GetType());
            if (File.Exists(path))
                File.Delete(path);
            using (TextWriter writer = new StreamWriter(File.OpenWrite(path)))
            {
                serializer.Serialize(writer, this);
                writer.Close();
            }
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));
            using (TextReader reader = new StreamReader(File.OpenRead(path)))
            {
                Data returndata = (Data)xmlSerializer.Deserialize(reader);
                reader.Close();
                return returndata;
            }
        }

    }
}
