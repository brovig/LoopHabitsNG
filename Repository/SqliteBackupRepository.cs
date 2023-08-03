using Contracts;
using Entities.SeedDataModels;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public class SqliteBackupRepository : ISqliteBackupRepository
{
    private SqliteBackupContext _sqliteBackupContext;

    public SqliteBackupRepository(SqliteBackupContext sqliteBackupContext)
    {
        _sqliteBackupContext = sqliteBackupContext;
    }

    public async Task<IEnumerable<Habit>> GetAllHabitsFromBackupAsync()
    {
        return await _sqliteBackupContext.Set<Habit>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Repetition>> GetAllRepetitionsFromBackupAsync()
    {
        return await _sqliteBackupContext.Set<Repetition>().AsNoTracking().ToListAsync();
    }
}
