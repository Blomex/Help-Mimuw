using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using File = archive.Data.Entities.File;


namespace archive.Services
{
    public interface IStorageService
    {
        /// <summary>
        /// Odczytaj z bazy danych informacje o pliku i uzupełnij w encji wartość `.Path`.
        /// </summary>
        /// <param name="fileId">GUID pliku do odczytania</param>
        /// <returns></returns>
        Task<File> Retrieve(Guid fileId);

        /// <summary>
        /// Zapisz plik w StorageService.
        /// </summary>
        /// <param name="fileName">domyślna nazwa pliku podczas pobierania (nie musi być unikatowa)</param>
        /// <param name="content">strumień z którego przekopiowana zostanie zawartość pliku</param>
        /// <param name="mimeType">type MIME w formacjie 'typ/podtyp'</param>
        /// <returns></returns>
        Task<File> Store(string fileName, Stream content, string mimeType = "application/octet-stream");

        /// <summary>
        /// Zaktualizuj plik, zmieniając tylko jego zawartość.
        /// </summary>
        /// <param name="fileId">GUID pliku do modyfikacji</param>
        /// <param name="newContent">strumień, z którego zostanie skopiowana nowa zawartość pliku</param>
        /// <returns></returns>
        Task Update(Guid fileId, Stream newContent);

        /// <summary>
        /// Usuń informacje o pliku z bazy danych i plik z dysku.
        /// </summary>
        /// <param name="fileId">GUID pliku do usunięcia</param>
        /// <returns></returns>
        Task Delete(Guid fileId);
    }
}
