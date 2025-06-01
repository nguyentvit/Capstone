namespace Capstone.Application.Interface.Services
{
    public interface ICSVService
    {
        IEnumerable<T> ReadCSV<T>(Stream file);
        void WriteCSV<T>(List<T> records);
    }
}
