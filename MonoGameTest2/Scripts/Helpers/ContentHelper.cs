using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

using MonoGameTest2.Managers;

namespace MonoGameTest2.Helpers
{
    static class ContentHelper
    {
        public static T[] LoadAll<T>(this ContentManager contentManager, string folder)
        {
            var directory = new DirectoryInfo(Path.Combine(contentManager.RootDirectory, folder));
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            var files = directory.GetFiles();

            var result = new List<T>();
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.Name);
                result.Add(contentManager.Load<T>(Path.Combine(folder, fileName))) ;
            }

            return result.ToArray();
        }
    }
}
 