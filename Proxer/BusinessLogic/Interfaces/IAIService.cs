using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAIService
    {
        //קבלת הקוד של המשתמש עם סוג הפעולה שהוא רוצה לבצע על הקוד
        Task<string> RequestRefactorAsync(string originalCode, string actionType, string languageName);
    }
}
