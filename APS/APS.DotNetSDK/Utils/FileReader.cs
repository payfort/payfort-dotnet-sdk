using System.IO;
using System.Reflection;

namespace APS.DotNetSDK.Utils
{
    public class FileReader
    {
        public static string ReadFromFile(string filePath)
        {
            string path = Path.GetFullPath(filePath);

            return File.ReadAllText(path);
        }

        public static string GetEmbeddedResourceContent(string resource)
        {
            var asm = Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream(resource);
            var source = new StreamReader(stream);
            var fileContent = source.ReadToEnd();

            return fileContent;
        }
    }
}
