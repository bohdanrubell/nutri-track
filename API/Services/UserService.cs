using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.Entities;
using NutriTrack.Exceptions;

namespace NutriTrack.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(ApplicationDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> GetUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId");

        if (userId is null) throw new UserIsNotAuthorizedException();

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) throw new UserDoesNotExistException(userId);

        return user;
    }
    
    public async Task<GoalTypeLog> GetLastUsersGoalTypeLog(Guid userId)
    {
        return await _context.GoalTypeLogs
            .Include(u => u.User)
            .Include(goalTypeLog => goalTypeLog.Goal)
            .Where(u => u.User.Id == userId)
            .OrderByDescending(u => u.Date)
            .FirstAsync();
    }
    
    public async Task<ActivityLevelLog> GetLastUserActivityLevelLog(Guid userId)
    {
        return await _context.ActivityLevelLogs
            .Include(u => u.User)
            .Include(activityLevelLog => activityLevelLog.ActivityLevel)
            .Where(u => u.User.Id == userId)
            .OrderByDescending(u => u.Date)
            .FirstAsync();
    }
 
    public async Task<WeightRecord> GetLatestWeightRecordAsync(Guid userId)
    {
        return await _context.WeightRecords
            .Where(w => w.User.Id == userId)
            .OrderByDescending(w => w.DateOfRecordCreated)
            .FirstAsync();
    }
    
}