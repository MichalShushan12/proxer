using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class RefactorRequestDTO
    {
        //הקוד של המשתמש המקורי שעליו ירוץ סוג הפעולה שהוא רוצה 
        public string OriginalCode { get; set; }

        //ישנם 3 סוגי פעולות: שיפור קוד, שיפור שמות הנתונים ותיקון TYPOS
        public string ActionType { get; set; }

        public string TargetLanguage { get; set; }     
    }
    
}
