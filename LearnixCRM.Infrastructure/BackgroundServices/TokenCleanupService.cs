using LearnixCRM.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class TokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public TokenCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var tokenRepository = scope.ServiceProvider
                .GetRequiredService<ISetPasswordRepository>();

            await tokenRepository.DeleteExpiredAsync();

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
