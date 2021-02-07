using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YDK
{
    namespace Misc
    {
        public class LineReader{
            private string str;
            public LineReader(string str){
                this.str = str;
            }
            public string[] GetLines(){
                return str.Split('\n');
            }
        }
        public class Tools{
            public static void Printf(string Text, string[] args){
                for(int i = 0; i < args.Length; i++){
                    Text = Text.Replace("{" + i + "}", args[i]);
                }
                Console.WriteLine(Text);
            }
            public static void Printf(string Text){
                Console.WriteLine(Text);
            }

            public static object Read(string path){
                using(StreamReader reader = new StreamReader(File.OpenRead(path))){
                    string output = reader.ReadToEnd();
                    reader.Close();
                    return output;
                }
            }

            public static object Read(Stream stream)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string output = reader.ReadToEnd();
                    reader.Close();
                    return output;
                }
            }

            public static void Write(string path, byte[] data){
                using(StreamWriter writer = new StreamWriter(path)){
                    writer.Write(data);
                    writer.Close();
                }
            }
            public static void Write(string path, string data){
                using(StreamWriter writer = new StreamWriter(path)){
                    writer.Write(data); 
                    writer.Close();
                }
            }
            public static void Write(string path, object data){
                using(StreamWriter writer = new StreamWriter(path)){
                    writer.Write(data);
                    writer.Close();
                }
            }
        }
    }
}