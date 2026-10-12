using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class RefactorResponseDTO
    {
        //הקוד המתוקן לאחר בחירת הפעולה שהמתשמש בחר
        public string CorrectedCode { get; set; }

        //האם הקוד החדש עבר בהצלחה
        public bool IsSuccess { get; set; }

        //תקלת שגיאה במקרה שאין הצלחה
        public string? ErrorMessage { get; set; }
    }
}
