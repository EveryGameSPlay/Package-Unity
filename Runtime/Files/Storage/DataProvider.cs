using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Egsp.Core;

namespace Egsp.Core
{
    /// <summary>
    /// Предоставляет методы для доступа к данным в определенном профиле.
    /// </summary>
    public class DataProvider
    {
        /// <summary>
        /// Используемый профиль.
        /// </summary>
        public readonly DataProfile Profile;

        /// <summary>
        /// Корневая папка, куда сохраняются все файлы.
        /// </summary>
        public readonly string RootFolder;
        
        /// <summary>
        /// Расширения сохраняемых файлов.
        /// </summary>
        public readonly string Extension;
        
        /// <summary>
        /// Сериализатор, используемый провайдером.
        /// </summary>
        public ISerializer Serializer { get; set; }

        public DataProvider(DataProfile profile, string rootFolder, string extension = ".txt")
        {
            Profile = profile;
            
            if(DataProfile.ValidateProfile(profile)==false)
                throw new InvalidDataException();

            RootFolder = rootFolder + "/" + profile.Name+"/";
            Extension = extension;
            
            Serializer = new UnitySerializer();
        }
        
        public DataProvider(DataProfile profile, string rootFolder, ISerializer serializer, string extension = ".txt")
            : this(profile, rootFolder, extension)
        {
            Serializer = serializer;
        }
        
        /// <summary>
        /// Возвращает прокси для файла со свойствами.
        /// При отсутствии файла создает новый, если createDefault == true.
        /// </summary>
        public PropertyFileProxy GetPropertiesFromFile(string filePath, bool createDefault = true)
        {
            var path = RootFolder + filePath + Extension;
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());

            if (File.Exists(path))
            {
                var fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite);

                return new PropertyFileProxy(fs);
            }

            if (createDefault)
            {
                var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                return new PropertyFileProxy(fs);
            }

            throw new FileNotFoundException();
        }

        /// <summary>
        /// Сохраняет любую сущность с помощью сериализации.
        /// </summary>
        public void SetObject<T>(T entity, string file)
        {
            var path = CombineFilePath(file);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());

            SetObjectInternal(entity, path);
        }

        private void SetObjectInternal<T>(T entity, string path)
        {
            var data = Serializer.Serialize(entity);

            // Перезапись старого файла.
            var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bw = new BinaryWriter(fs);
            bw.Write(data);
            
            fs.Close();
        }
        
        /// <summary>
        /// Загружает любую сущность с помощью десериализации.
        /// </summary>
        public Option<T> GetObject<T>(string file)
        {
            var path = CombineFilePath(file);

            if (!File.Exists(path))
                return Option<T>.None;

            var data = File.ReadAllBytes(path);

            var entity = Serializer.Deserialize<T>(data);

            return entity.option;
        }

        public void SetObjects<T>(string file, IEnumerable<T> entities)
        {
            var path = CombineFilePath(file);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
            
            var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            var binaryWriter = new BinaryWriter(fs);

            var data = Serializer.Serialize(entities.ToList());
            binaryWriter.Write(data);

            fs.Close();
        }

        /// <summary>
        /// Загружает все сущности и игнорирует несериализованные значения.
        /// </summary>
        public Option<List<T>> GetObjects<T>(string file)
        {
            var path = CombineFilePath(file);
            if (!File.Exists(path))
                return new List<T>();

            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            var sr = new BinaryReader(fs);

            var list = Serializer.Deserialize<List<T>>(sr.ReadBytes(int.MaxValue));
            if(list.option == null)
                list = Option<List<T>>.None;
            
            fs.Close();
            return list;
        }

        public void SetObjectsToFiles<T>(string directory, IEnumerable<T> entities, Func<T, string> fileName,
            string fileExtension = null)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
                fileExtension = Extension;
            
            var directoryPath = CombineDirectoryPath(directory);
            Directory.CreateDirectory(directoryPath ?? throw new InvalidOperationException());

            var dpForFile = directoryPath + "/";
            
            foreach (var entity in entities)
            {
                var path = dpForFile + fileName(entity) + fileExtension;
                SetObjectInternal(entity, path);
            }
        }

        public LinkedList<T> GetObjectsFromDirectory<T>(string directory, string fileFilter = "*.txt")
        {
            var directoryPath = CombineDirectoryPath(directory);
            Directory.CreateDirectory(directoryPath ?? throw new InvalidOperationException());

            var files = Directory.EnumerateFiles(directoryPath, fileFilter,
                SearchOption.TopDirectoryOnly);

            var linkedList = new LinkedList<T>();
                       
            foreach (var file in files)
            {
                var bytes = File.ReadAllBytes(file);
                var obj = Serializer.Deserialize<T>(bytes);
                
                if(!obj.IsSome || obj.option == null)
                    continue;

                linkedList.AddLast(obj.option);
            }

            return linkedList;
        }

        public string CombineFilePath(string file)
        {
            return RootFolder + file + Extension;
        }

        public string CombineDirectoryPath(string directory)
        {
            return RootFolder + directory;
        }
    }
}