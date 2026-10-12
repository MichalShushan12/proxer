using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DTO.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class RefactorLogService : IRefactorLogService
    {
        private readonly IRefactorLogRepository _repository;
        private readonly IAIService _aiService;

        public RefactorLogService(IRefactorLogRepository repository, IAIService aiService)
        {
            _repository = repository;
            _aiService = aiService;
        }

        public async Task<RefactorResponseDTO> ExecuteRefactorAsync(RefactorRequestDTO request, int userId)
        {
            // 1. בדיקה שהמשתמש קיים במערכת
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new RefactorResponseDTO
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found."
                };
            }

            // 2. קריאה ל-AI. קוד השפה הגולמי ('he', 'he-IL' וכו') מומר ל-AIService
            string realAiResult;
            try
            {
                realAiResult = await _aiService.RequestRefactorAsync(
                    request.OriginalCode,
                    request.ActionType,
                    request.TargetLanguage);
            }
            // אם נשלחו יותר מידי הודעות באותו יום וזה עבר את המכסה, חוזרת הודעת שגיאה עם הסבר
            catch (HttpRequestException ex)
            {
                return new RefactorResponseDTO { IsSuccess = false, ErrorMessage = ex.Message };
            }

            // 3. בדיקה אם ה-AI החזיר שגיאה
            if (realAiResult.StartsWith("// Error"))
            {
                return new RefactorResponseDTO { IsSuccess = false, ErrorMessage = realAiResult };
            }

            // 4. שמירת הלוג
            var logEntity = new RefactorLog
            {
                UserId = userId,
                OriginalCode = request.OriginalCode,
                CorrectedCode = realAiResult,
                ActionType = request.ActionType,
                RequestDate = DateTime.UtcNow
            };
            await _repository.AddLogAsync(logEntity);

            // 5. החזרת התשובה
            return new RefactorResponseDTO
            {
                CorrectedCode = realAiResult,
                IsSuccess = true
            };
        }
    }
}
