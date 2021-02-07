using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using YDK;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace StoryMaker
{
    [Serializable]
    public class Project
    {
        public string Name;
        public readonly DateTime CreationDate;
        public Dictionary<TreeNode, Data> Nodes;

        public Project(string name)
        {
            Name = name;
            Nodes = new Dictionary<TreeNode, Data>();
            Nodes.Add(new TreeNode("index"), new Data());
            CreationDate = DateTime.Now;
        }
        public Project(string name, Dictionary<TreeNode, Data> nodes)
        {
            Name = name;
            if(nodes.Count == 0)
                nodes.Add(new TreeNode("index"), new Data());
            Nodes = nodes;
            CreationDate = DateTime.Now;
        }

        public static void ToFile(string path, Project project)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(File.OpenWrite(path), project); 
        }
        public void toFile(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(File.OpenWrite(path), this);
        }

        public static Project FromFile(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Project)formatter.Deserialize(File.OpenRead(path));
        }

        public ZipArchive ExportToStory(string path = "")
        {
            using (ZipArchive storyBits = ZipFile.Open(path + "\\" + this.Name + ".ypac", ZipArchiveMode.Update))
            {
                foreach (KeyValuePair<TreeNode, Data> Node in Nodes)
                {
                    if (Node.Key.Level != 0)
                    {
                        TreeNode parent = Node.Key.Parent;
                        string extension = "";
                        while (parent != null)
                        {
                            extension += parent.Text;
                            parent = parent.Parent;
                        }
                        using (ZipArchive zipArchive = ZipFile.Open("story" + extension + ".pak", ZipArchiveMode.Update))
                        {
                            ZipArchiveEntry entry = zipArchive.CreateEntry("index.ydkl");
                            Node.Value.ToStream(entry.Open());
                        }

                        storyBits.CreateEntryFromFile("story" + extension + ".pak", "story" + extension + ".pak");
                    }
                    else
                    {
                        using (ZipArchive zipArchive = ZipFile.Open("story.pak", ZipArchiveMode.Update))
                        {
                            ZipArchiveEntry entry = zipArchive.CreateEntry("index.ydkl");
                            Node.Value.ToStream(entry.Open());
                        }

                        storyBits.CreateEntryFromFile("story.pak", "story.pak");
                    }
                }
                ZipArchiveEntry manifest = storyBits.CreateEntry("package.xml");
                Package package = new Package();
                package.Author = "StoryMaker";
                package.Name = this.Name;
                package.ExtractPath = "\\Story";
                using (StringWriter writer = new StringWriter())
                {
                    XmlSerializer serializer = new XmlSerializer(package.GetType());
                    serializer.Serialize(writer, package);
                    using (StreamWriter streamWriter = new StreamWriter(manifest.Open()))
                    {
                        streamWriter.Write(writer.ToString());
                        streamWriter.Close();
                        writer.Close();
                    }
                }
                return storyBits;
            }
        }
    }
}
