using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PBT.Workbench.Utilities
{
    public static class XmlUtil
    {
        public static void XmlSerialize<T>(string path, T obj, params Type[] types)
        {
            var folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var tempPath = path + ".temp";
            var xs = new XmlSerializer(obj.GetType(), types);
            using (var sw = new StreamWriter(tempPath))
            {
                xs.Serialize(sw, obj);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Move(tempPath, path);
        }

        public static void XmlDeserialize<T>(string path, out T obj, params Type[] types)
        {
            var xs = new XmlSerializer(typeof(T), types);
            using var sr = new StreamReader(path);
            obj = (T)xs.Deserialize(sr);
        }
    }
}
