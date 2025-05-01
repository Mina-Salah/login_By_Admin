namespace WebApplication3.Services
{
    public interface IDeletedDataService
    {
        // الحصول على البيانات المحذوفة لكل موديل
        Task<List<T>> GetDeletedDataAsync<T>() where T : class;

        // حذف السجلات المحذوفة نهائيًا
        Task DeletePermanentlyAsync<T>(int id) where T : class;

        // استعادة السجلات المحذوفة
        Task RestoreAsync<T>(int id) where T : class;
    }
}
