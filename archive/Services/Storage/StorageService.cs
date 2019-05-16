using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using archive.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using File = archive.Data.Entities.File;
using Task = System.Threading.Tasks.Task;

namespace archive.Services.Storage
{
    public class StorageService: IStorageService
    {
        /// <summary>
        /// jak głębokie jest drzewo katalogów w STORAGE_DIRECTORY.
        /// </summary>
        /// Na przykład gdy jest równe 3, plik z GUID="030B4A82-1B7C-11CF-9D53-00AA003C9CB6"
        /// jest przechowywany jako "03/0B/42/030B4A82-1B7C-11CF-9D53-00AA003C9CB6"
        public static readonly int SUBDIRECTORY_LEVELS = 3;

        /// <summary>
        /// Nazwa katalogu, w którym zapisywane są przechowywane pliki.
        /// </summary>
        public static readonly string STORAGE_DIRACTORY = "storage";

        public string StorageDirectory { get; }

        protected Data.ApplicationDbContext database_ { get; }

        protected ILogger<StorageService> logger_;

        public StorageService(Data.ApplicationDbContext database, ILogger<StorageService> logger)
        {
            database_ = database;
            logger_ = logger;
        }

        public async Task<File> Retrieve(Guid fileId)
        {
            var fileEntity = await database_.Files.FindAsync(fileId);
            if (fileEntity == null)
                return null;

            var path = GuidToPath(fileId);
            if (!System.IO.File.Exists(path))
                logger_.LogWarning($"File with guid={fileId} could not be found under '{path}', but occurs in database");

            fileEntity.Path = path;
            return fileEntity;
        }

        public async Task<File> Store(string fileName, Stream content, string mimeType="application/octet-stream")
        {
            if (fileName == null || content == null || mimeType == null)
                throw new ArgumentNullException();
            // Simple mimetype validation
            var mime = mimeType.Split("/");
            if (mime.Length != 2)
                throw new ArgumentException($"Invalid mime type: '{mimeType}'");
            if (mime[0].Length > 127)
                throw new ArgumentException($"Mime type too long: '{mime[0]}'");
            if (mime[0].Length > 127)
                throw new ArgumentException($"Mime subtype too long: '{mime[1]}'");

            // Populate the entry in database
            var fileEntity = new Data.Entities.File
            {
                Id = System.Guid.NewGuid(),
                FileName = fileName,
                MimeType = mime[0],
                MimeSubtype = mime[1],
            };
            fileEntity.Path = GuidToPath(fileEntity.Id);

            // Save to database and to storage directory
            using (var transaction = database_.Database.BeginTransaction())
            {
                while (await database_.Files.FindAsync(fileEntity.Id) != null)
                    fileEntity.Id = System.Guid.NewGuid();
                database_.Add(fileEntity);
                await database_.SaveChangesAsync();

                Directory.CreateDirectory(Path.GetDirectoryName(fileEntity.Path));
                using (var stream = System.IO.File.Open(fileEntity.Path, FileMode.CreateNew))
                {
                    await content.CopyToAsync(stream);
                }

                transaction.Commit();
            }

            return fileEntity;
        }

        public async Task Update(Guid fileId, Stream newContent)
        {
            if (await database_.Files.FindAsync(fileId) == null)
                throw new FileNotFoundException($"File with guid={fileId} not found");
            await newContent.CopyToAsync(System.IO.File.Open(GuidToPath(fileId), FileMode.Create));
        }

        public async Task Delete(Guid fileId)
        {
            var fileEntity = await database_.Files.FindAsync(fileId);
            if (await database_.Files.FindAsync(fileId) == null)
                throw new FileNotFoundException($"File with guid={fileId} not found");

            using (var transaction = database_.Database.BeginTransaction())
            {
                database_.Files.Remove(fileEntity);
                await database_.SaveChangesAsync();
                System.IO.File.Delete(GuidToPath(fileId));
                transaction.Commit();
            }
        }

        public async Task<Guid> CreateFilesGroup(ISet<File> files)
        {
            Guid guid = Guid.NewGuid();

            using (var transaction = database_.Database.BeginTransaction())
            {
                while (await database_.FileGroupEntries.FindAsync(guid) != null)
                    guid = Guid.NewGuid();
                foreach (var file in files)
                    database_.FileGroupEntries.Add(new FileGroupEntry {FileId = file.Id, FileGroupId = guid});
                await database_.SaveChangesAsync();
                transaction.Commit();
            }

            return guid;
        }

        public async Task<Guid?> CreateFilesGroup(Guid likeGroup, ISet<File> with = null, ISet<File> except = null)
        {
            ISet<File> filesInNewGroup = with != null ? new HashSet<File>(with) : new HashSet<File>();

            foreach (var file in await FilesFromGroup(likeGroup))
            {
                if (!except.Contains(file))
                    filesInNewGroup.Add(file);
            }

            return await CreateFilesGroup(filesInNewGroup);
        }

        public async Task<int> DeleteFilesGroup(Guid group, bool removeUnused = true)
        {
            var filesInGroup = await FilesFromGroup(group);

            // Remove group
            database_.FileGroupEntries.RemoveRange(
                database_.FileGroupEntries.Where(e => e.FileGroupId == group));
            await database_.SaveChangesAsync();

            int deleted = 0;
            foreach (var file in filesInGroup)
            {
                var refCount = await database_.FileGroupEntries.Where(e => e.FileId == file.Id).CountAsync();
                if (refCount == 0)
                {
                    ++deleted;
                    Delete(file.Id);
                }
            }

            return deleted;
        }

        public async Task<ISet<File>> FilesFromGroup(Guid group)
        {
            var fileEntries = await database_.FileGroupEntries.Where(e => e.FileGroupId == group).ToListAsync();
            var files = new HashSet<File>();
            foreach (var entry in fileEntries)
                files.Add(await Retrieve(entry.FileId));
            return files;
        }

        protected string GuidToPath(Guid guid)
        {
            var guidstr = guid.ToString("N");
            var path = STORAGE_DIRACTORY;
            for (int i = 0; i < SUBDIRECTORY_LEVELS; ++i)
                path += Path.DirectorySeparatorChar + guidstr.Substring(2 * i, 2);
            path += path.Length > 0 ? Path.DirectorySeparatorChar + guidstr : guidstr;
            return path;
        }
    }
}
