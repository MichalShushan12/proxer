using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRefactorLogRepository
    {
        //קבלת אובייקט עם הקוד המקורי, הקוד החדש, המשתמש וסוג הפעולה ושמירה בטבלה ב DB
        Task<RefactorLog> AddLogAsync(RefactorLog log);

        //בודק אם המשתמש קיים בטבלת המשתמש ב DB, אם הוא לא קיים מחזיר NULL
        Task<User?> GetUserByIdAsync(int userId);
    }
}
