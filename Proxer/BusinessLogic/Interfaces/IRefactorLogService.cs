using DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IRefactorLogService
    {
        //מקבל מה שהתוסף שלח ממחלקת REQUESTDTO עם הקוד המקורי ואת המ.ז של המשתמש. 
        //בודק אם המשתמש קיים  
        //ויבקש מהבינה המלאכותית לשמור את הלוג החדש בדאטה בייס, ובסוף יחזיר את הקוד החדש שחוזר לתוסף REPOSITORY - יפנה ל
        Task<RefactorResponseDTO> ExecuteRefactorAsync(RefactorRequestDTO request, int userId);
    }
}
