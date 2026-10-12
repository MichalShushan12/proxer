using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RefactorLogRepository : IRefactorLogRepository
    {
        private readonly ProxerContext _context;

        public RefactorLogRepository(ProxerContext context)
        {
            _context = context;
        }

        public async Task<RefactorLog> AddLogAsync(RefactorLog log)
        {
            _context.RefactorLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

    }
}
