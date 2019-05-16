// XD
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File = archive.Data.Entities.File;


namespace archive.Services.Storage
{
    public interface IStorageService
    {
        /// <summary>
        /// Odczytaj z bazy danych informacje o pliku i uzupełnij w encji wartość `.Path`.
        /// </summary>
        /// 
        /// <param name="fileId">GUID pliku do odczytania</param>
        /// <returns>Po zakończeniu zwróci znaleziony plik</returns>
        Task<File> Retrieve(Guid fileId);

        /// <summary>
        /// Zapisz plik w StorageService.
        /// </summary>
        /// 
        /// <param name="fileName">domyślna nazwa pliku podczas pobierania (nie musi być unikatowa)</param>
        /// <param name="content">strumień z którego przekopiowana zostanie zawartość pliku</param>
        /// <param name="mimeType">type MIME w formacjie 'typ/podtyp'</param>
        /// <returns>Po zakończeniu zwróci encję nowo utworzonego pliku</returns>
        Task<File> Store(string fileName, Stream content, string mimeType = "application/octet-stream");

        /// <summary>
        /// Zaktualizuj plik, zmieniając tylko jego zawartość.
        /// </summary>
        /// 
        /// <param name="fileId">GUID pliku do modyfikacji</param>
        /// <param name="newContent">strumień, z którego zostanie skopiowana nowa zawartość pliku</param>
        Task Update(Guid fileId, Stream newContent);

        /// <summary>
        /// Usuń informacje o pliku z bazy danych i plik z dysku.
        /// </summary>
        /// 
        /// <param name="fileId">GUID pliku do usunięcia</param>
        Task Delete(Guid fileId);

        /// <summary>
        /// Utwórz nowy zbiór załączników złożony z podanych plików.
        /// </summary>
        /// 
        /// <param name="files">Niepusty biór plików, które muszą już być zapisane w usłudze</param>
        /// <returns>Identyfikator zbiru załączników</returns>
        Task<Guid> CreateFilesGroup(ISet<File> files);

        /// <summary>
        /// Utwórz nowy zbiór załączników na podstawie podanego innego zbioru.
        /// </summary>
        /// 
        /// Utworzony zbiór załaczników będzie miał wszystkie takie pliki jak grupa o podanym identyfikatorze,
        /// oprócz plików ze zbioru <c>with</c>, a dodatkowo jeszcze pliki ze zbioru <c>except</c>.
        /// Jeżeli zbiór załączników miałby być pusty, nie zostanie utworzony -- funkcja zwróci <c>null</c>.
        /// 
        /// Pliki nie są kopiowane, tylko dowiązywane do grupy.
        /// 
        /// <param name="likeGroup">Guid grupy, na podstawie której tworzymy nową</param>
        /// <param name="with">Dodatkowe (w stosunku do kopiowanej grupy) pliki, które mają znaleźć się w grupie.
        ///     Wszystkie muszą już być wcześniej dodane do usługi.</param>
        /// <param name="except">Pliki, które mają zostać pominięte przy kopiowaniu</param>
        /// <returns>Guid nowej grupy, jeżeli jest niepusta; <c>null</c> w przeciwnym wypadku</returns>
        Task<Guid?> CreateFilesGroup(Guid likeGroup, ISet<File> with = null, ISet<File> except = null);

        /// <summary>
        /// Usuń grupę plików, opcjonalnie usuwając tez "nieużywane" pliki.
        /// </summary>
        /// 
        /// Jeżeli <c>removeUnused</c>, to pliki zawarte w podanej grupie i w żadnej innej, zostaną wykasowane.
        /// W szczególności jeżeli pliki są używane w jakiś sposób z pominięciem grupy plików, to też zostaną usunięte.
        /// 
        /// <param name="group">Guid grupy do wykasowania</param>
        /// <param name="removeUnused">Czy wykasować pliki, które były tylko w tej jednej grupie?</param>
        /// <returns>Liczbę usuniętych plików</returns>
        Task<int> DeleteFilesGroup(Guid group, bool removeUnused = true);

        /// <summary>
        /// Zwróć listę wszystkich plików w grupie o podanym GUID.
        /// </summary>
        /// <param name="group">GUID grupy plików zapisanej w usłudze.</param>
        /// <returns>Zbiór plików znajdujących się w grupie o podanym GUID</returns>
        Task<ISet<File>> FilesFromGroup(Guid group);
    }
}
